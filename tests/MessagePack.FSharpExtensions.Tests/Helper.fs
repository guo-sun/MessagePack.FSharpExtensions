[<AutoOpen>]
module MessagePack.Tests.Helper

open System
open MessagePack
open MessagePack.Resolvers
open MessagePack.FSharp

type WithFSharpDefaultResolverOut(out: string -> unit) =
    interface IFormatterResolver with
        member __.GetFormatter<'T>() =
            out "fsharp get formatter"
            match FSharpResolver.Instance.GetFormatter<'T>() with
            | null ->
                out "Didn't find fsharp resolver"
                StandardResolver.Instance.GetFormatter<'T>()
            | x ->
                out "Found fsharp resolver"
                x

let WithFSharpDefaultResolver() = WithFSharpDefaultResolverOut(ignore)

let convertOut<'T> out (value: 'T) =
    let resolver = WithFSharpDefaultResolverOut(out) :> IFormatterResolver
    let options = MessagePackSerializerOptions.Standard.WithResolver(resolver)
    out "Built options"

    let serialized = MessagePackSerializer.Serialize(value, options)
    let readOnlyMemory = ReadOnlyMemory(serialized)
    MessagePackSerializer.Deserialize<'T>(readOnlyMemory, options)

let convert (value: 'T) =
        convertOut ignore value
