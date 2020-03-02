module MessagePack.Tests.UnitTest

open NUnit.Framework

[<Test>]
let ``unit value`` () =

  let input = ()
  let actual = convert input
  Assert.AreEqual(input, actual)
