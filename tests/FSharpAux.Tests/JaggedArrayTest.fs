module JaggedArrayTest

open FSharpAux
open Expecto

let testArr2d1 =
    array2D [|
        [0 .. 9]
        [99; 66; 44; 11; 22; 33; 55; 0; 11; 0]
    |]

let testArr2d1_ofArray2D_Equal =     [|[|0; 1; 2; 3; 4; 5; 6; 7; 8; 9|]; [|99; 66; 44; 11; 22; 33; 55; 0; 11; 0|]|]
let testArr2d1_ofArray2D_NotEqual =  [|[|5; 6; 7; 8; 9; 0; 1; 2; 3; 4|]; [|55; 0; 11; 99; 66; 44; 11; 22; 33; 0|]|]

let testJaggArr1 = 
    [|
        [|"I "; "am "; "Jesus!"|]
        [|"No"; " you"; "are"; "not"|]
    |]

let testJaggArr1_toIndexedArray_Equal = 
    [|(0, 0, "I "); (0, 1, "am "); (0, 2, "Jesus!"); (1, 0, "No"); (1, 1, " you"); (1, 2, "are"); (1, 3, "not")|]
let testJaggArr1_toIndexedArray_NotEqual = 
    [|(1, 0, "No"); (1, 1, " you"); (1, 2, "are"); (1, 3, "not"); (0, 0, "I "); (0, 1, "am "); (0, 2, "Jesus!")|]

[<Tests>]
let jaggedArrayTests =
    testList "JaggedArrayTests" [
        testList "JaggedArray.toIndexedArray" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testJaggArr1 |> JaggedArray.toIndexedArray) testJaggArr1_toIndexedArray_Equal "JaggedArray.toIndexedArray did return correct array"
            )
            testCase "returns incorrect array" (fun _ ->
                Expect.notEqual (testJaggArr1 |> JaggedArray.toIndexedArray) testJaggArr1_toIndexedArray_NotEqual "JaggedArray.toIndexedArray did not return incorrect array"
            )
        ]
        testList "JaggedArray.ofArray2D" [
            testCase "returns correct jagged array" (fun _ ->
                Expect.equal (testArr2d1 |> JaggedArray.ofArray2D) testArr2d1_ofArray2D_Equal "JaggedArray.ofArray2D did return correct jagged array"
            )
            testCase "returns incorrect jagged array" (fun _ ->
                Expect.notEqual (testArr2d1 |> JaggedArray.ofArray2D) testArr2d1_ofArray2D_NotEqual "JaggedArray.ofArray2D did not return incorrect jagged array"
            )
        ]
    ]