[<AutoOpen>]
module MessagePack.Tests.Helper

open System
open MessagePack
open MessagePack.Resolvers
open MessagePack.FSharp

type WithFSharpDefaultResolver() =
  interface IFormatterResolver with
    member __.GetFormatter<'T>() =
      match FSharpResolver.Instance.GetFormatter<'T>() with
      | null -> StandardResolver.Instance.GetFormatter<'T>()
      | x -> x

let convert<'T> (value: 'T) =
  let resolver = WithFSharpDefaultResolver() :> IFormatterResolver
  let options = MessagePackSerializerOptions.Standard.WithResolver(resolver)
  
  printfn "Building binary"
  let bin = ReadOnlyMemory(MessagePackSerializer.Serialize(value, options))

  MessagePackSerializer.Deserialize<'T>(bin, options)

let roundTrip x =
    let actual = convert x
    Assert.Equal(actual, x, "roundtrip")