module MessagePack.Tests.OptionTest

open NUnit.Framework

[<Test>]
let some () =

  let input = Some 1
  let actual = convert input
  Assert.AreEqual(input, actual)

[<Test>]
let none () =

  let input: int option = None
  let actual = convert input
  Assert.AreEqual(input, actual)
