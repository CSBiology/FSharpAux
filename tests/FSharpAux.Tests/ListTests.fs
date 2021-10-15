module ListTests

open FSharpAux
open Expecto

let testList1                          = [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]

let testList1_filteri_Equal            = [14; 23; 23; 69]
let testList1_filteri_NotEqual         = [1337; 14; 23;]
let testList1_choosei_Equal            = [14.; 23.; 23.; 69.]
let testList1_choosei_NotEqual         = [1337.; 14.; 23.;]
let testList1_findIndices_Equal        = [1; 2; 3; 4; 5; 6; 7; 10]
let testList1_findIndices_NotEqual     = [5; 6; 7; 10]
let testList1_findIndicesBack_Equal    = [10; 7; 6; 5; 4; 3; 2; 1]
let testList1_findIndicesBack_NotEqual = [5; 6; 7; 10]
let testList1_takeNth_Equal            = [23; 1; 1000]
let testList1_takeNth_NotEqual         = [5; 6; 7; 10]
let testList1_skipNth_Equal            = [1337; 14; 23; 69; 2; 3; 9001; 23]
let testList1_skipNth_NotEqual         = [5; 6; 7; 10]

[<Tests>]
let listTests =
    testList "ListTests" [
        testList "List.filteri" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.filteri (fun i t -> i < 5 && t < 100)) testList1_filteri_Equal "List.filteri did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.filteri (fun i t -> i < 5 && t < 100)) testList1_filteri_NotEqual "List.filteri did not return incorrect List"
            )
        ]
        testList "List.countByPredicate" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.countByPredicate (fun t -> t < 100)) 8 "List.countByPredicate did return correct integer"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.countByPredicate (fun t -> t < 100)) 5 "List.countByPredicate did not return incorrect integer"
            )
        ]
        testList "List.countiByPredicate" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.countiByPredicate (fun i t -> i < 5 && t < 100)) 4 "List.countiByPredicate did return correct integer"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.countiByPredicate (fun i t -> i < 5 && t < 100)) 1 "List.countiByPredicate did not return incorrect integer"
            )
        ]
        testList "List.choosei" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testList1_choosei_Equal "List.choosei did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testList1_choosei_NotEqual "List.choosei did not return incorrect List"
            )
        ]
        testList "List.findIndices" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.findIndices (fun t -> t < 100)) testList1_findIndices_Equal "List.findIndices did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.findIndices (fun t -> t < 100)) testList1_findIndices_NotEqual "List.findIndices did not return incorrect List"
            )
        ]
        testList "List.findIndicesBack" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.findIndicesBack (fun t -> t < 100)) testList1_findIndicesBack_Equal "List.findIndicesBack did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.findIndicesBack (fun t -> t < 100)) testList1_findIndicesBack_NotEqual "List.findIndicesBack did not return incorrect List"
            )
        ]
        testList "List.takeNth" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.takeNth 3) testList1_takeNth_Equal "List.takeNth did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.takeNth 3) testList1_takeNth_NotEqual "List.takeNth did not return incorrect List"
            )
        ]
        testList "List.skipNth" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.skipNth 3) testList1_skipNth_Equal "List.skipNth did return correct List"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.skipNth 3) testList1_skipNth_NotEqual "List.skipNth did not return incorrect List"
            )
        ]
    ]