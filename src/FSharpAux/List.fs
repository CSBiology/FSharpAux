namespace FSharpAux

open Microsoft.FSharp.Core.OptimizedClosures

[<AutoOpen>]
module List = 
    
    /// Applies a function to each element of the list, threading an accumulator argument through the computation. If the input function is f and the elements are i0...iN then computes f (... (f i0 i1)...) iN and returns the intermediary and final results. Raises ArgumentException if the list has size zero.
    let scanReduce f l = 
        match l with 
        | [] -> invalidArg "l" "the input list is empty"
        | (h::t) -> List.scan f h t

    /// Applies a function to each element of the array in between (inclusively) start and fin indices of the array, going backwards (from the end to the start), threading an accumulator argument through the computation. If the input function is f and the elements are i(start)...i(fin) then computes f i(start) (...(f i(fin)-1 i(fin))). Returns the result as a list.
    let scanArraySubRight<'T,'State> (f : FSharpFunc<'T,'State,'State>) (arr : _ []) start fin initState = 
        let mutable state = initState  
        let mutable res = [state]  
        for i = fin downto start do
            state <- f.Invoke(arr.[i], state);
            res <- state :: res
        res

    /// Applies a function to each element of the list, starting from the end, threading an accumulator argument through the computation. If the input function is f and the elements are i0...iN then computes f i0 (...(f iN-1 iN)) and returns the intermediary and final results.
    let scanReduceBack f l = 
        match l with 
        | []    -> invalidArg "l" "the input list is empty"
        | _     -> 
            let f = FSharpFunc<_,_,_>.Adapt(f)
            let arr = Array.ofList l 
            let arrn = Array.length arr 
            scanArraySubRight f arr 0 (arrn - 2) arr.[arrn - 1]

    
    /// Cuts a list after N and returns both parts.
    let cutAfterN n input = // Tail-recursive
        let rec gencut cur acc = function
            | hd :: tl when cur < n ->
                gencut (cur + 1) (hd :: acc) tl
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



    /// Groups elements in the input list according to function f.
    let groupEquals f (input : 'a list) =
        let rec groupLoop (first) (heap : 'a list) (stack : 'a list) (groupStack : 'a list) =
            match heap with 
            | head :: rest -> 
                if (f first head) then
                    groupLoop first rest stack (head :: groupStack)
                else
                    groupLoop first rest (head :: stack) groupStack
            | [] -> stack,groupStack

        let rec outerLoop (heap : 'a list) (stack : list<'a list>) =
            match heap with 
            | head :: rest -> 
                let filteredStack,groupStack = groupLoop head rest [] [head]
                outerLoop filteredStack (groupStack :: stack)
            | [] -> stack

        outerLoop input []

    
    // Example:
    // applyEachPairwise (+) ["A";"B";"C";"D";] --> ["AB"; "AC"; "AD"; "BC"; "BD"; "CD"]
    /// Applies function f to each unique compination of items in the input list.
    let applyEachPairwise (f : 'a -> 'a -> 'b) (l : 'a list) =
        let rec innerLoop hh ll acc =
            match ll with
            | h :: ll   -> innerLoop hh ll ((f hh h) :: acc)
            | []        -> acc 
        let rec loop l' acc =
            match l' with
            | h :: tail -> loop tail (innerLoop h tail acc)
            | []        -> acc |> List.rev
        loop l []


    // Example:
    // applyEachPairwiseWith (+) ["A";"B";"C";"D";] --> ["AA"; "AB"; "AC"; "AD"; "BB"; "BC"; "BD"; "CC"; "CD"; "DD"]
    /// Applies function f two each unique compination of items in the input list.
    let applyEachPairwiseWith (f : 'a -> 'a -> 'b) (l : 'a list) =
        let rec innerLoop hh ll acc =
            match ll with
            | h :: ll   -> innerLoop hh ll ((f hh h) :: acc)
            | []        -> acc 
        let rec loop l' acc =
            match l' with
            | h :: tail -> loop tail (innerLoop h l' acc)
            | []        -> acc |> List.rev
        loop l []

    
    /// Removes an element from a list at a given index.
    /// (Not recommended list operation)
    let removeAt index input =
      input 
      // Associate each element with a boolean flag specifying whether 
      // we want to keep the element in the resulting list
      |> List.mapi (fun i el -> (i <> index, el)) 
      // Remove elements for which the flag is 'false' and drop the flags
      |> List.filter fst |> List.map snd


    /// Inserts an element into a list at a given index.
    /// (Not recommended list operation)
    let insertAt index newEl input =
      // For each element, we generate a list of elements that should
      // replace the original one - either singleton list or two elements
      // for the specified index
      input |> List.mapi (fun i el -> if i = index then [newEl; el] else [el])
            |> List.concat

    /// Applies a function to each element and its index of the list, threading an accumulator argument through the computation.
    let foldi (f : int -> 'State -> 'T -> 'State) (acc : 'State) (l : 'T list) =
        let rec loop i acc l = 
            match l with
            | [] -> acc
            | h :: t -> loop (i + 1) (f i acc h) t
        loop 0 acc l

    /// Applies a keyfunction to each element and counts the amount of each distinct resulting key.
    let countDistinctBy (keyf : 'T -> 'Key) (list: 'T list) =
        let dict = System.Collections.Generic.Dictionary<_, int ref> HashIdentity.Structural<'Key>
        // Build the distinct-key list with count
        let rec loop list =
            match list with
            | v :: t -> 
                let key = keyf v
                match dict.TryGetValue(key) with
                | true, count ->
                    count := !count + 1 // If it matches a key in the dictionary increment by one
                | _ -> 
                    dict.[key] <- ref 1 // If it doesn't match create a new count for this key
                loop t
            | _ -> ()
        loop list
        //Write to list
        [for v in dict do yield v.Key |> Operators.id, !v.Value]


    // TODO: add to seperate combinatorics module
    /// Returns the power set of l (not including the empty set).
    let powerSetOf l = 
        let rec loop (l : list<'a>) = 
            match l with
            | []        -> [[]]
            | x :: xs   -> List.collect (fun subSet -> [subSet; x :: subSet]) (loop xs)
        match loop l with
        | h :: t    -> t
        | h         -> h

    /// Treats l as a series of turns of which elements can be drawed from. Returns
    /// a list including all combinations (different order is not considered) 
    /// of elements if in one cycle one item is drawed of each turn.
    let drawExaustively (l : list<list<'a>>) =
        let rec loop acc l =
            match l with
            | []        -> acc
            | h :: t    -> 
                let tmp =
                    h
                    |> List.map (fun x -> acc |> List.map (fun y -> x::y)) 
                    |> List.concat
                loop tmp t
        match l with 
        | []        -> []
        | h :: []   -> h |> List.map (fun x -> [x])
        | h :: t    -> loop (h |> List.map (fun x -> [x])) t

    /// Returns a new list containing only the elements of the list for which the given predicate returns true.
    let filteri (predicate : int -> 'T -> bool) (list : 'T list) =
        let mutable i = -1
        List.filter (
            fun x ->
                i <- i + 1
                predicate i x
        ) list
    
    /// Returns the length of an array containing only the elements of the input array for which the given predicate returns true.
    let countByPredicate (predicate : 'T -> bool) (list : 'T list) =
        let mutable counter = 0
        let rec loop predicate' list' =
            match list' with
            | [] -> ()
            | h :: t -> 
                if predicate' h then counter <- counter + 1; loop predicate' t 
                else loop predicate' t
        loop predicate list; counter
    
    /// Returns the length of an array containing only the elements of the input array for which the given predicate returns true.
    let countiByPredicate (predicate : int -> 'T -> bool) (list : 'T list) =
        let mutable counter = 0
        let mutable i = -1
        let rec loop predicate' list' =
            i <- i + 1
            match list' with
            | [] -> ()
            | h :: t -> 
                if predicate' i h then counter <- counter + 1; loop predicate' t
                else loop predicate' t
        loop predicate list; counter
    
    /// Applies the given function to each element of the list. Returns the list comprised of the results x for each element where the function returns Some x.
    let choosei (predicate : int -> 'T -> 'U option) (list : 'T list) =
        let mutable i = -1
        let rec loop predicate' list' outputList =
            match list' with
            | [] -> outputList
            | h :: t -> 
                i <- i + 1
                match predicate' i h with
                | Some b -> loop predicate' t (b :: outputList)
                | None ->   loop predicate' t outputList
        loop predicate list [] |> List.rev
    
    /// Returns a reversed list with the indices of the elements in the input array that satisfy the given predicate.
    let findIndicesBack (predicate : 'T -> bool) (list : 'T list) =
        let mutable i = -1
        let rec loop predicate' list' outputList =
            match list' with
            | [] -> outputList
            | h :: t -> 
                i <- i + 1
                if predicate' h then loop predicate' t (i :: outputList)
                else loop predicate' t outputList
        loop predicate list []
    
    /// Returns a list with the indices of the elements in the input array that satisfy the given predicate.
    let findIndices (predicate: 'T -> bool) (list: 'T list) = findIndicesBack predicate list |> List.rev
    
    /// Returns a list comprised of every nth element of the input list.
    let takeNth (n : int) (list : 'T list) = filteri (fun i _ -> (i + 1) % n = 0) list
    
    /// Returns a list without every nth element of the input list.
    let skipNth (n : int) (list : 'T list) = filteri (fun i _ -> (i + 1) % n <> 0) list

    /// Iterates over elements of the input list and groups adjacent elements.
    /// A new group is started when the specified predicate holds about the element
    /// of the list (and at the beginning of the iteration).
    ///
    /// For example: 
    ///    List.groupWhen isOdd [3;3;2;4;1;2] = [[3]; [3; 2; 4]; [1; 2]]
    let groupWhen f list =
        list
        |> List.fold (
            fun acc e ->
                match f e, acc with
                | true  , _         -> [e] :: acc       // true case
                | false , h :: t    -> (e :: h) :: t    // false case, non-empty acc list
                | false , _         -> [[e]]            // false case, empty acc list
        ) []
        |> List.map List.rev
        |> List.rev

    /// Computes the intersection of two lists.
    let intersect (list1 : 'T list) (list2 : 'T list) =
        let smallerList, largerList =
            if list1.Length >= list2.Length then list2, list1
            else list1, list2
        let hs = System.Collections.Generic.HashSet<'T>(HashIdentity.Structural<'T>)    // for distinction
        let rec loop predicate l1 l2 fl =
            match l1 with
            | h :: t -> if predicate h l2 && hs.Add h then loop predicate t l2 (h :: fl) else loop predicate t l2 fl
            | [] -> fl
        loop List.contains smallerList largerList []

    /// Computes the outersection (known as "symmetric difference" in mathematics) of two lists.
    let outersect (list1 : 'T list) list2 = [
        for e in list1 do if List.contains e list2 then e
        for e in list2 do if List.contains e list1 then e
    ]

// ########################################
// Static extensions


[<AutoOpen>]
module FsharpListExtensions =
  

    //-------------->
    //List extensions

    type Microsoft.FSharp.Collections.List<'a > with //when 'a : equality
    
        //
        static member iterWhile (f : 'a -> bool) (ls : 'a list) = 
            let rec iterLoop f ls = 
                match ls with
                | head :: tail -> if f head then iterLoop f tail
                | _ -> ()
            iterLoop f ls


