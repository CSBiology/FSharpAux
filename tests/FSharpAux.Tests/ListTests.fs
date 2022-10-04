module ListTests

open FSharpAux
open Expecto

let testList1                           = [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]
let testList2                           = [3; 3; 2; 4; 1; 2]
let testList3                           = [3; 3; 2; 4; 1; 1]
let testList4                           = [3; 3; 2; 4; 2; 2]
let testList5                           = [3; 3; 2; 4; 2; 1]
let testList6                           = [6; 6; 2; 4; 2; 8]
                                        
let testList1_filteri_Equal             = [14; 23; 23; 69]
let testList1_filteri_NotEqual          = [1337; 14; 23;]
let testList1_choosei_Equal             = [14.; 23.; 23.; 69.]
let testList1_choosei_NotEqual          = [1337.; 14.; 23.;]
let testList1_findIndices_Equal         = [1; 2; 3; 4; 5; 6; 7; 10]
let testList1_findIndices_NotEqual      = [5; 6; 7; 10]
let testList1_findIndicesBack_Equal     = [10; 7; 6; 5; 4; 3; 2; 1]
let testList1_findIndicesBack_NotEqual  = [5; 6; 7; 10]
let testList1_takeNth_Equal             = [23; 1; 1000]
let testList1_takeNth_NotEqual          = [5; 6; 7; 10]
let testList1_skipNth_Equal             = [1337; 14; 23; 69; 2; 3; 9001; 23]
let testList1_skipNth_NotEqual          = [5; 6; 7; 10]
let testList1_groupWhen_Equal           = [[1337; 14]; [23]; [23]; [69]; [1; 2]; [3; 1000]; [9001]; [23]]
let testList1_groupWhen_NotEqual        = [[1337; 14]; [23]; [23]; [69]; [1; 2]; [3; 1000]; [9001; 23]]
let testList2_groupWhen_Equal           = [[3]; [3; 2; 4]; [1; 2]]
let testList2_groupWhen_NotEqual        = [[3]; [3; 2; 4]; [1]; [2]]
let testList3_groupWhen_Equal           = [[3]; [3; 2; 4]; [1]; [1]]
let testList3_groupWhen_NotEqual        = [[3]; [3; 2; 4]; [1; 1]]
let testList4_groupWhen_Equal           = [[3]; [3; 2; 4; 2; 2]]
let testList4_groupWhen_NotEqual        = [[3]; [3; 2; 4; 2]; [2]]
let testList5_groupWhen_Equal           = [[3]; [3; 2; 4; 2]; [1]]
let testList5_groupWhen_NotEqual        = [[3]; [3; 2; 4; 2; 1]]

