// Learn more about F# at http://fsharp.org

// TODO
// add project reference to FSharpExtensions
// iterate over types here which have the MessagePackObject attribute
//   and are DU's
// call DynamicUnionResolver.GetFormatter<typ>() for each
// grab DynamicUnionResolver.assembly
// write it out with Assembly.save
namespace MessagePack.FSharpGenerator

open System
open System.Collections.Generic
open System.Reflection
open System.Reflection.Emit

open Microsoft.FSharp.Reflection
// open Microsoft.FSharp.Reflection.FSharpReflectionExtensions

module AssemblerTools = 
    let loadAssembly (filePath: string) =
        let loaded = Assembly.LoadFrom filePath
        loaded

    let printall seq =
        seq
            |> Seq.iter (fun x -> printfn "%A" x)

open MessagePack
open MessagePack.Resolvers
open MessagePack.FSharp
open MessagePack.Tests.DUTest

module TypesReference =
    type MessagePack_Tests_DUTest_SimpleUnionFormatter() =
        interface Formatters.IMessagePackFormatter<SimpleUnion> with
            member x.Serialize (writer, value, options) = ()
            member x.Deserialize (reader, options) = SimpleUnion.A

    type FSharpGeneratedFormatter<'T>() =
        static member Formatter : Formatters.IMessagePackFormatter<'T> = null

    type SimpleUnionFormatterCache() =
        inherit FSharpGeneratedFormatter<SimpleUnion>()
        static member Formatter = MessagePack_Tests_DUTest_SimpleUnionFormatter()

    let simpleUnionFormatter = FSharpGeneratedFormatter<SimpleUnion>.Formatter

    // type FSharpGeneratedFormatter'1SimpleUnion () =
    //     static member Formatter = MessagePack_Tests_DUTest_SimpleUnionFormatter()

    type FSharpGeneratedResolver() =
        static member Instance = FSharpGeneratedResolver()
        static member formatterCache : IDictionary<Type, Object> = dict []
        interface IFormatterResolver with
            member x.GetFormatter<'T> () =
                // let formatter = FSharpGeneratedFormatter<'T>.Formatter
                // FIXME how to build concrete type instance? or just create an instance of the generic? how to reference that instance?

                match FSharpGeneratedResolver.formatterCache.TryGetValue typeof<'T> with
                | true, formatter -> formatter :?> Formatters.IMessagePackFormatter<'T>
                | _ -> null

    // type FSharpGeneratedResolver(formatters: (Type * Object) seq) =
    //     member x.ResolverMap = dict formatters
    //     interface IFormatterResolver with
    //         member x.GetFormatter<'T> () =
    //             let typ = typeof<'T>
    //             match x.ResolverMap.TryGetValue typ with
    //             | true, value -> value :?> Formatters.IMessagePackFormatter<'T>
    //             | _ -> StandardResolver.Instance.GetFormatter<'T>()

[<AutoOpen>]
module TypeGeneration =
    let CacheFormatterFieldName = "Formatter"
    let GenericCacheTypeName = "FSharpGeneratedFormatterCache"

    let messagePackFormatterTyp = 
        let t = typeof<Formatters.IMessagePackFormatter<_>>
        t.GetGenericTypeDefinition()

    let createConcreteFormatterCacheType
        (cacheGenericTb: TypeBuilder)
        (formatterObj: Object)
        (serializationType: Type)
        (formatterInstanceFi: FieldInfo)
        =
        let concreteCache = cacheGenericTb.MakeGenericType([|serializationType|])
        let formatterField = TypeBuilder.GetField(concreteCache, formatterInstanceFi)
        formatterField.SetValue (null, formatterObj) // formatter field is static so obj instance is ignored

        concreteCache


    let createConcrete
        (genericCache: Type)
        (serializationTyp: Type)
        (formatterTypCtor: ConstructorInfo)
        (moduleBuilder: ModuleBuilder)
        =
        let tb =
            moduleBuilder.DefineType(
                GenericCacheTypeName,
                TypeAttributes.Public,
                genericCache)
        let concreteTyp = genericCache.MakeGenericType(serializationTyp)
        // let parentCtor = TypeBuilder.GetConstructor(concreteTyp, genericCache.GetConstructor(Type.EmptyTypes))
        let ctorIL = 
            let ctor =
                tb.DefineConstructor (
                    MethodAttributes.Public,
                    CallingConventions.Standard,
                    Type.EmptyTypes
                )

            ctor.GetILGenerator()

        let formatterInstanceField = tb.GetField(CacheFormatterFieldName)

        // store formatter in field
        ctorIL.Emit(OpCodes.Ldarg_0)
        ctorIL.Emit(OpCodes.Newobj, formatterTypCtor)
        ctorIL.Emit(OpCodes.Stfld, formatterInstanceField)
        ctorIL.Emit(OpCodes.Ret)

        tb.CreateType()

    let createCacheGenericType (moduleBuilder: ModuleBuilder) =
        let tb =
            moduleBuilder.DefineType (
                GenericCacheTypeName,
                TypeAttributes.Public)

        let serializationType =
            let typeParams = tb.DefineGenericParameters ([|"'T"|])
            typeParams.[0]

        let formatterInstanceFb =
            tb.DefineField (
                CacheFormatterFieldName,
                messagePackFormatterTyp.MakeGenericType(serializationType),
                FieldAttributes.Public ||| FieldAttributes.Static)

        let ctorIL =
            let ctor = tb.DefineConstructor (MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes)
            ctor.GetILGenerator()

        ctorIL.Emit(OpCodes.Ldarg_0)
        ctorIL.Emit(OpCodes.Ldnull)
        ctorIL.Emit(OpCodes.Stfld, formatterInstanceFb)
        ctorIL.Emit(OpCodes.Ret)

        tb.CreateType(), tb, formatterInstanceFb


