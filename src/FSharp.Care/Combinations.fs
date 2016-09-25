namespace FSharp.Care

//module Combinations =
// https://docs.python.org/3/library/itertools.html#itertools.permutations

[<AutoOpen>]
module List =
    
    // combinations 2 [1 .. 3]
    // [[2; 1]; [3; 1]; [3; 2]]

    let combinations size set =
        let rec combinations' acc size set = seq {
          match size, set with 
          | n, x::xs -> 
              if n > 0 then yield! combinations' (x::acc) (n - 1) xs
              if n >= 0 then yield! combinations' acc n xs 
          | 0, [] -> yield acc 
          | _, [] -> () }
        combinations' [] size set

    // combinationsWithRep 2 [1 .. 3]
    // [|[1; 1]; [2; 1]; [3; 1]; [2; 2]; [3; 2]; [3; 3]|]
    let rec combinationsWithRep size set = 
        let rec combinationsWithRep' acc size set = seq {
          match size, set with 
          | n, x::xs -> 
              if n > 0 then yield! combinationsWithRep' (x::acc) (n - 1) set
              if n >= 0 then yield! combinationsWithRep' acc n xs
          | 0, [] -> yield acc 
          | _, [] -> () }
        
        combinationsWithRep' [] size set


    /// Carteseian product
    // 
    let product setA setB =
        let rec product' acc setA setB =
            match setA with
              | h::tail ->
                setB
                |> List.collect (fun item -> 
                                        product' (h+item::acc) tail setB)
              | [] -> [acc |> List.rev]
        product' [] setA setB
  

    let rec distribute e l = 
        match l with
        | [] -> [[e]]
        | x :: xs' as xs -> 
            (e :: xs) ::     
                [for xs in distribute e xs' ->  x :: xs ] 
  
    
    // From: http://stackoverflow.com/questions/286427/calculating-permutations-in-f
    // Much faster than anything else I've tested
    let rec private insertions x = function
        | []             -> [[x]]
        | (y :: ys) as l -> (x::l)::(List.map (fun x -> y::x) (insertions x ys))

    /// Return successive r length permutations of elements 
    let rec permutations = function
        | []      -> seq [ [] ]
        | x :: xs -> Seq.concat (Seq.map (insertions x) (permutations xs))    


    /// Permutations with Repetition (order does matter)
    let rec permutationsWithRep m l = seq {
      if m = 1 then 
        // If we want just one replication, generate singleton lists
        for v in l do yield [v]
      else 
        // Otherwise, iterate over all lists with m-1 replicates
        for s in permutationsWithRep (m - 1) l do
          // .. and append elements from 'l' to the front
          for v in l do yield v::s }


[<AutoOpen>]
module Seq =

    let rec private inserts x l =
      seq { match l with
            | [] -> yield [x]
            | y::rest ->
                yield x::l
                for i in inserts x rest do
                  yield y::i
          }

    let rec permutations l =
      seq { match l with
            | [] -> yield []
            | x::rest ->
                for p in permutations rest do
                  yield! inserts x p
          }


