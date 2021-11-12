module SeqTests

open FSharpAux
open Expecto

let testSeq1                            = seq {1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23}
let testSeq2                            = seq {3; 3; 2; 4; 1; 2}
let testSeq3                            = seq {3; 3; 2; 4; 1; 1}
let testSeq4                            = seq {3; 3; 2; 4; 2; 2}
let testSeq5                            = seq {3; 3; 2; 4; 2; 1}

let testSeq2_groupWhen_Equal            = seq {seq {1337; 14}; seq {23}; seq {23}; seq {69}; seq {1; 2}; seq {3; 1000}; seq {9001; 23}}
let testSeq2_groupWhen_NotEqual         = seq {seq {1337; 14}; seq {23}; seq {23}; seq {69}; seq {1; 2}; seq {3; 1000}; seq {9001; 23}}

[<Tests>]
let seqTests =
    testList "SeqTests" [
        let isOdd = fun n -> n % 2 <> 0
        testList "Seq.groupWhen" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (testSeq1 |> Seq.groupWhen isOdd) testSeq1_groupWhen_Equal "Seq.groupWhen did return correct jagged sequence"
            )
            testCase "returns incorrect list" (fun _ ->
                Expect.notEqual (testSeq1 |> Seq.groupWhen isOdd) testList1_filteri_NotEqual "Seq.groupWhen did not return incorrect jagged sequence"
            )
        ]
    ]