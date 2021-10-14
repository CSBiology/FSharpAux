module StringTests

open FSharpAux
open Expecto 

[<Tests>]
let stringTests =
    testList "StringTests" [
        testList "String.contains" [
            testCase "contains present substring" (fun _ ->
                Expect.isTrue ("Hi there!" |> String.contains "Hi") "String.contains did not return true for proper substring"
            )
            testCase "does not contain missing substring" (fun _ ->
                Expect.isFalse ("Hi there!" |> String.contains "soos") "String.contains did not return false for improper substring"
            )
        ]
    ]