[<Tests>]
let listTests =
    testList "ListTests" [
        testList "List.filteri" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.filteri (fun i t -> i < 5 && t < 100)) testList1_filteri_Equal "List.filteri did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.filteri (fun i t -> i < 5 && t < 100)) testList1_filteri_NotEqual "List.filteri did not return incorrect List"
            )
        ]
        testList "List.countByPredicate" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.countByPredicate (fun t -> t < 100)) 8 "List.countByPredicate did return correct integer"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.countByPredicate (fun t -> t < 100)) 5 "List.countByPredicate did not return incorrect integer"
            )
        ]
        testList "List.countiByPredicate" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.countiByPredicate (fun i t -> i < 5 && t < 100)) 4 "List.countiByPredicate did return correct integer"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.countiByPredicate (fun i t -> i < 5 && t < 100)) 1 "List.countiByPredicate did not return incorrect integer"
            )
        ]
        testList "List.choosei" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testList1_choosei_Equal "List.choosei did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.choosei (fun i t -> if i < 5 && t < 100 then Some (float t) else None)) testList1_choosei_NotEqual "List.choosei did not return incorrect List"
            )
        ]
        testList "List.findIndices" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.findIndices (fun t -> t < 100)) testList1_findIndices_Equal "List.findIndices did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.findIndices (fun t -> t < 100)) testList1_findIndices_NotEqual "List.findIndices did not return incorrect List"
            )
        ]
        testList "List.findIndicesBack" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.findIndicesBack (fun t -> t < 100)) testList1_findIndicesBack_Equal "List.findIndicesBack did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.findIndicesBack (fun t -> t < 100)) testList1_findIndicesBack_NotEqual "List.findIndicesBack did not return incorrect List"
            )
        ]
        testList "List.takeNth" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.takeNth 3) testList1_takeNth_Equal "List.takeNth did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.takeNth 3) testList1_takeNth_NotEqual "List.takeNth did not return incorrect List"
            )
        ]
        testList "List.skipNth" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testList1 |> List.skipNth 3) testList1_skipNth_Equal "List.skipNth did return correct List"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (testList1 |> List.skipNth 3) testList1_skipNth_NotEqual "List.skipNth did not return incorrect List"
            )
        ]
        testList "List.groupWhen" [
            let isOdd n = n % 2 <> 0
            testCase "returns correct jagged list, case1: [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]" (fun _ ->
                Expect.equal (testList1 |> List.groupWhen isOdd) testList1_groupWhen_Equal "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case1: [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]" (fun _ ->
                Expect.notEqual (testList1 |> List.groupWhen isOdd) testList1_groupWhen_NotEqual "List.groupWhen did not return incorrect JaggedList"
            )
            testCase "returns correct jagged list, case2: [3; 3; 2; 4; 1; 2]" (fun _ ->
                Expect.equal (testList2 |> List.groupWhen isOdd) testList2_groupWhen_Equal "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case2: [3; 3; 2; 4; 1; 2]" (fun _ ->
                Expect.notEqual (testList2 |> List.groupWhen isOdd) testList2_groupWhen_NotEqual "List.groupWhen did not return incorrect JaggedList"
            )
            testCase "returns correct jagged list, case3: [3; 3; 2; 4; 1; 1]" (fun _ ->
                Expect.equal (testList3 |> List.groupWhen isOdd) testList3_groupWhen_Equal "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case3: [3; 3; 2; 4; 1; 1]" (fun _ ->
                Expect.notEqual (testList3 |> List.groupWhen isOdd) testList3_groupWhen_NotEqual "List.groupWhen did not return incorrect JaggedList"
            )
            testCase "returns correct jagged list, case4: [3; 3; 2; 4; 2; 2]" (fun _ ->
                Expect.equal (testList4 |> List.groupWhen isOdd) testList4_groupWhen_Equal "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case4: [3; 3; 2; 4; 2; 2]" (fun _ ->
                Expect.notEqual (testList4 |> List.groupWhen isOdd) testList4_groupWhen_NotEqual "List.groupWhen did not return incorrect JaggedList"
            )
            testCase "returns correct jagged list, case5: [3; 3; 2; 4; 2; 1]" (fun _ ->
                Expect.equal (testList5 |> List.groupWhen isOdd) testList5_groupWhen_Equal "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case5: [3; 3; 2; 4; 2; 1]" (fun _ ->
                Expect.notEqual (testList5 |> List.groupWhen isOdd) testList5_groupWhen_NotEqual "List.groupWhen did not return incorrect JaggedList"
            )
            testCase "returns correct jagged list, case6: [6; 6; 2; 4; 2; 8]" (fun _ ->
                Expect.equal (testList6 |> List.groupWhen isOdd) [testList6] "List.groupWhen did return correct JaggedList"
            )
            testCase "does not return incorrect jagged list, case6: [6; 6; 2; 4; 2; 8]" (fun _ ->
                Expect.notEqual (testList6 |> List.groupWhen isOdd) [[]] "List.groupWhen did not return incorrect JaggedList"
            )
        ]
        testList "List.intersect" [
            testCase "does not return incorrect list, case1: []" (fun _ ->
                Expect.equal (List.intersect List.empty List.empty) [] "List.intersect did return correct list"
            )
            testCase "does not return incorrect list, case2: []" (fun _ ->
                Expect.equal (List.intersect List.empty testList6) [] "List.intersect did return correct list"
            )
            testCase "does not return incorrect list, case3: []" (fun _ ->
                Expect.equal (List.intersect testList5 List.empty) [] "List.intersect did return correct list"
            )
            testCase "does not return incorrect list, case4: [2; 4]" (fun _ ->
                Expect.equal (List.intersect testList5 testList6) [2; 4] "List.intersect did return correct list"
            )
        ]
        testList "List.outersect" [
            testCase "does not return incorrect list, case1: []" (fun _ ->
                Expect.equal (List.outersect List.empty List.empty) [] "List.outersect did return correct list"
            )
            testCase "does not return incorrect list, case2: [6; 2; 4; 8]" (fun _ ->
                Expect.equal (List.outersect List.empty testList6) [6; 2; 4; 8] "List.outersect did return correct list"
            )
            testCase "does not return incorrect list, case3: [3; 2; 4; 1]" (fun _ ->
                Expect.equal (List.outersect testList5 List.empty) [3; 2; 4; 1] "List.outersect did return correct list"
            )
            testCase "does not return incorrect list, case4: [3; 1; 6; 8]" (fun _ ->
                Expect.equal (List.outersect testList5 testList6) [3; 1; 6; 8] "List.outersect did return correct list"
            )
        ]
    ]