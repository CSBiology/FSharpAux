module SeqTests

open FSharpAux
open Expecto

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
        testList "Seq.outersect" [
            testCase "returns correct list, case1: []" (fun _ ->
                Expect.equal (Seq.outersect Seq.empty Seq.empty |> list) [] "Seq.outersect did return correct list"
            )
            testCase "returns correct list, case2: [3; 2; 4; 1]" (fun _ ->
                Expect.equal (Seq.outersect Seq.empty testSeq3 |> list) [3; 2; 4; 1] "Seq.outersect did return correct list"
            )
            testCase "returns correct list, case3: [3; 2; 4; 1]" (fun _ ->
                Expect.equal (Seq.outersect testSeq2 Seq.empty |> list) [3; 2; 4; 1] "Seq.outersect did return correct list"
            )
            testCase "returns correct list, case4: [3; 1; 6; 8]" (fun _ ->
                Expect.equal (Seq.outersect testSeq5 testSeq6 |> list) [3; 1; 6; 8] "Seq.outersect did return correct list"
            )
        ]
    ]