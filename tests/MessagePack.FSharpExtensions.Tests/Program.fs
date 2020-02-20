open MessagePack.Tests.DUTests
open Xunit.Abstractions

type OutputHelper() =
    interface ITestOutputHelper with
        member __.WriteLine (message) =
            printfn "%s" message

        member __.WriteLine (format, args) =
            printfn (new Printf.TextWriterFormat<_>(format)) args


module Program =

    [<EntryPoint>]
    let main _ =
        let outputHelper = OutputHelper()
        let test = DUTest(outputHelper)
        test.simple()
        0