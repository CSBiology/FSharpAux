namespace FSharp.Care.Collections

open Microsoft.FSharp.Core.OptimizedClosures

[<AutoOpen>]
module List = 
    
    let scanReduce f l = 
        match l with 
        | [] -> invalidArg "l" "the input list is empty"
        | (h::t) -> List.scan f h t

    let scanArraySubRight<'T,'State> (f:FSharpFunc<'T,'State,'State>) (arr:_[]) start fin initState = 
        let mutable state = initState  
        let mutable res = [state]  
        for i = fin downto start do
            state <- f.Invoke(arr.[i], state);
            res <- state :: res
        res

    let scanReduceBack f l = 
        match l with 
        | [] -> invalidArg "l" "the input list is empty"
        | _ -> 
            let f = FSharpFunc<_,_,_>.Adapt(f)
            let arr = Array.ofList l 
            let arrn = Array.length arr 
            scanArraySubRight f arr 0 (arrn - 2) arr.[arrn - 1]

    
    /// Cuts a list after N and returns both parts
    let cutAfterN n input = // Tail-recursive
        let rec gencut cur acc = function
            | hd::tl when cur < n ->
                gencut (cur+1) (hd::acc) tl
            | rest -> (List.rev acc), rest //need to reverse accumulator!
        gencut 0 [] input

//    // Non-tail-recursive
//    let cutAfterN n input =
//        let rec gencut cur = function
//            | hd::tl when cur < n ->
//                let x, y = gencut (cur+1) tl //stackoverflow with big lists!
//                hd::x, y
//            | rest -> [], rest
//        gencut 0 input

    /// Cuts a list into two parts
    let cut input = 
        let half = (input |> List.length) / 2
        input |> cutAfterN half



    /// Groups elements in list that are b function f
    let groupEquals f (input:'a list) =
        let rec groupLoop (first) (heap:'a list) (stack:'a list) (groupStack:'a list) =
            match heap with 
            | head::rest -> if (f first head) then
                                groupLoop first rest stack (head::groupStack)
                            else
                                groupLoop first rest (head::stack) groupStack
                        
            | []         -> stack,groupStack
    



        let rec outerLoop (heap:'a list) (stack:list<'a list>) =
            match heap with 
            | head::rest -> let filteredStack,groupStack = groupLoop head rest [] [head]
                            outerLoop filteredStack (groupStack::stack)
            | []         -> stack

    

        outerLoop input []

    
    // Example:
    // applyEachPairwise (+) ["A";"B";"C";"D";] --> ["AB"; "AC"; "AD"; "BC"; "BD"; "CD"]
    /// Applies function f two each unique compination of items in list 
    let applyEachPairwise (f: 'a -> 'a -> 'b ) (l : 'a list) =
        let rec innerLoop hh ll acc =
            match ll with
            | h::ll -> innerLoop hh ll ((f hh h)::acc)
            | []    -> acc 
        let rec loop l' acc =
            match l' with
            | h::tail -> loop tail (innerLoop h tail acc)
            | []      -> acc |> List.rev
        loop l []


    // Example:
    // applyEachPairwiseWith (+) ["A";"B";"C";"D";] --> ["AA"; "AB"; "AC"; "AD"; "BB"; "BC"; "BD"; "CC"; "CD"; "DD"]
    /// Applies function f two each unique compination of items in list 
    let applyEachPairwiseWith (f: 'a -> 'a -> 'b ) (l : 'a list) =
        let rec innerLoop hh ll acc =
            match ll with
            | h::ll -> innerLoop hh ll ((f hh h)::acc)
            | []    -> acc 
        let rec loop l' acc =
            match l' with
            | h::tail -> loop tail (innerLoop h l' acc)
            | []      -> acc |> List.rev
        loop l []

    
    /// Removes an element from a list at a given index
    /// (Not recommended list opperation)
    let removeAt index input =
      input 
      // Associate each element with a boolean flag specifying whether 
      // we want to keep the element in the resulting list
      |> List.mapi (fun i el -> (i <> index, el)) 
      // Remove elements for which the flag is 'false' and drop the flags
      |> List.filter fst |> List.map snd


    /// Inserts an element into a list at a given index
    /// (Not recommended list opperation)
    let insertAt index newEl input =
      // For each element, we generate a list of elements that should
      // replace the original one - either singleton list or two elements
      // for the specified index
      input |> List.mapi (fun i el -> if i = index then [newEl; el] else [el])
            |> List.concat

    ///Applies a function to each element and its index of the list, threading an accumulator argument through the computation
    let foldi (f : int -> 'State -> 'T -> 'State) (acc: 'State) (l:'T list) =
        let rec loop i acc l = 
            match l with
            | [] -> acc
            | h :: t -> loop (i+1) (f i acc h) t
        loop 0 acc l

    ///Applies a keyfunction to each element and counts the amount of each distinct resulting key
    let countDistinctBy (keyf : 'T -> 'Key) (list: 'T list) =
        let dict = System.Collections.Generic.Dictionary<_, int ref> HashIdentity.Structural<'Key>
        // Build the distinct-key list with count
        let rec loop list =
            match list with
            | v :: t -> 
                let key = keyf v
                match dict.TryGetValue(key) with
                | true, count ->
                        count := !count + 1 //If it matches a key in the dictionary increment by one
                | _ -> 
                    dict.[key] <- ref 1 //If it doesnt match create a new count for this key
                loop t
            | _ -> ()
        loop list
        //Write to list
        [
        for v in dict do yield v.Key |> Operators.id,!v.Value
        ]
// ########################################
// Static extensions


[<AutoOpen>]
module FsharpListExtensions =
  

    //-------------->
    //List extensions

    type Microsoft.FSharp.Collections.List<'a > with //when 'a : equality
    
        //
        static member iterWhile (f:'a -> bool) (ls:'a list) = 
            let rec iterLoop f ls = 
                match ls with
                | head :: tail -> if f head then iterLoop f tail
                | _ -> ()
            iterLoop f ls


