module ArrayTests

open FSharpAux
open Expecto 

let testArray1                          = [|1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23|]
let testArray2                          = [|3; 3; 2; 4; 2; 1|]
let testArray3                          = [|6; 6; 2; 4; 2; 8|]
let testArray4                          = [|6; 6; 2; 4; 2; 9|]
let testArray5                          = [|6; 6; 2; 4; 2; 5|]

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
let testArray1_groupWhen_Equal          = [|[|1337; 14|]; [|23|]; [|23|]; [|69|]; [|1; 2|]; [|3; 1000|]; [|9001|]; [|23|]|]
let testArray1_groupWhen_NotEqual       = [|[|1337; 14|]; [|23|]; [|23|]; [|69|]; [|1; 2|]; [|3; 1000|]; [|9001; 23|]|]
let testArray_map4                      = [|(3, 6, 6, 6); (3, 6, 6, 6); (2, 2, 2, 2); (4, 4, 4, 4); (2, 2, 2, 2); (1, 8, 9, 5)|]

[<Tests>]
let arrayTests =
    testList "ArrayTests" [
        testList "Array.map4" [
            testCase "Throws when any array is null" <| fun _ ->
                Expect.throws (fun _ -> Array.map4 (fun _ _ _ _ -> ()) testArray2 testArray3 null null |> ignore) "Array.map4 did not throw when an input array was null"
            testCase "Throws when arrays have unequal lengths" <| fun _ ->
                Expect.throws (fun _ -> Array.map4 (fun _ _ _ _ -> ()) [|1|] [||] [|3|] [|4|] |> ignore) "Array.map4 did not throw when input arrays have unequal length"
            testCase "Maps correctly" <| fun _ ->
                let res = Array.map4 (fun a b c d -> a, b, c, d) testArray2 testArray3 testArray4 testArray5
                Expect.sequenceEqual res testArray_map4 "Array.map4 did not map correctly"
        ]
        testList "Array.filteri" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.filteri (fun i t -> i < 5 && t < 100)) testArray1_filteri_Equal "Array.filteri did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.filteri (fun i t -> i < 5 && t < 100)) testArray1_filteri_NotEqual "Array.filteri did not return incorrect array"
            )
        ]
        testList "Array.countByPredicate" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.countByPredicate (fun t -> t < 100)) 8 "Array.countByPredicate did return correct integer"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.countByPredicate (fun t -> t < 100)) 5 "Array.countByPredicate did not return incorrect integer"
            )
        ]
        testList "Array.countiByPredicate" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.countiByPredicate (fun i t -> i < 5 && t < 100)) 4 "Array.countiByPredicate did return correct integer"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.countiByPredicate (fun i t -> i < 5 && t < 100)) 1 "Array.countiByPredicate did not return incorrect integer"
            )
        ]
        testList "Array.choosei" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testArray1_choosei_Equal "Array.choosei did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testArray1_choosei_NotEqual "Array.choosei did not return incorrect array"
            )
        ]
        testList "Array.findIndices" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.findIndices (fun t -> t < 100)) testArray1_findIndices_Equal "Array.findIndices did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.findIndices (fun t -> t < 100)) testArray1_findIndices_NotEqual "Array.findIndices did not return incorrect array"
            )
        ]
        testList "Array.findIndicesBack" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.findIndicesBack (fun t -> t < 100)) testArray1_findIndicesBack_Equal "Array.findIndicesBack did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.findIndicesBack (fun t -> t < 100)) testArray1_findIndicesBack_NotEqual "Array.findIndicesBack did not return incorrect array"
            )
        ]
        testList "Array.takeNth" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.takeNth 3) testArray1_takeNth_Equal "Array.takeNth did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.takeNth 3) testArray1_takeNth_NotEqual "Array.takeNth did not return incorrect array"
            )
        ]
        testList "Array.skipNth" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testArray1 |> Array.skipNth 3) testArray1_skipNth_Equal "Array.skipNth did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.skipNth 3) testArray1_skipNth_NotEqual "Array.skipNth did not return incorrect array"
            )
        ]
        testList "Array.groupWhen" [
            let isOdd n = n % 2 <> 0
            testCase "returns correct jagged array" (fun _ ->
                Expect.equal (testArray1 |> Array.groupWhen isOdd) testArray1_groupWhen_Equal "Array.groupWhen did return correct jagged array"
            )
            testCase "does not return incorrect jagged array" (fun _ ->
                Expect.notEqual (testArray1 |> Array.groupWhen isOdd) testArray1_groupWhen_NotEqual "Array.groupWhen did not return incorrect jagged array"
                Expect.notEqual (testArray1 |> Array.groupWhen isOdd) [||] "Array.groupWhen did not return empty jagged array"
            )
        ]
        testList "Array.intersect" [
            testCase "returns correct array, case1: [||]" (fun _ ->
                Expect.equal (Array.intersect Array.empty Array.empty) [||] "Array.intersect did not return empty array"
            )
            testCase "returns correct array, case2: [||]" (fun _ ->
                Expect.equal (Array.intersect Array.empty testArray3) [||] "Array.intersect did not return empty array"
            )
            testCase "returns correct array, case3: [||]" (fun _ ->
                Expect.equal (Array.intersect testArray2 Array.empty) [||] "Array.intersect did not return empty array"
            )
            testCase "returns correct array, case4: [|2; 4|]" (fun _ ->
                Expect.equal (Array.intersect testArray2 testArray3) [|2; 4|] "Array.intersect did not return correct array"
            )
        ]
        testList "Array.outersect" [
            testCase "returns correct array, case1: [||]" (fun _ ->
                Expect.equal (Array.outersect Array.empty Array.empty) [||] "Array.outersect did not return empty array"
            )
            testCase "returns correct array, case2: [|6; 2; 4; 8|]" (fun _ ->
                Expect.equal (Array.outersect Array.empty testArray3) [|6; 2; 4; 8|] "Array.outersect did return false array"
            )
            testCase "returns correct array, case3: [|3; 2; 4; 1|]" (fun _ ->
                Expect.equal (Array.outersect testArray2 Array.empty) [|3; 2; 4; 1|] "Array.outersect did return false array"
            )
            testCase "returns correct array, case4: [|6; 8; 3; 1|]" (fun _ ->
                Expect.equal (Array.outersect testArray2 testArray3) [|3; 1; 6; 8|] "Array.outersect did return false array"
            )
        ]
    ]