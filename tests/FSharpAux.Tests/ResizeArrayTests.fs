module ResizeArrayTests

open FSharpAux
open Expecto

let private emptyArray : ResizeArray<int> = ResizeArray()
let private intArray = [6; 5; 2; 3; 2; 8] |> ResizeArray.ofList

let resizeArrayTests =
    testList "ResizeArrayTests" [
        testList "ResizeArray.sum" [
            testCase "Empty array sum is 0" (fun _ ->
                Expect.equal (ResizeArray.sum emptyArray) 0 "ResizeArray.sum of empty array is not 0."
            )
            testCase "returns correct sum" (fun _ ->
                Expect.equal (ResizeArray.sum intArray) 26 "ResizeArray.sum calculates incorrectly"
            )
        ]
        testList "ResizeArray.sumBy" [
            testCase "Empty array sumBy is 0" (fun _ ->
                Expect.equal (emptyArray |> ResizeArray.sumBy (fun x -> x * 2)) 0 "ResizeArray.sumBy of empty array is not 0."
            )
            testCase "returns correct sum" (fun _ ->
                Expect.equal (intArray |> ResizeArray.sumBy (fun x -> x * 2)) 52 "ResizeArray.sumBy calculates incorrectly"
            )
        ]
        testList "ResizeArray.countIf" [
            testCase "Empty array count is 0" (fun _ ->
                Expect.equal (emptyArray |> ResizeArray.countIf (fun x -> x % 2 = 0)) 0 "ResizeArray.countIf of empty array is not 0."
            )
            testCase "returns correct count" (fun _ ->
                Expect.equal (intArray |> ResizeArray.countIf (fun x -> x % 2 = 0)) 4 "ResizeArray.countIf calculates incorrectly"
            )
        ]
    ]