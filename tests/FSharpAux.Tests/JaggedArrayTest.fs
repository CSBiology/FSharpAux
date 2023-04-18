module JaggedArrayTest

open FSharpAux
#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif



let private testArr2d1_ofArray2D_Equal =     [|[|0; 1; 2; 3; 4; 5; 6; 7; 8; 9|]; [|99; 66; 44; 11; 22; 33; 55; 0; 11; 0|]|]
let private testArr2d1_ofArray2D_NotEqual =  [|[|5; 6; 7; 8; 9; 0; 1; 2; 3; 4|]; [|55; 0; 11; 99; 66; 44; 11; 22; 33; 0|]|]

let private testJaggArr1 = 
    [|
        [|"I "; "am "; "Jesus!"|]
        [|"No"; " you"; "are"; "not"|]
    |]

let private testJaggArr1_init_Equal = [|
    [|(0, 0); (0, 3); (0, 6); (0, 9); (0, 12); (0, 15); (0, 18)|];
    [|(2, 0); (2, 3); (2, 6); (2, 9); (2, 12); (2, 15); (2, 18)|];
    [|(4, 0); (4, 3); (4, 6); (4, 9); (4, 12); (4, 15); (4, 18)|]
|]
let private testJaggArr1_init_NotEqual = [|
    [|(2, 0); (2, 3); (2, 6); (2, 9); (2, 12); (2, 15); (2, 18)|];
    [|(0, 0); (0, 3); (0, 6); (0, 9); (0, 12); (0, 15); (0, 18)|];
    [|(4, 0); (4, 3); (4, 6); (4, 9); (4, 12); (4, 15); (4, 18)|]
|]
let private testJaggArr1_toIndexedArray_Equal = 
    [|(0, 0, "I "); (0, 1, "am "); (0, 2, "Jesus!"); (1, 0, "No"); (1, 1, " you"); (1, 2, "are"); (1, 3, "not")|]
let private testJaggArr1_toIndexedArray_NotEqual = 
    [|(1, 0, "No"); (1, 1, " you"); (1, 2, "are"); (1, 3, "not"); (0, 0, "I "); (0, 1, "am "); (0, 2, "Jesus!")|]

let private jaggedArrayTests =
    testList "JaggedArrayTests" [
        testList "JaggedArray.toIndexedArray" [
            testCase "returns correct array" (fun _ ->
                Expect.equal (testJaggArr1 |> JaggedArray.toIndexedArray) testJaggArr1_toIndexedArray_Equal "JaggedArray.toIndexedArray did return correct array"
            )
            testCase "does not return incorrect array" (fun _ ->
                Expect.notEqual (testJaggArr1 |> JaggedArray.toIndexedArray) testJaggArr1_toIndexedArray_NotEqual "JaggedArray.toIndexedArray did not return incorrect array"
            )
        ]
        #if FABLE_COMPILER
        #else
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
        #endif
        testList "JaggedArray.init" [
            testCase "returns correct jagged array" (fun _ ->
                Expect.equal (JaggedArray.init 3 7 (fun i j -> i * 2, j * 3)) testJaggArr1_init_Equal "JaggedArray.init did return correct jagged array"
            )
            testCase "does not return incorrect jagged array" (fun _ ->
                Expect.notEqual (JaggedArray.init 3 7 (fun i j -> i * 2, j * 3)) testJaggArr1_init_NotEqual "JaggedArray.init did not return incorrect jagged array"
            )
        ]
    ]


let private testJaggList1_init_Equal = [
    [(0, 0); (0, 3); (0, 6); (0, 9); (0, 12); (0, 15); (0, 18)];
    [(2, 0); (2, 3); (2, 6); (2, 9); (2, 12); (2, 15); (2, 18)];
    [(4, 0); (4, 3); (4, 6); (4, 9); (4, 12); (4, 15); (4, 18)]
]
let private testJaggList1_init_NotEqual = [
    [(2, 0); (2, 3); (2, 6); (2, 9); (2, 12); (2, 15); (2, 18)];
    [(0, 0); (0, 3); (0, 6); (0, 9); (0, 12); (0, 15); (0, 18)];
    [(4, 0); (4, 3); (4, 6); (4, 9); (4, 12); (4, 15); (4, 18)]
]

let private jaggedListTests =
    testList "JaggedListTests" [
        testList "JaggedList.init" [
            testCase "returns correct list" (fun _ ->
                Expect.equal (JaggedList.init 3 7 (fun i j -> i * 2, j * 3)) testJaggList1_init_Equal "JaggedList.init did return correct list"
            )
            testCase "does not return incorrect list" (fun _ ->
                Expect.notEqual (JaggedList.init 3 7 (fun i j -> i * 2, j * 3)) testJaggList1_init_NotEqual "JaggedList.init did not return incorrect list"
            )
        ]
    ]

let main =
    testList "JaggedArray" [
        jaggedArrayTests
        jaggedListTests
    ]