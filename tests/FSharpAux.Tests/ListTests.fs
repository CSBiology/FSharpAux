module ListTests

open FSharpAux
open Expecto

let private testList1                           = [1337; 14; 23; 23; 69; 1; 2; 3; 1000; 9001; 23]
let private testList2                           = [3; 3; 2; 4; 1; 2]
let private testList3                           = [3; 3; 2; 4; 1; 1]
let private testList4                           = [3; 3; 2; 4; 2; 2]
let private testList5                           = [3; 3; 2; 4; 2; 1]
let private testList6                           = [6; 6; 2; 4; 2; 8]
                                        
let private testList1_filteri_Equal             = [14; 23; 23; 69]
let private testList1_filteri_NotEqual          = [1337; 14; 23;]
let private testList1_choosei_Equal             = [14.; 23.; 23.; 69.]
let private testList1_choosei_NotEqual          = [1337.; 14.; 23.;]
let private testList1_findIndices_Equal         = [1; 2; 3; 4; 5; 6; 7; 10]
let private testList1_findIndices_NotEqual      = [5; 6; 7; 10]
let private testList1_findIndicesBack_Equal     = [10; 7; 6; 5; 4; 3; 2; 1]
let private testList1_findIndicesBack_NotEqual  = [5; 6; 7; 10]
let private testList1_takeNth_Equal             = [23; 1; 1000]
let private testList1_takeNth_NotEqual          = [5; 6; 7; 10]
let private testList1_skipNth_Equal             = [1337; 14; 23; 69; 2; 3; 9001; 23]
let private testList1_skipNth_NotEqual          = [5; 6; 7; 10]
let private testList1_groupWhen_Equal           = [[1337; 14]; [23]; [23]; [69]; [1; 2]; [3; 1000]; [9001]; [23]]
let private testList1_groupWhen_NotEqual        = [[1337; 14]; [23]; [23]; [69]; [1; 2]; [3; 1000]; [9001; 23]]
let private testList2_groupWhen_Equal           = [[3]; [3; 2; 4]; [1; 2]]
let private testList2_groupWhen_NotEqual        = [[3]; [3; 2; 4]; [1]; [2]]
let private testList3_groupWhen_Equal           = [[3]; [3; 2; 4]; [1]; [1]]
let private testList3_groupWhen_NotEqual        = [[3]; [3; 2; 4]; [1; 1]]
let private testList4_groupWhen_Equal           = [[3]; [3; 2; 4; 2; 2]]
let private testList4_groupWhen_NotEqual        = [[3]; [3; 2; 4; 2]; [2]]
let private testList5_groupWhen_Equal           = [[3]; [3; 2; 4; 2]; [1]]
let private testList5_groupWhen_NotEqual        = [[3]; [3; 2; 4; 2; 1]]

let listTests =
    testList "ListTests" [
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