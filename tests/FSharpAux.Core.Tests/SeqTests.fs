module SeqTests

open FSharpAux
#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

let private testSeq1                            = seq {1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23}
let private testSeq2                            = seq {3; 3; 2; 4; 1; 2}
let private testSeq3                            = seq {3; 3; 2; 4; 1; 1}
let private testSeq4                            = seq {3; 3; 2; 4; 2; 2}
let private testSeq5                            = seq {3; 3; 2; 4; 2; 1}
let private testSeq6                            = seq {6; 6; 2; 4; 2; 8}

let private testSeq2_groupWhen_Equal            = seq {seq {3}; seq {3; 2; 4}; seq {1; 2}}
let private testSeq2_groupWhen_NotEqual         = seq {seq {3}; seq {3; 2; 4}; seq {1}; seq {2}}
let private testSeq3_groupWhen_Equal            = seq {seq {3}; seq {3; 2; 4}; seq {1}; seq {1}}
let private testSeq3_groupWhen_NotEqual         = seq {seq {3}; seq {3; 2; 4}; seq {1; 1}}
let private testSeq4_groupWhen_Equal            = seq {seq {3}; seq {3; 2; 4; 2; 2}}
let private testSeq4_groupWhen_NotEqual         = seq {seq {3}; seq {3; 2; 4; 2}; seq {2}}
let private testSeq5_groupWhen_Equal            = seq {seq {3}; seq {3; 2; 4; 2}; seq {1}}
let private testSeq5_groupWhen_NotEqual         = seq {seq {3}; seq {3; 2; 4; 2; 1}}

// helper functions
let list s = Seq.toList s
let list2 s = Seq.map (Seq.toList) s |> Seq.toList

let seqTests =
    testList "SeqTests" [
        let isOdd = fun n -> n % 2 <> 0
        testList "Seq.groupWhen" [
            testCase "returns correct jagged list, case1: [3; 3; 2; 4; 1; 2]" (fun _ ->
                Expect.equal (testSeq2 |> Seq.groupWhen isOdd |> list2) (testSeq2_groupWhen_Equal |> list2) "Seq.groupWhen did return correct jagged list"
            )
            testCase "does not return incorrect jagged list, case1: [3; 3; 2; 4; 1; 2]" (fun _ ->
                Expect.notEqual (testSeq2 |> Seq.groupWhen isOdd |> list2) (testSeq2_groupWhen_NotEqual |> list2) "Seq.groupWhen did not return incorrect jagged list"
            )
            testCase "returns correct jagged list, case2: [3; 3; 2; 4; 1; 1]" (fun _ ->
                Expect.equal (testSeq3 |> Seq.groupWhen isOdd |> list2) (testSeq3_groupWhen_Equal |> list2) "Seq.groupWhen did return correct jagged list"
            )
            testCase "does not return incorrect jagged list, case2: [3; 3; 2; 4; 1; 1]" (fun _ ->
                Expect.notEqual (testSeq3 |> Seq.groupWhen isOdd |> list2) (testSeq3_groupWhen_NotEqual |> list2) "Seq.groupWhen did not return incorrect jagged list"
            )
            testCase "returns correct jagged list, case3: [3; 3; 2; 4; 2; 2]" (fun _ ->
                Expect.equal (testSeq4 |> Seq.groupWhen isOdd |> list2) (testSeq4_groupWhen_Equal |> list2) "Seq.groupWhen did return correct jagged list"
            )
            testCase "does not return incorrect jagged list, case3: [3; 3; 2; 4; 2; 2]" (fun _ ->
                Expect.notEqual (testSeq4 |> Seq.groupWhen isOdd |> list2) (testSeq4_groupWhen_NotEqual |> list2) "Seq.groupWhen did not return incorrect jagged list"
            )
            testCase "returns correct jagged list, case4: [3; 3; 2; 4; 2; 1]" (fun _ ->
                Expect.equal (testSeq5 |> Seq.groupWhen isOdd |> list2) (testSeq5_groupWhen_Equal |> list2) "Seq.groupWhen did return correct jagged list"
            )
            testCase "does not return incorrect jagged list, case4: [3; 3; 2; 4; 2; 1]" (fun _ ->
                Expect.notEqual (testSeq5 |> Seq.groupWhen isOdd |> list2) (testSeq5_groupWhen_NotEqual |> list2) "Seq.groupWhen did not return incorrect jagged list"
            )
            testCase "returns correct jagged list, case4: [6; 6; 2; 4; 2; 8]" (fun _ ->
                Expect.equal (testSeq6 |> Seq.groupWhen isOdd |> list2) ([testSeq6 |> list]) "Seq.groupWhen did return correct jagged list"
            )
            testCase "does not return incorrect jagged list, case4: [6; 6; 2; 4; 2; 8]" (fun _ ->
                Expect.notEqual (testSeq6 |> Seq.groupWhen isOdd |> list2) ([]) "Seq.groupWhen did not return empty (jagged) list"
            )
        ]
        testList "Seq.intersect" [
            testCase "returns correct list, case1: []" (fun _ ->
                Expect.equal (Seq.intersect Seq.empty Seq.empty |> list) [] "Seq.intersect did return correct list"
            )
            testCase "returns correct list, case2: []" (fun _ ->
                Expect.equal (Seq.intersect Seq.empty testSeq1 |> list) [] "Seq.intersect did return correct list"
            )
            testCase "returns correct list, case3: []" (fun _ ->
                Expect.equal (Seq.intersect testSeq2 Seq.empty |> list) [] "Seq.intersect did return correct list"
            )
            testCase "returns correct list, case4: [2; 4]" (fun _ ->
                Expect.equal (Seq.intersect testSeq5 testSeq6 |> list) [2; 4] "Seq.intersect did return correct list"
            )
        ]
    ]