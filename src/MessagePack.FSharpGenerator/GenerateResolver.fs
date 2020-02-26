namespace MessagePack.FSharpGenerator

open System
open System.Reflection
open System.Reflection.Emit
open System.Collections.Generic

open MessagePack

module TypeReference =
    type LookForThisType() =
        static member aDictionary = dict [
            "a", 0
            "b", 1
        ]

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


module GenerateResolver =
    module GenerateType =
        type ResolverParts = {
            tb : TypeBuilder
            instanceFb : FieldBuilder
            typeFormatterDictFb : FieldBuilder
            getFormatterMb : MethodBuilder
            serializationT'pb : GenericTypeParameterBuilder
        }

        let createResolverTyp
            (moduleBuilder: ModuleBuilder)
            =
            let tb =
                moduleBuilder.DefineType (
                    "FSharpGeneratedResolver",
                    TypeAttributes.Public ||| TypeAttributes.Class,
                    typeof<IFormatterResolver>
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
                    typeof<IDictionary<Type, Object>>,
                    FieldAttributes.Public ||| FieldAttributes.Static
                )

            let getFormatterMb =
                tb.DefineMethod(
                    "GetFormatter",
                    MethodAttributes.Public
                )

            let genericSerializationType'T =
                let genericParameters = getFormatterMb.DefineGenericParameters ([|"'T"|])
                genericParameters.[0]

            {
                tb = tb
                instanceFb = instanceFb
                typeFormatterDictFb = typeFormatterDictFb
                getFormatterMb = getFormatterMb
                serializationT'pb = genericSerializationType'T
            }

    module GenerateIL =
        let staticConstructor
            (tb: TypeBuilder)
            (instanceFb: FieldBuilder)
            (typeFormatterDictFb: FieldBuilder)
            =
            let ctor = tb.GetConstructor(Type.EmptyTypes)
            let cctor = tb.DefineTypeInitializer()
            let il = cctor.GetILGenerator()
            let dictCtor = (typeof<Dictionary<Type, Object>>).GetConstructor(Type.EmptyTypes)

            // create instance, set on static field
            il.Emit(OpCodes.Newobj, ctor)
            il.Emit(OpCodes.Stsfld, instanceFb)

            // create dictionary, set on static field
            il.Emit(OpCodes.Newobj, dictCtor)
            il.Emit(OpCodes.Stsfld, typeFormatterDictFb)
            cctor

        let dictionaryAdd
            (il: ILGenerator)
            (dictionaryFld: FieldBuilder)
            (keyTyp: Type, formatterObject : Object)
            =
            let dictionaryAddMethod =
                let typ = typeof<IDictionary<Type, Object>>
                typ.GetRuntimeMethod("Add", [|typeof<Type>; typeof<Object>|])

            let formatterCtor =
                formatterObject.GetType().GetConstructor(Type.EmptyTypes)

            // load the dictionary field
            il.Emit(OpCodes.Ldfld, dictionaryFld)

            // load key metadata token
            il.Emit(OpCodes.Ldtoken, keyTyp)
            // create formatter object
            il.Emit(OpCodes.Newobj, formatterCtor)

            // call Dictionary.Add mi
            il.Emit(OpCodes.Call, dictionaryAddMethod)

        let getFormatterGeneric
            (tb: TypeBuilder)
            (typeFormatterDictFb: FieldBuilder)
            =
            let tryGetValue =
                typeof<Dictionary<Type, Object>>
                    .GetMethod("TryGetValue", [|
                        typeof<Type>
                        typeof<Object>.MakeByRefType()
                    |])

            let getFormatterMb =
                tb.DefineMethod (
                    "GetFormatter",
                    MethodAttributes.Public ||| MethodAttributes.Static
                )

            let serializationTypeParam =
                let genericParams =
                    getFormatterMb.DefineGenericParameters([|"'T"|])

                genericParams.[0]

            getFormatterMb.SetParameters serializationTypeParam

            let getFormatterReturnTyp = 
                typeof<Formatters.IMessagePackFormatter>.MakeGenericType(serializationTypeParam)

            getFormatterMb.SetReturnType getFormatterReturnTyp

            let il = getFormatterMb.GetILGenerator()

            let tryGetOutFormatter = il.DeclareLocal typeof<Object>

            // load dictionary field
            il.Emit(OpCodes.Ldsfld, typeFormatterDictFb)

            // get type token of type argument
            il.Emit(OpCodes.Ldtoken, serializationTypeParam)

            // load out address
            il.Emit(OpCodes.Ldloca, tryGetOutFormatter)

            // call Dictionary.TryGetValue
            il.Emit(OpCodes.Callvirt, tryGetValue)

            let nullResult = il.DefineLabel ()

            // branch
            il.Emit(OpCodes.Brfalse, nullResult)
            
            // found result -- return
            il.Emit(OpCodes.Ldloc, tryGetOutFormatter)

            // cast to IMessagePackFormatter<'T>
            il.Emit(OpCodes.Castclass, getFormatterReturnTyp)
            il.Emit(OpCodes.Ret)

            // null result
            il.MarkLabel nullResult
            il.Emit(OpCodes.Ldnull)
            il.Emit(OpCodes.Ret)


    let createResolverType
        (moduleBuilder: ModuleBuilder)
        (formatters: seq<Type * Object>)
        =
        let typeParts = GenerateType.createResolverTyp moduleBuilder

        let cctor =
            GenerateIL.staticConstructor
                typeParts.tb 
                typeParts.instanceFb
                typeParts.typeFormatterDictFb

        for formatter in formatters do
            GenerateIL.dictionaryAdd
                (cctor.GetILGenerator())
                typeParts.typeFormatterDictFb
                formatter

        do
            GenerateIL.getFormatterGeneric
                typeParts.tb
                typeParts.typeFormatterDictFb

        typeParts.tb.CreateType()
