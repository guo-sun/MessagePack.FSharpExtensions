// Learn more about F# at http://fsharp.org

// TODO
// add project reference to FSharpExtensions
// iterate over types here which have the MessagePackObject attribute
//   and are DU's
// call DynamicUnionResolver.GetFormatter<typ>() for each
// grab DynamicUnionResolver.assembly
// write it out with Assembly.save

open System
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
    type FSharpGeneratedFormatter<'T>() =
        static member Formatter : Formatters.IMessagePackFormatter<'T> = null
    
    type MessagePack_Tests_DUTest_SimpleUnionFormatter() =
        interface Formatters.IMessagePackFormatter<SimpleUnion> with
            member x.Serialize (writer, value, options) = ()
            member x.Deserialize (reader, options) = SimpleUnion.A

    type FSharpGeneratedFormatter'1SimpleUnion () =
        static member Formatter = MessagePack_Tests_DUTest_SimpleUnionFormatter()

    type FSharpGeneratedResolver() =
        static member Instance = FSharpGeneratedResolver()
        interface IFormatterResolver with
            member __.GetFormatter<'T> () =
                let formatter = FSharpGeneratedFormatter<'T>.Formatter

                if not (isNull formatter) then
                    formatter
                else
                    null

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

    let messagePackFormatterTyp = 
        let t = typeof<Formatters.IMessagePackFormatter<_>>
        t.GetGenericTypeDefinition()

    let createConcreteFormatterCacheType
        (cacheGenericTyp: Type)
        (formatterObj: Object)
        (serializationType: Type)
        =
        let concreteCache = cacheGenericTyp.MakeGenericType([|serializationType|])
        let formatterField = concreteCache.GetField(CacheFormatterFieldName)
        formatterField.SetValue (null, formatterObj) // formatter field is static so obj instance is ignored

        concreteCache

    // Unused
    let assignFormatterInstance
        (formatterTyp: Type)
        (formatterFb: FieldBuilder)
        (il: ILGenerator)
        =
        let ctor = formatterTyp.GetConstructor(Type.EmptyTypes)
        il.Emit(OpCodes.Newobj, ctor)
        il.Emit(OpCodes.Stfld, formatterFb)

    let createCacheGenericType (moduleBuilder: ModuleBuilder) =
        let tb =
            moduleBuilder.DefineType (
                "FSharpGeneratedFormatter",
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

        tb.CreateType()


module TestFromHere =
    // TODO
    // define this on the dynamic assembly
    // foreach typ (where typ is Type of the formatter)
    // makegenerictype of this, on typ
    // set Formatter to an instance of typ

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

[<EntryPoint>]
let main argv =
    let resolver =
        CompositeResolver.Create (
            FSharpResolver.Instance,
            StandardResolver.Instance
        )

    let getFormatterWithVerify (typ: Type) =
        let resolverTyp = typeof<FormatterResolverExtensions>
        let mi = resolverTyp.GetMethod("GetFormatterWithVerify", BindingFlags.Static ||| BindingFlags.Public)
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

    let outAssembly = DiscriminatedUnionResolver.assembly

    let cacheGenericTyp = createCacheGenericType outAssembly.ModuleBuilder
    printfn "Made generic"

    for (serializationTyp, formatterObj) in formatters do
        printfn "Making concrete for %A" serializationTyp
        let concreteCacheTyp = createConcreteFormatterCacheType cacheGenericTyp formatterObj
        printfn "Made concrete: %A" concreteCacheTyp

    // build resolver
    // let generatedResolver = FSharpGeneratedResolver(dict formatters)
    // need to generate a resolver type
    // define it on a module in outAssembly
    //
    // okay, looks like i'd need to write the il to generate the initial dict value
    // in any case, probably better to create static generics
    // for each typ:
    // formatterType = makeGenericType(typ, baseType)
    // then just add a resolver type where GetFormatter<'T> = baseType<'T>.getFormatter


    // let generatedResolverType =
    //     let tb =
    //         outAssembly.ModuleBuilder.DefineType(
    //             "FSharpGeneratedResolver",
    //             TypeAttributes.Class ||| TypeAttributes.Public,
    //             typeof<FSharpGeneratedResolver>)


    //     let ctor = tb.DefineConstructor(
    //         MethodAttributes.Public,
    //         CallingConventions.Standard,
    //         [|typeof<unit>|]
    //     )

    //     let ctorIL = ctor.GetILGenerator()
        // construct FSharpGeneratedFormatter<'T> foreach typ

        // todo call FSharpGeneratedFormatter<'T>.GetFormatter()

    //     // dragons
    //     let formatterDict = ctorIL.DeclareLocal

    outAssembly.AssemblyBuilder.Save("whatever.dll")
    0

    // for domainAssembly in AppDomain.CurrentDomain.GetAssemblies() do
    //     printfn "Assembly: %A" domainAssembly
    //     for referencedAssembly in domainAssembly.GetReferencedAssemblies() do
    //         printfn "- References: %A" referencedAssembly

    //     for mmodule in domainAssembly.GetModules(true) do
    //         printfn "  Module: %A -- PEKind: %A" mmodule (mmodule.GetPEKind())

    //     // for typ in domainAssembly.GetExportedTypes() do
    //     //     printfn "  Type: %A" typ
    