module StringTests

open FSharpAux
open Expecto 

[<Tests>]
let stringTests =
    testList "StringTests" [
        testList "String.endsWith" [
            testCase "Returns correct bool" <| fun _ ->
                Expect.isTrue (String.endsWith "World" "Hello World") "Returned incorrect bool"
        ]
        testList "String.trim" [
            testCase "trims input correctly" <| fun _ ->
                Expect.equal (String.trim "      \tHi there!\n     ") "Hi there!" "String.trim did not trim correctly"
        ]
        testList "String.contains" [
            testCase "contains present substring" (fun _ ->
                Expect.isTrue ("Hi there!" |> String.contains "Hi") "String.contains did not return true for proper substring"
            )
            testCase "does not contain missing substring" (fun _ ->
                Expect.isFalse ("Hi there!" |> String.contains "soos") "String.contains did not return false for improper substring"
            )
        ]
        testList "String.startsWith" [
            testCase "does start with" (fun _ ->
                Expect.isTrue ("Hi there!" |> String.startsWith "Hi ") "String.startsWith did not return true for proper substring"
            )
            testCase "does not fail to start with" (fun _ ->
                Expect.isFalse ("Hi there!" |> String.startsWith "soos") "String.startsWith did not return false for improper substring"
            )
        ]
        testList "String.replace" [
            testCase "replaces" (fun _ ->
                Expect.equal ("Hi there!" |> String.replace "Hi " "Yo ") "Yo there!" "String.replaces did return correct replacement"
            )
            testCase "does not fail to replace" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.replace "soos" "Yo") "Yo there!" "String.replaces did not return incorrect replacement"
            )
        ]
        testList "String.first" [
            testCase "gives first" (fun _ ->
                Expect.equal ("Hi there!" |> String.first) 'H' "String.first did return correct Char"
            )
            testCase "does not give anything else" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.first) '!' "String.first did not return incorrect Char"
            )
            testCase "errors if empty" (fun _ ->
                Expect.throws (fun () -> "" |> String.first |> ignore) "String.first did throw an exception"
            )
        ]
        testList "String.last" [
            testCase "gives last" (fun _ ->
                Expect.equal ("Hi there!" |> String.last) '!' "String.first did return correct Char"
            )
            testCase "does not give anything else" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.last) 'u' "String.first did not return incorrect Char"
            )
            testCase "errors if empty" (fun _ ->
                Expect.throws (fun () -> "" |> String.last |> ignore) "String.last did throw an exception"
            )
        ]
        testList "String.splitS" [
            testCase "splits correctly" (fun _ ->
                Expect.equal ("Hi there!" |> String.splitS " t") [|"Hi"; "here!"|] "String.splitS did return correct string array"
            )
            testCase "does not splits incorrectly" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.splitS " t") [|"Hi "; "there!"|] "String.first did not return incorrect string array"
            )
        ]
        testList "String.findIndexBack" [
            testCase "finds correct index" (fun _ ->
                Expect.equal ("Hi there!" |> String.findIndexBack ' ') 2 "String.findIndexBack did return correct index"
            )
            testCase "does not find incorrect index" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.findIndexBack ' ') 3 "String.findIndexBack did not return incorrect index"
            )
        ]
        testList "String.findIndex" [
            testCase "finds correct index" (fun _ ->
                Expect.equal ("Hi there!" |> String.findIndex ' ') 2 "String.findIndex did return correct index"
            )
            testCase "does not find incorrect index" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.findIndex ' ') 3 "String.findIndex did not return incorrect index"
            )
        ]
        testList "String.findIndices" [
            testCase "finds correct indices" (fun _ ->
                Expect.equal ("Hi there!" |> String.findIndices 'e') [|5; 7|] "String.findIndices did return correct indices"
            )
            testCase "does not find incorrect indices" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.findIndices 'e') [|0; 1|] "String.findIndices did not return incorrect indices"
            )
        ]
        testList "String.findIndicesBack" [
            testCase "finds correct indices" (fun _ ->
                Expect.equal ("Hi there!" |> String.findIndicesBack 'e') [|7; 5|] "String.findIndicesBack did return correct indices"
            )
            testCase "does not find incorrect indices" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.findIndicesBack 'e') [|0; 1|] "String.findIndicesBack did not return incorrect indices"
            )
        ]
        testList "String.takeWhile" [
            testCase "finds correct substring" (fun _ ->
                Expect.equal ("Hi there!" |> String.takeWhile ((=) 'H')) "H" "String.takeWhile did return correct substring"
            )
            testCase "does not find incorrect substring" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.takeWhile ((=) 'H')) "fdsfds" "String.takeWhile did not return incorrect substring"
            )
        ]
        testList "String.skipWhile" [
            testCase "finds correct substring" (fun _ ->
                Expect.equal ("Hi there!" |> String.skipWhile ((=) 'H')) "i there!" "String.skipWhile did return correct substring"
            )
            testCase "does not find incorrect substring" (fun _ ->
                Expect.notEqual ("Hi there!" |> String.skipWhile ((=) 'H')) "fdsfds" "String.skipWhile did not return incorrect substring"
            )
        ]
    ]