module MessagePack.Tests.AsyncTest

open NUnit.Framework

[<Test>]
let ``async value`` () =

  let input = async.Return(1)
  let actual = convert input
  Assert.AreEqual(1, Async.RunSynchronously actual)
  