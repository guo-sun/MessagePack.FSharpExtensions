namespace MessagePack.FSharpGenerator

open System
open System.Reflection
open System.Reflection.Emit
open System.Collections.Generic

open MessagePack

[<AutoOpen>]
module Constants =
    let DictionaryType =
        typeof<Dictionary<_,_>>.GetGenericTypeDefinition().MakeGenericType ([|typeof<Type>; typeof<Object>|])


module TypeReference =
    type FSharpGeneratedResolver() =
        static member Instance = FSharpGeneratedResolver()
        static member formatterCache : IDictionary<Type, Object> = dict []
        interface IFormatterResolver with
            member x.GetFormatter<'T> () =
                match FSharpGeneratedResolver.formatterCache.TryGetValue typeof<'T> with
                | true, formatter -> formatter :?> Formatters.IMessagePackFormatter<'T>
                | _ -> null


module GenerateResolver =
    module GenerateType =
        type ResolverParts = {
            tb : TypeBuilder
            instanceFb : FieldBuilder
            typeFormatterDictFb : FieldBuilder
            getFormatterMb : MethodBuilder
            serializationTpb : GenericTypeParameterBuilder
            formatterTyp : Type
        }

        let createResolverTyp
            (moduleBuilder: ModuleBuilder)
            =
            let tb =
                moduleBuilder.DefineType (
                    "FSharpGeneratedResolver",
                    TypeAttributes.Public ||| TypeAttributes.Class,
                    typeof<Object>,
                    [|typeof<IFormatterResolver>|]
                )

            let instanceFb =
                tb.DefineField (
                    "Instance",
                    tb,
                    FieldAttributes.Public ||| FieldAttributes.Static
                )

            let typeFormatterDictFb =
                tb.DefineField (
                    "formatterCache",
                    DictionaryType,
                    FieldAttributes.Public ||| FieldAttributes.Static
                )

            let getFormatterMb =
                tb.DefineMethod(
                    "GetFormatter",
                    MethodAttributes.Public ||| MethodAttributes.Virtual)

            let getFormatterInterfaceMi =
                typeof<IFormatterResolver>.GetMethod("GetFormatter")

            let genericSerializationType =
                let genericParameters =
                    getFormatterMb.DefineGenericParameters ([|"T"|])

                genericParameters.[0]

            let getFormatterReturnTyp = 
                typeof<Formatters.IMessagePackFormatter<_>>
                    .GetGenericTypeDefinition()
                    .MakeGenericType(genericSerializationType)

            getFormatterMb.SetReturnType(getFormatterReturnTyp)

            tb.DefineMethodOverride(getFormatterMb, getFormatterInterfaceMi)

            {
                tb = tb
                instanceFb = instanceFb
                typeFormatterDictFb = typeFormatterDictFb
                getFormatterMb = getFormatterMb
                serializationTpb = genericSerializationType
                formatterTyp = getFormatterReturnTyp
            }

    module GenerateIL =
        let instanceConstructor
            (tb: TypeBuilder)
            =
            tb.DefineDefaultConstructor (MethodAttributes.Public)

        let staticConstructor
            (tb: TypeBuilder)
            (instanceFb: FieldBuilder)
            (typeFormatterDictFb: FieldBuilder)
            (ctor: ConstructorBuilder)
            =
            let cctor = tb.DefineTypeInitializer()
            let il = cctor.GetILGenerator()
            let dictCtor = (DictionaryType).GetConstructor(Type.EmptyTypes)

            // create instance, set on static field
            il.Emit(OpCodes.Newobj, ctor)
            il.Emit(OpCodes.Stsfld, instanceFb)

            // create dictionary, set on static field
            il.Emit(OpCodes.Newobj, dictCtor)
            il.Emit(OpCodes.Stsfld, typeFormatterDictFb)

            cctor, il

        let dictionaryAdd
            (il: ILGenerator)
            (dictionaryFld: FieldBuilder)
            (keyTyp: Type, formatterObject : Object)
            =
            let dictionaryAddMethod =
                DictionaryType
                    .GetMethod("Add", [|
                        typeof<Type>
                        typeof<Object>
                    |])

            let formatterCtor =
                formatterObject.GetType().GetConstructor(Type.EmptyTypes)

            // load the dictionary field
            il.Emit(OpCodes.Ldsfld, dictionaryFld)

            // load key metadata token
            il.Emit(OpCodes.Ldtoken, keyTyp)

            // create formatter object
            il.Emit(OpCodes.Newobj, formatterCtor)

            // call Dictionary.Add
            il.Emit(OpCodes.Callvirt, dictionaryAddMethod)


        let getFormatterGeneric
            (tb: TypeBuilder)
            (getFormatterMb: MethodBuilder)
            (serializationTpb: GenericTypeParameterBuilder)
            (formatterTyp: Type)
            (typeFormatterDictFb: FieldBuilder)
            =
            let tryGetValue =
                DictionaryType
                    .GetMethod("TryGetValue", [|
                        typeof<Type>
                        typeof<Object>.MakeByRefType()
                    |])

            let il = getFormatterMb.GetILGenerator()

            let tryGetOutFormatter = il.DeclareLocal(typeof<Object>)

            // load dictionary field
            il.Emit(OpCodes.Ldsfld, typeFormatterDictFb)

            // get type token of type argument
            il.Emit(OpCodes.Ldtoken, serializationTpb)

            // load out address
            il.Emit(OpCodes.Ldloca, tryGetOutFormatter)

            // call Dictionary.TryGetValue
            il.Emit(OpCodes.Callvirt, tryGetValue)

            let nullResult = il.DefineLabel()

            // branch
            il.Emit(OpCodes.Brfalse, nullResult)
            
            // found result -- return
            il.Emit(OpCodes.Ldloc, tryGetOutFormatter)

            // cast to IMessagePackFormatter<'T>
            il.Emit(OpCodes.Castclass, formatterTyp)
            il.Emit(OpCodes.Ret)

            // null result
            il.MarkLabel(nullResult)
            il.Emit(OpCodes.Ldnull)
            il.Emit(OpCodes.Ret)


    let createResolverType
        (moduleBuilder: ModuleBuilder)
        (formatters: seq<Type * Object>)
        =
        let typeParts = GenerateType.createResolverTyp moduleBuilder

        let ctor =
            GenerateIL.instanceConstructor
                typeParts.tb

        let (cctor, cctorIl) =
            GenerateIL.staticConstructor
                typeParts.tb 
                typeParts.instanceFb
                typeParts.typeFormatterDictFb
                ctor

        // TODO this should be moved to static GenerateIL.staticConstructor
        for formatter in formatters do
            GenerateIL.dictionaryAdd
                cctorIl
                typeParts.typeFormatterDictFb
                formatter

        cctorIl.Emit(OpCodes.Ret)

        do
            GenerateIL.getFormatterGeneric
                typeParts.tb
                typeParts.getFormatterMb
                typeParts.serializationTpb
                typeParts.formatterTyp
                typeParts.typeFormatterDictFb

        typeParts.tb.CreateType()