module TestFromHere =
    type WithFSharpDefaultResolver() =
      interface IFormatterResolver with
        member __.GetFormatter<'T>() =
          match FSharpResolver.Instance.GetFormatter<'T>() with
          | null -> StandardResolver.Instance.GetFormatter<'T>()
          | x -> x

    let convert (value: 'T) =
        let resolver = WithFSharpDefaultResolver() :> IFormatterResolver
        let options = MessagePackSerializerOptions.Standard.WithResolver(resolver)

        let bin = ReadOnlyMemory(MessagePackSerializer.Serialize<'T>(value, options))

        MessagePackSerializer.Deserialize<'T>(bin, options)

[<AutoOpen>]
module Predicates =
    let isDUNullCase (typ: Type) =
        let onlyCaseTypeBound = BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.DeclaredOnly
        let items = typ.GetProperties(onlyCaseTypeBound)
        items.Length = 0

    let isDU (typ: Type) =
        let isDUTypeOrCase = FSharpType.IsUnion typ
        isDUTypeOrCase && not (isDUNullCase typ)


    let isNesteddInternal (typ: Type) =
        typ.IsNestedAssembly

    let isSubtype (typ: Type) =
        typ.DeclaringType <> typ

    let isInternal (typ: Type) =
        typ.IsNotPublic

module GenerateFormatters =
    let resolver =
        CompositeResolver.Create (
            FSharpResolver.Instance,
            StandardResolver.Instance
        )

    let getFormatterWithVerify (typ: Type) =
        let mi =
            typeof<FormatterResolverExtensions>
                .GetMethod(
                    "GetFormatterWithVerify",
                    BindingFlags.Static ||| BindingFlags.Public)

        let getFormat = mi.MakeGenericMethod(typ)

        getFormat.Invoke(null, [|resolver|])

    let testsAssembly = (typeof<MessagePack.Tests.DUTest.SimpleUnion>).Assembly

    let formatters = seq {
        for typ in testsAssembly.GetTypes() do
            let skipTyp =
                isDUNullCase typ ||
                isNesteddInternal typ

            if not (skipTyp) then
                printfn "Getting formatter for %A" typ

                let formatter = getFormatterWithVerify typ // instance

                if isNull formatter then
                    printfn "  Couldn't get formatter"
                else
                    printfn "  Got formatter"
                    yield (typ, formatter)
            else
                printfn "Skipping %A" typ
    }

    [<EntryPoint>]
    let main argv =
        let outAssembly = DiscriminatedUnionResolver.assembly

        let resolverTyp =
            GenerateResolver.createResolverType
                outAssembly.ModuleBuilder
                formatters

        ignore <| DiscriminatedUnionResolver.Instance.Save()
        ignore <| DynamicEnumResolver.Instance.Save()
        ignore <| DynamicObjectResolver.Instance.Save()
        ignore <| DynamicUnionResolver.Instance.Save()

        0
