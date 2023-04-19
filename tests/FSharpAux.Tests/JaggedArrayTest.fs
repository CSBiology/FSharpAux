module JaggedArrayTest

open FSharpAux
open Expecto



let private testArr2d1_ofArray2D_Equal =     [|[|0; 1; 2; 3; 4; 5; 6; 7; 8; 9|]; [|99; 66; 44; 11; 22; 33; 55; 0; 11; 0|]|]
let private testArr2d1_ofArray2D_NotEqual =  [|[|5; 6; 7; 8; 9; 0; 1; 2; 3; 4|]; [|55; 0; 11; 99; 66; 44; 11; 22; 33; 0|]|]

let private jaggedArrayTests =
    testList "JaggedArrayTests" [
        let testArr2d1 =
            array2D [|
                [0 .. 9]
                [99; 66; 44; 11; 22; 33; 55; 0; 11; 0]
            |]
        testList "JaggedArray.ofArray2D" [
            testCase "returns correct jagged array" (fun _ ->
                Expect.equal (testArr2d1 |> JaggedArray.ofArray2D) testArr2d1_ofArray2D_Equal "JaggedArray.ofArray2D did return correct jagged array"
            )
            testCase "does not return incorrect jagged array" (fun _ ->
                Expect.notEqual (testArr2d1 |> JaggedArray.ofArray2D) testArr2d1_ofArray2D_NotEqual "JaggedArray.ofArray2D did not return incorrect jagged array"
            )
        ]
    ]

let main =
    testList "JaggedArray" [
        jaggedArrayTests
    ]