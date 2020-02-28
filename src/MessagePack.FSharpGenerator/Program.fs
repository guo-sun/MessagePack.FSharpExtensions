namespace MessagePack.FSharpGenerator

open System
open System.Collections.Generic
open System.Reflection
open System.Reflection.Emit

open Microsoft.FSharp.Reflection

open MessagePack
open MessagePack.Resolvers
open MessagePack.FSharp
open MessagePack.Tests.DUTest

[<AutoOpen>]
module Predicates =
    let isDUNullCase (typ: Type) =
        (*
            type DU =
                | DUNullCase
                | DUWithIntItem of int
         *)
        let onlyCaseTypeBound = BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.DeclaredOnly
        let items = typ.GetProperties(onlyCaseTypeBound)
        items.Length = 0

    let isNesteddInternal (typ: Type) =
        // TODO I think this ends up covering the DU null case as well
        typ.IsNestedAssembly


module GenerateFormatters =
    let testsAssembly = (typeof<MessagePack.Tests.DUTest.SimpleUnion>).Assembly

    let resolver =
        CompositeResolver.Create (
            FSharpResolver.Instance,
            StandardResolver.Instance
        )

    let getFormatterWithVerify (typ: Type) =
        let getFormat = 
            let mi =
                typeof<FormatterResolverExtensions>
                    .GetMethod(
                        "GetFormatterWithVerify",
                        BindingFlags.Static ||| BindingFlags.Public)

            mi.MakeGenericMethod(typ)

        getFormat.Invoke(null, [|resolver|])

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

        // TODO
        // these depend on NETFRAMEWORK targets and code changes upstream
        // specify in fsproj
        ignore <| DiscriminatedUnionResolver.Instance.Save()
        ignore <| DynamicEnumResolver.Instance.Save()
        ignore <| DynamicObjectResolver.Instance.Save()
        ignore <| DynamicUnionResolver.Instance.Save()

        printfn "Saved output dlls"

        0
