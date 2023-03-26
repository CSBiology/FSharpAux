module DictionaryTests

open Expecto
open FSharpAux
open System.Collections.Generic


let testDic1 = new Dictionary<int,string>()
testDic1.Add(1, "Hello")
testDic1.Add(2, "World")
testDic1.Add(3, "  World  ")
let testDicRec = new Dictionary<int,Dictionary<int,string>>()
let testDicRecInner = new Dictionary<int,string>()
testDicRecInner.Add(1, "inner string")
testDicRec.Add(1, testDicRecInner)


[<Tests>]
let dictTests =
    testList "DictionaryTests" [
        testList "Dictionary.tryGetValue" [
            testCase "Is Some" <| fun _ ->
                Expect.isSome (Dictionary.tryGetValue 2 testDic1) "Is None"
            testCase "Is None when expected" <| fun _ ->
                Expect.isNone (Dictionary.tryGetValue 4 testDic1) "Is Some"
            testCase "Returns correct value" <| fun _ ->
                let res = Dictionary.tryGetValue 2 testDic1
                Expect.equal res.Value "World" "Did not return correct value"
        ]
        testList "Dictionary.tryGetString" [
            testCase "Is Some" <| fun _ ->
                Expect.isSome (Dictionary.tryGetString 2 testDic1) "Is None"
            testCase "Is None when expected" <| fun _ ->
                Expect.isNone (Dictionary.tryGetString 4 testDic1) "Is Some"
            testCase "Returns correct string" <| fun _ ->
                let res = Dictionary.tryGetString 2 testDic1
                Expect.equal res.Value "World" "Did not return correct string"
            testCase "Returns correct string, trimmed" <| fun _ ->
                let res = Dictionary.tryGetString 3 testDic1
                Expect.equal res.Value "World" "Did not return correctly trimmed string"
        ]
        testList "Dictionary.getValue" [
            testCase "Throws when expected" <| fun _ ->
                Expect.throws (fun _ -> Dictionary.getValue 4 testDic1 |> ignore) "Did not throw though item does not exist"
            testCase "Returns correct value" <| fun _ ->
                let res = Dictionary.getValue 2 testDic1
                Expect.equal res "World" "Did not return correct value"
        ]
        testList "Dictionary.length" [
            testCase "Returns correct count" <| fun _ ->
                let res = Dictionary.length testDic1
                Expect.equal res 3 "Returns incorrect count"
        ]
        testList "Dictionary.copyRecursive" [
            let testDicRecCopy = Dictionary.copyRecursive (Dictionary.copyRecursive id) testDicRec
            testCase "Returns deep copy, check outer count" <| fun _ ->
                Expect.equal testDicRecCopy.Count 1 "Copied Dictionary does not have correct outer count"
            testCase "Returns deep copy, check inner count" <| fun _ ->
                Expect.equal (testDicRecCopy.Item 1).Count 1 "Copied Dictionary does not have correct inner count"
            testCase "Returns deep copy, check inner value" <| fun _ ->
                Expect.equal ((testDicRecCopy.Item 1).Item 1) "inner string" "Copied Dictionary does not have correct inner count"
        ]
    ]