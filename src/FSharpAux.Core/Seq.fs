namespace FSharpAux

open System.Collections.Generic

[<AutoOpen>]
module Seq =    
    
    ///Adds a value to the back of a sequence.
    let appendSingleton (s : seq<'T>) (value : 'T) =
        Seq.append s (Seq.singleton value)

    ///Adds a value to the front of a sequence.
    let consSingleton (s : seq<'T>) (value : 'T) =
        Seq.append (Seq.singleton value) s

    /// Initialize a sequence of length and repeated value (like R! repeat : but swapped input).
    let initRepeatValue length value =
        Seq.initInfinite (fun _ -> value) |> Seq.take length

    /// Initialize a sequence of length and repeated values (like R! repeat: but swapped input).
    let initRepeatValues length values =
        Seq.initInfinite (fun _ -> values) |> Seq.take length |> Seq.concat

    /// Iterates over the elements of the input sequence and groups adjacent
    /// elements. A new group is started after the specified predicate holds 
    /// about the element of the sequence (and at the beginning of the iteration).
    // with tail recursion and continuous passing 
    // [3;3;2;4;1;2] |> Seq.groupAfter (fun n -> n%2 = 1);;
    let groupAfter f (input : seq<_>) =     
        let rec group (en : IEnumerator<_>) cont acc c  =            
                if not (f en.Current) && en.MoveNext() then
                    group en (fun l -> cont <| c :: l) acc en.Current
                else
                    (fun l -> cont <| c :: l) []
        seq {
            use en = input.GetEnumerator()
            while en.MoveNext() do
                yield group en id [] en.Current
        }

    /// Iterates over elements of the input sequence and groups adjacent elements.
    /// A new group is started when the specified predicate holds about the element
    /// of the sequence (and at the beginning of the iteration).
    ///
    /// For example: 
    ///    Seq.groupWhen isOdd [3;3;2;4;1;2] = seq [[3]; [3; 2; 4]; [1; 2]]
    let groupWhen f (input : seq<'a>) =
        use en = input.GetEnumerator()
        // in older commits, if the first case returned true, the first element (i.e., the last element of the input seq) was concatinated to the next element instead of standing alone.
        // this is not the most elegant solution, but the only working one I came up with.
        let mutable firstCase = false 
    
        // this function matches the cases and iterats from the last to the first element of the input sequence (tested via print debugging), 
        // concatinating elements if false and concatinating the element with an empty list if true.
        let rec loop cont =
            if en.MoveNext() then
                if (f en.Current) then
                    let temp = en.Current
                    loop (fun y ->
                        cont
                            (
                                match y, firstCase with
                                | _ :: _, true ->
                                    firstCase <- false
                                    [] :: [temp] :: y 
                                | h :: t, false ->
                                    [] :: (temp :: h) :: t 
                                | [], _ -> 
                                    firstCase <- true
                                    [[temp]]
                            )
                    )
                else
                    let temp = en.Current
                    loop (fun y ->
                        cont
                            (
                                match y, firstCase with
                                | _ :: _, true ->
                                    firstCase <- false
                                    [temp] :: y 
                                | h :: t, false ->
                                    (temp :: h) :: t
                                | [], _ -> 
                                    [[temp]]
                            )
                    )
            else
                cont []
        // Remove when first element is empty due to "[] :: (temp :: h) :: t"
        let tmp : seq<seq<'a>> = 
            match (loop id) with
            | h :: t -> 
                match h with
                | [] -> t
                | _  -> h :: t
            | [] -> 
                []
            |> Seq.cast
    
        tmp

        /// Computes the intersection of two sequences.
    let intersect (seq1 : seq<'T>) seq2 : seq<'T> = 
        let smallerSeq, largerSeq =
            if Seq.length seq1 >= Seq.length seq2 then seq2, seq1
            else seq1, seq2
        let hsSs = HashSet<'T>(HashIdentity.Structural<'T>)
        smallerSeq |> Seq.iter (hsSs.Add >> ignore)
        hsSs.IntersectWith largerSeq
        hsSs

    /// <summary>
    /// Divides the input sequence into chunks depending on the projection function.
    /// 
    /// A new chunk is created each time the projection function returns true.
    ///
    /// Example: Seq.chunkBy isOdd [3;3;2;4;1;2] = seq [[3]; [3; 2; 4]; [1; 2]]
    /// </summary>
    /// <param name="projection">The function to determine if an element creates a new chunk</param>
    /// <param name="source"></param>
    let chunkBy (projection: 'T -> 'Key) (source: seq<'T>) = 
        seq {
            use e = source.GetEnumerator ()
            if e.MoveNext () then
                let mutable g = projection e.Current
                let mutable members = ResizeArray ()
                members.Add e.Current
                while e.MoveNext () do
                    let key = projection e.Current
                    if g = key then 
                        members.Add e.Current
                    else
                        yield g, members |> Seq.cast<'T>
                        g <- key
                        members <- ResizeArray ()
                        members.Add e.Current
                yield g, members |> Seq.cast<'T>
        }

// // Without continuation passing

//    let groupWhen f (input:seq<_>) = seq {
//        use en = input.GetEnumerator()
//        let running = ref true
//    
//        // Generate a group starting with the current element. Stops generating
//        // when it founds element such that 'f en.Current' is 'true'
//        let rec group() = 
//            [ yield en.Current
//              if en.MoveNext() then
//                if not (f en.Current) then yield! group() 
//              else running := false ]
//    
//        if en.MoveNext() then
//            // While there are still elements, start a new group
//            while running.Value do
//            yield group() |> Seq.ofList }





    
    /// Break sequence into n-element subsequences.
    let groupsOfAtMost (size : int) (s : seq<'v>) : seq<list<'v>> =
        seq {
            let en = s.GetEnumerator()
            let more = ref true
            while !more do
            let group =
                [
                    let i = ref 0
                    while !i < size && en.MoveNext() do
                        yield en.Current
                        i := !i + 1
                ]
            if List.isEmpty group then
                more := false
            else
                yield group
        }


    /// Iterates over elements of the input sequence and increase the counter
    /// if the function returens true.
    let countIf f (input:seq<_>) =         
        let en = input.GetEnumerator()
        let rec loop (en: IEnumerator<_>) (counter:int) =
            if en.MoveNext() then
                if (f en.Current) then
                    loop en (counter + 1)
                else
                    loop en counter                    
            else
                counter
        loop en 0


    let pivotize (aggregation:seq<'T> -> 'A) (defaultValue:'A) (keyList:seq<'key>) (valueList:seq<'key*seq<'T>>) =
        let m = valueList |> Map.ofSeq
        keyList 
        |> Seq.map (
            fun k -> 
                match m.TryGetValue k with
                | true, mk ->
                    aggregation mk
                | false, _ ->
                    defaultValue 
        )


    // /// Combines two sequences according to key generating functions.
    //let joinBy (keyf1: 'a -> 'key) (keyf2: 'b -> 'key) (s1:seq<'a>) (s2:seq<'b>) =
    //    // Wrap a StructBox(_) around all keys in case the key type is itself a type using null as a representation
    //        let dict = new Dictionary<'key,'a option*'b option>()

    //        let insertLeft key lValue =    
    //            let ok,refV = dict.TryGetValue(key)
    //            if ok then 
    //                let _,b = refV
    //                dict.[key] <- (Some lValue,b)            
    //            else             
    //                dict.Add(key,(Some lValue,None))
            
            
    //        let insertRight key rValue =    
    //            let ok,refV = dict.TryGetValue(key)
    //            if ok then 
    //                let a,_ = refV
    //                dict.[key] <- (a,Some rValue) 
    //            else             
    //                dict.Add(key,(None,Some rValue))
            

    //        s1 |> Seq.iter (fun l -> insertLeft  (keyf1 l) l)                                         
    //        s2 |> Seq.iter (fun r -> insertRight (keyf2 r) r)
    
    //        dict |> Seq.map (fun group -> group.Value)
                        


   
    
    /// Returns head of a seq as option or None if seq is empty.
    let tryHead s = Seq.tryPick Some s
    
    /// Returns head of a seq or default value if seq is empty.
    let headOrDefault defaultValue s =
        match (tryHead s) with
        | Some x    -> x
        | None      -> defaultValue

    /// Splits a sequence of pairs into two sequences.
    let unzip (input : seq<_>) =
        let (lstA, lstB) = 
            Seq.foldBack (fun (a,b) (accA, accB) -> 
                a :: accA, b :: accB) input ([], [])
        (Seq.ofList lstA, Seq.ofList lstB)    

    /// Splits a sequence of triples into three sequences.
    let unzip3 (input : seq<_>) =
        let (lstA, lstB, lstC) = 
            Seq.foldBack (fun (a,b,c) (accA, accB, accC) -> 
                a :: accA, b :: accB, c :: accC) input ([],[],[])
        (Seq.ofList lstA, Seq.ofList lstB, Seq.ofList lstC)

    let foldi (f : int -> 'State -> 'T -> 'State) (acc : 'State) (sequence : seq<'T>) =
        let en = sequence.GetEnumerator()
        let rec loop i acc = 
            match en.MoveNext() with
            | false -> acc
            | true -> loop (i + 1) (f i acc en.Current)
        loop 0 acc

    /// Applies a keyfunction to each element and counts the amount of each distinct resulting key.
    let countDistinctBy (keyf : 'T -> 'Key) (sequence : seq<'T>) =
        let dict = Dictionary<_, int> HashIdentity.Structural<'Key>
        let en = sequence.GetEnumerator()
        // Build the distinct-key dictionary with count
        do 
            while en.MoveNext() do
                let key = keyf en.Current
                match dict.TryGetValue(key) with
                | true, count ->
                        dict.[key] <- count + 1 // If it matches a key in the dictionary increment by one
                | _ -> 
                        dict.[key] <- 1 // If it doesn't match create a new count for this key
        // Write to Sequence
        seq {for v in dict do yield v.Key |> Operators.id, v.Value}
    
    
    type JoinOption<'a, 'b, 'c> = seq<'a option * 'b option> -> seq<'c>

    let joinCross (s : seq<'a option * 'b option>) =
        s
        |> Seq.choose (
            fun v -> 
                match v with
                | (Some left, Some right)   -> Some (left,right)
                | _,_                       -> None
        )

    let joinLeft (s : seq<'a option * 'b option>) =
        s
        |> Seq.choose (
            fun v -> 
                match v with
                | (Some left, Some right)   -> Some (left,right)
                | _,_                       -> None
        )

    let joinRight (s : seq<'a option * 'b option>) =
        s
        |> Seq.choose (
            fun v -> 
                match v with
                | (Some left, Some right)   -> Some (left,right)
                | _,_                       -> None
        )

    // Combines two sequences according to key generating functions.
    let joinBy (joinOption:JoinOption<'a,'b, 'c>) (keyf1: 'a -> 'key) (keyf2: 'b -> 'key) (s1:seq<'a>) (s2:seq<'b>) =
        // Wrap a StructBox(_) around all keys in case the key type is itself a type using null as a representation
            let dict = new Dictionary<'key, 'a option * 'b option>()

            let insertLeft key lValue =    
                let ok,refV = dict.TryGetValue(key)
                if ok then 
                    let _,b = refV
                    dict.[key] <- (Some lValue, b)            
                else             
                    dict.Add(key, (Some lValue, None))
            
            
            let insertRight key rValue =    
                let ok,refV = dict.TryGetValue(key)
                if ok then 
                    let a,_ = refV
                    dict.[key] <- (a, Some rValue) 
                else             
                    dict.Add(key, (None, Some rValue))
            

            s1 |> Seq.iter (fun l -> insertLeft  (keyf1 l) l)                                         
            s2 |> Seq.iter (fun r -> insertRight (keyf2 r) r)
    
            dict 
            |> Seq.map (fun group -> group.Value)
            |> joinOption           


    // #region seq double extension
    
    /// Seq module extensions specialized for seq<float>.
    module Double = 

        /// Generates a sequence (like R! seq.int).
        let seqInit (from : float) (tto : float) (length : float) =
            let stepWidth = (tto - from) / (length - 1.)
            Seq.init (int length) ( fun x -> (float x * stepWidth) + from)    

        /// Generates sequence given step width (like R! seq).
        let seqInitStepWidth (from : float) (tto : float) (stepWidth : float) =
            seq { from .. stepWidth .. tto }

        /// Returns the copy of an input sequence where nans were filtered out.
        let filterNaN (sq : seq<float>) =
            sq |> Seq.filter (fun x -> not (System.Double.IsNaN(x)))

        /// Returns the copy of an input sequence where nans were filtered out after applying f to every element.
        let filterNanBy (f : 'a -> float) (sq : seq<'a>) =
            sq |> Seq.filter (fun x -> not (System.Double.IsNaN(f x)))

        /// Returns the copy of an input sequence where infinitys were filtered out.
        let filterInfinity (sq : seq<float>) =
            sq |> Seq.filter (fun x -> not (System.Double.IsInfinity(x)))
    
        /// /// Returns the copy of an input sequence where infinitys were filtered out after applying f to every element.
        let filterInfinityBy (f : 'a -> float) (sq : seq<'a>) =
            sq |> Seq.filter (fun x -> not (System.Double.IsInfinity(f x)))

        /// Returns the copy of an input sequence where nans and infinitys were filtered out.
        let filterNanAndInfinity (sq : seq<float>) =
            sq |> Seq.filter (fun x -> not (System.Double.IsNaN(x) || System.Double.IsInfinity(x)))

        /// Returns the copy of an input sequence where nans and infinitys were filtered out after applying f to every element.
        let filterNanAndInfinityBy (f : 'a -> float) (sq : seq<'a>) =
            sq 
            |> Seq.filter (
                fun v ->
                    let x = f v
                    not (System.Double.IsNaN(x) || System.Double.IsInfinity(x))
            )

        /// Returns true if the input sequence contains nan.
        let existsNaN (sq : seq<float>) =
            sq |> Seq.exists (fun x -> System.Double.IsNaN(x))

        /// Returns true if the input sequence contains nan after applying f to every element.
        let existsNanBy (f : 'a -> float) (sq : seq<'a>) =
            sq |> Seq.exists (fun x -> System.Double.IsNaN(f x))


    //#endregion seq double extension

    
