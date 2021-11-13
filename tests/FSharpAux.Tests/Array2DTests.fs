module Array2DTests

open FSharpAux
open Expecto 

let testArr2d1 =
    array2D [|
        [0 .. 9]
        [99; 66; 44; 11; 22; 33; 55; 0; 11; 0]
    |]

let testArr2d_rotate90DegClockwise_Equal = 
    array2D
        [
            [99; 0]
            [66; 1]
            [44; 2]
            [11; 3]
            [22; 4]
            [33; 5]
            [55; 6]
            [0; 7]
            [11; 8]
            [0; 9]
        ]
let testArr2d_rotate90DegClockwise_NotEqual = 
    array2D
        [
            [33; 5]
            [55; 6]
            [0; 7]
            [11; 8]
            [0; 9]
            [99; 0]
            [66; 1]
            [44; 2]
            [11; 3]
            [22; 4]
        ]
let testArr2d_rotate90DegCounterClockwise_Equal = 
    array2D
        [
            [9; 0]
            [8; 11]
            [7; 0]
            [6; 55]
            [5; 33]
            [4; 22]
            [3; 11]
            [2; 44]
            [1; 66]
            [0; 99]
        ]
let testArr2d_rotate90DegCounterClockwise_NotEqual = 
    array2D
        [
            [99; 0]
            [66; 1]
            [44; 2]
            [11; 3]
            [22; 4]
            [2; 44]
            [1; 66]
            [0; 99]
            [9; 0]
            [8; 11]
        ]
let testArr2d_rotate180Deg_Equal = 
    array2D
        [
            [0; 11; 0; 55; 33; 22; 11; 44; 66; 99]
            [9; 8; 7; 6; 5; 4; 3; 2; 1; 0]
        ]
let testArr2d_rotate180Deg_NotEqual = 
    array2D
        [
            [0; 11; 0; 55; 33; 4; 3; 2; 1; 0]
            [9; 8; 7; 6; 5; 22; 11; 44; 66; 99]
        ]
let testArr2d_flipHorizontally_Equal = 
    array2D
        [
            [99; 66; 44; 11; 22; 33; 55; 0; 11; 0]
            [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]
        ]
let testArr2d_flipHorizontally_NotEqual = 
    array2D
        [
            [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]
            [99; 66; 44; 11; 22; 33; 55; 0; 11; 0]
        ]
let testArr2d_flipVertically_Equal = 
    array2D
        [
            [9; 8; 7; 6; 5; 4; 3; 2; 1; 0]
            [0; 11; 0; 55; 33; 22; 11; 44; 66; 99]
        ]
let testArr2d_flipVertically_NotEqual = 
    array2D
        [
            [0; 11; 0; 55; 33; 22; 11; 44; 66; 99]
            [9; 8; 7; 6; 5; 4; 3; 2; 1; 0]
        ]
let testArr2d_toIndexedArray_Equal = 
    [|(0, 0, 0); (0, 1, 1); (0, 2, 2); (0, 3, 3); (0, 4, 4); (0, 5, 5);
    (0, 6, 6); (0, 7, 7); (0, 8, 8); (0, 9, 9); (1, 0, 99); (1, 1, 66);
    (1, 2, 44); (1, 3, 11); (1, 4, 22); (1, 5, 33); (1, 6, 55); (1, 7, 0);
    (1, 8, 11); (1, 9, 0)|]
let testArr2d_toIndexedArray_NotEqual = 
    [|(1, 2, 44); (1, 3, 11); (1, 4, 22); (1, 5, 33); (1, 6, 55); (1, 7, 0);
    (1, 8, 11); (1, 9, 0); (0, 0, 0); (0, 1, 1); (0, 2, 2); (0, 3, 3); (0, 4, 4); (0, 5, 5);
    (0, 6, 6); (0, 7, 7); (0, 8, 8); (0, 9, 9); (1, 0, 99); (1, 1, 66)|]


[<Tests>]
let array2dTests =
    testList "Array2DTests" [
        testList "Array2D.rotate90DegClockwise" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.rotate90DegClockwise) testArr2d_rotate90DegClockwise_Equal "Array2D.rotate90DegClockwise did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.rotate90DegClockwise) testArr2d_rotate90DegClockwise_NotEqual "Array2D.rotate90DegClockwise did not return incorrect array"
            )
        ]
        testList "Array2D.rotate90DegCounterClockwise" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.rotate90DegCounterClockwise) testArr2d_rotate90DegCounterClockwise_Equal "Array2D.rotate90DegCounterClockwise did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.rotate90DegCounterClockwise) testArr2d_rotate90DegCounterClockwise_NotEqual "Array2D.rotate90DegCounterClockwise did not return incorrect array"
            )
        ]
        testList "Array2D.rotate180Deg" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.rotate180Deg) testArr2d_rotate180Deg_Equal "Array2D.rotate180Deg did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.rotate180Deg) testArr2d_rotate180Deg_NotEqual "Array2D.rotate180Deg did not return incorrect array"
            )
        ]
        testList "Array2D.flipHorizontally" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.flipHorizontally) testArr2d_flipHorizontally_Equal "Array2D.flipHorizontally did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.flipHorizontally) testArr2d_flipHorizontally_NotEqual "Array2D.flipHorizontally did not return incorrect array"
            )
        ]
        testList "Array2D.flipVertically" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.flipVertically) testArr2d_flipVertically_Equal "Array2D.flipVertically did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.flipVertically) testArr2d_flipVertically_NotEqual "Array2D.flipVertically did not return incorrect array"
            )
        ]
        testList "Array2D.toIndexedArray" [
            testCase "returns correct 2D array" (fun _ ->
                Expect.equal (testArr2d1 |> Array2D.toIndexedArray) testArr2d_toIndexedArray_Equal "Array2D.toIndexedArray did return correct array"
            )
            testCase "does not return incorrect 2D array" (fun _ ->
                Expect.notEqual (testArr2d1 |> Array2D.toIndexedArray) testArr2d_toIndexedArray_NotEqual "Array2D.toIndexedArray did not return incorrect array"
            )
        ]
    ]