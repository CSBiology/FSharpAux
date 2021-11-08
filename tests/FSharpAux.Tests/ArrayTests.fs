﻿module ArrayTests

open FSharpAux
open Expecto 

let testArray1                          = [|1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23|]

let testArray1_filteri_Equal            = [|14; 23; 23; 69|]
let testArray1_filteri_NotEqual         = [|1337; 14; 23;|]
let testArray1_choosei_Equal            = [|14.; 23.; 23.; 69.|]
let testArray1_choosei_NotEqual         = [|1337.; 14.; 23.;|]
let testArray1_findIndices_Equal        = [|1; 2; 3; 4; 5; 6; 7; 10|]
let testArray1_findIndices_NotEqual     = [|5; 6; 7; 10|]
let testArray1_findIndicesBack_Equal    = [|10; 7; 6; 5; 4; 3; 2; 1|]
let testArray1_findIndicesBack_NotEqual = [|5; 6; 7; 10|]
let testArray1_takeNth_Equal            = [|23; 1; 1000|]
let testArray1_takeNth_NotEqual         = [|5; 6; 7; 10|]
let testArray1_skipNth_Equal            = [|1337; 14; 23; 69; 2; 3; 9001; 23|]
let testArray1_skipNth_NotEqual         = [|5; 6; 7; 10|]

[<Tests>]
let arrayTests =
    testList "ArrayTests" [
        testList "Array.filteri" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.filteri (fun i t -> i < 5 && t < 100)) testArray1_filteri_Equal "Array.filteri did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.filteri (fun i t -> i < 5 && t < 100)) testArray1_filteri_NotEqual "Array.filteri did not return incorrect array"
            )
        ]
        testList "Array.countByPredicate" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.countByPredicate (fun t -> t < 100)) 8 "Array.countByPredicate did return correct integer"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.countByPredicate (fun t -> t < 100)) 5 "Array.countByPredicate did not return incorrect integer"
            )
        ]
        testList "Array.countiByPredicate" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.countiByPredicate (fun i t -> i < 5 && t < 100)) 4 "Array.countiByPredicate did return correct integer"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.countiByPredicate (fun i t -> i < 5 && t < 100)) 1 "Array.countiByPredicate did not return incorrect integer"
            )
        ]
        testList "Array.choosei" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testArray1_choosei_Equal "Array.choosei did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testArray1_choosei_NotEqual "Array.choosei did not return incorrect array"
            )
        ]
        testList "Array.findIndices" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.findIndices (fun t -> t < 100)) testArray1_findIndices_Equal "Array.findIndices did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.findIndices (fun t -> t < 100)) testArray1_findIndices_NotEqual "Array.findIndices did not return incorrect array"
            )
        ]
        testList "Array.findIndicesBack" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.findIndicesBack (fun t -> t < 100)) testArray1_findIndicesBack_Equal "Array.findIndicesBack did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.findIndicesBack (fun t -> t < 100)) testArray1_findIndicesBack_NotEqual "Array.findIndicesBack did not return incorrect array"
            )
        ]
        testList "Array.takeNth" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.takeNth 3) testArray1_takeNth_Equal "Array.takeNth did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.takeNth 3) testArray1_takeNth_NotEqual "Array.takeNth did not return incorrect array"
            )
        ]
        testList "Array.skipNth" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.skipNth 3) testArray1_skipNth_Equal "Array.skipNth did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.skipNth 3) testArray1_skipNth_NotEqual "Array.skipNth did not return incorrect array"
            )
        ]
    ]