module ArrayTests

open FSharpAux
open Expecto

let private testArray2                          = [|3; 3; 2; 4; 2; 1|]
let private testArray3                          = [|6; 6; 2; 4; 2; 8|]

let arrayTests =
    testList "ArrayTests" [
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