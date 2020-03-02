module MessagePack.Tests.CollectionTest

open NUnit.Framework

[<Test>]
let ``fsharp list`` () =

  let input: int list = []
  let actual = convert input
  Assert.AreEqual(box input, box actual)

  let input = [1]
  let actual = convert input
  Assert.AreEqual(box input, box actual)

[<Test>]
let ``fsharp map`` () =

  let input= Map.empty<int, bool>
  let actual = convert input
  Assert.AreEqual(box input, box actual)

  let input = Map.empty |> Map.add 0 true
  let actual = convert input
  Assert.AreEqual(box input, box actual)

[<Test>]
let ``fsharp set`` () =

  let input = Seq.empty<int>
  let actual = convert input
  Assert.AreEqual(box input, box actual)

  let input = Seq.singleton 1
  let actual = convert input
  Assert.AreEqual(box input, box actual)

