namespace FSharpAux

open System.Collections.Generic

[<AutoOpen>]
module Array = 

    open System

    /// Checks if a give argument is not null and fails if it is.
    let inline checkNonNull argName arg = 
        match box arg with 
        | null  -> nullArg argName 
        | _     -> ()
    
    //let inline contains x (arr : 'T []) =
    //    let mutable found = false
    //    let mutable i = 0
    //    let eq = LanguagePrimitives.FastGenericEqualityComparer

    //    while not found && i < arr.Length do
    //        if eq.Equals(x, arr.[i]) then
    //            found <- true
    //        else
    //            i <- i + 1
    //    found
    
    /// Builds a new array that contains every element of the input array except for that on position index.
    let removeIndex index (arr : 'T []) =
        if index < arr.Length then
            //Array.init (arr.Length - 2) (fun i -> if i <> index then arr.[i])
            Array.ofSeq (seq {for i = 0 to arr.Length - 1  do if i <> index then yield arr.[i]})
        else
            invalidArg "index" "index not present in array"

    /// Applies a function to each element of the array in between inclusively start and fin indices of the array, going backwards (from the end to the start), threading an accumulator argument through the computation. If the input function is f and the elements are i(start)...i(fin) then computes f i(start) (...(f i(fin)-1 i(fin))).
    let scanSubRight f (arr : _ []) start fin initState = 
        let mutable state = initState 
        let res = Array.create (2 + fin - start) initState 
        for i = fin downto start do
            state <- f arr.[i] state;
            res.[i - start] <- state
        done;
        res

    /// Applies a function to each element in between inclusively start and fin indices of the array, threading an accumulator argument through the computation. If the input function is f and the elements are i(start)...i(fin) then computes f (... (f i(start) i(start + 1))...) i(fin). Raises ArgumentException if the array has size zero.
    let scanSubLeft f initState (arr : _ []) start fin = 
        let mutable state = initState 
        let res = Array.create (2 + fin - start) initState 
        for i = start to fin do
            state <- f state arr.[i];
            res.[i - start + 1] <- state
        done;
        res

    /// Applies a function to each element of the array, threading an accumulator argument through the computation. If the input function is f and the elements are i0...iN then computes f (... (f i0 i1)...) iN and returns the intermediary and final results. Raises ArgumentException if the array has size zero.
    let scanReduce f (arr : _ []) = 
        let arrn = arr.Length
        if arrn = 0 then invalidArg "arr" "the input array is empty"
        else scanSubLeft f arr.[0] arr 1 (arrn - 1)

    /// Applies a function to each element of the array, starting from the end, threading an accumulator argument through the computation. If the input function is f and the elements are i0...iN then computes f i0 (...(f iN-1 iN)) and returns the intermediary and final results.
    let scanReduceBack f (arr : _ [])  = 
        let arrn = arr.Length
        if arrn = 0 then invalidArg "arr" "the input array is empty"
        else scanSubRight f arr 0 (arrn - 2) arr.[arrn - 1]

    
    /// Stacks an array of arrays horizontally.
    let stackHorizontal (arrs : array<array<'t>>) =
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init arrs.Length  aMinlength (fun i ii -> arrs.[i].[ii])
        else
            invalidArg "arr" "the input arrays are of different length"

    /// Stacks an array of arrays vertically.
    let stackVertical (arrs : array<array<'t>>) =    
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init aMinlength arrs.Length (fun i ii -> arrs.[ii].[i])
        else
        invalidArg "arr" "the input arrays are of different length"
  
  
    /// Shuffels the input array (method: Fisher-Yates). Define the random number generator outside of a potential loop.
    let shuffleFisherYates (rnd : System.Random) (arr : _[]) =
        let tmpArr = Array.copy arr
        for i = arr.Length downto 1 do
            // Pick random element to swap.
            let j = rnd.Next(i) // 0 <= j <= i-1
            // Swap.
            let tmp = tmpArr.[j]
            tmpArr.[j] <- tmpArr.[i - 1]
            tmpArr.[i - 1] <- tmp
        tmpArr  
        
    /// Shuffels the input array (method: Fisher-Yates) in place. Define the random number generator outside of a potential loop.
    let shuffleInPlace (rnd : System.Random) (arr : _[]) =
        for i = arr.Length downto 1 do
            // Pick random element to swap.
            let j = rnd.Next(i) // 0 <= j <= i-1
            // Swap.
            let tmp = arr.[j]
            arr.[j] <- arr.[i - 1]
            arr.[i - 1] <- tmp
        arr  


    /// Look up an element in an array by index, returning a value if the index is in the domain of the array and default value if not
    let tryFindDefault  (arr : _[]) (zero : 'value) (index : int) =
        match arr.Length > index with
        | true  -> arr.[index]
        | false -> zero


    /// Returns an array of sliding windows of data drawn from the source array.
    /// Each window contains the n elements surrounding the current element.
    let centeredWindow n (source : _ []) =    
        if n < 0 then invalidArg "n" "n must be a positive integer"
        let lastIndex = source.Length - 1
    
        let window i _ =
            let windowStartIndex = System.Math.Max(i - n, 0)
            let windowEndIndex = System.Math.Min(i + n, lastIndex)
            let arrSize = windowEndIndex - windowStartIndex + 1
            let target = Array.zeroCreate arrSize
            Array.blit source windowStartIndex target 0 arrSize
            target

        Array.mapi window source
    
    ///Applies a function to each element and its index of the array, threading an accumulator argument through the computation
    let foldi (f : int -> 'State -> 'T -> 'State) (acc : 'State) (arr : 'T[]) =
        let l = arr.Length
        let rec loop i acc = 
            if i = l then acc
            else loop (i + 1) (f i acc arr.[i])
        loop 0 acc

    // Applies a function to each element of the sub-collection in range (iFrom - iTo), threading an accumulator argument through the computation.
    let foldSub<'T,'State> (f : 'State -> 'T -> 'State) (acc : 'State) (arr : 'T[]) iFrom iTo =
        checkNonNull "array" arr
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        let mutable state = acc 
        //let len = array.Length
        for i = iFrom to iTo do
            state <- f.Invoke(state, arr.[i])
        state

    // Applies a function to each element of two sub-collections in range (iFrom - iTo), threading an accumulator argument through the computation.
    let fold2Sub<'T1,'T2,'State>  f (acc : 'State) (arr1 : 'T1[]) (arr2 : 'T2 []) iFrom iTo =
        checkNonNull "array1" arr1
        checkNonNull "array2" arr2
        let f = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt(f)
        let mutable state = acc 
        let len = arr1.Length
        if len <> arr2.Length then invalidArg "array2" "Arrays must have same size."
        for i = iFrom to iTo do 
            state <- f.Invoke(state, arr1.[i], arr2.[i])
        state

    /// Applies a keyfunction to each element and counts the amount of each distinct resulting key
    let countDistinctBy (keyf : 'T -> 'Key) (arr : 'T array) =
        let dict = System.Collections.Generic.Dictionary<_, int ref> HashIdentity.Structural<'Key>
        // Build the distinct-key dictionary with count
        for v in arr do 
            let key = keyf v
            match dict.TryGetValue(key) with
            | true, count ->
                count := !count + 1 // If it matches a key in the dictionary increment by one
            | _ -> 
                dict.[key] <- ref 1 // If it doesn't match create a new count for this key
        // Write to array
        [|
        for v in dict do yield v.Key |> Operators.id, !v.Value
        |]

    /// Uses a binary search to retrieve an element out of the sorted array arr that fullfills a condition defined by the function compare.
    /// The compare function has to be implemented in the following way:
    ///  let find findThis elementOfArray = 
    ///      if abs (findThis - elementOfArray) < 0.001  then 0 
    ///      elif findThis < elementOfArray then -1 
    ///      else 1
    /// If an element in the array is found that satisfies the condition returning 0, the index is returned.
    /// If the search ends within the array, the negative bitwise complement of the closest larger element is returned.
    /// If the element is bigger than the largest element of the array, the negative bitwise complement of the arr.lenght+1 is returned.
    let binarySearchIndexBy compare (arr : 'a []) = 
        if Array.isEmpty arr then failwith "Array cannot be empty." 
        else
            let rec loop lower upper = 
                if lower > upper then ~~~ lower 
                else
                    let middle = lower + ((upper - lower) / 2)
                    let comparisonResult = compare arr.[middle]   
                    if comparisonResult = 0 then
                        middle
                    elif comparisonResult < 0 then
                        loop lower (middle - 1)
                    else
                        loop (middle + 1) upper
            loop 0 (arr.Length - 1) 


    /// Iterates the data array beginning from the startIdx. 
    /// The step size and direction are implied by magnitude and sign of stepSize. The function returns
    /// the idx of the first value for which predicate returns true or the end/start of the collection
    /// is reached (returning None). 
    let iterUntil (predicate : 'T -> bool) stepSize startIdx (arr : 'T []) =
        let rec loop  (arr : 'T []) currentIdx =
            if currentIdx <= 0 then None
            elif currentIdx >= arr.Length - 1 then None
            else                                              
                match predicate arr.[currentIdx] with 
                | true  -> Some currentIdx   
                | _     -> loop arr (currentIdx + stepSize) 
        loop arr startIdx 

    /// Iterates the data array beginning from the startIdx. 
    /// The step size and direction are implied by magnitude and sign of stepSize. The function returns
    /// the idx of the first value for which predicate returns true or the end/start of the collection
    /// is reached (returning None). The predicate function takes the idx of the current value as an additional
    /// parameter.
    let iterUntili (predicate : int -> 'T -> bool) stepSize startIdx (arr : 'T []) =
        let rec loop (arr: 'T []) currentIdx =
            if currentIdx <= 0 then None
            elif currentIdx >= arr.Length - 1 then None
            else                                              
                match predicate currentIdx arr.[currentIdx] with 
                | true  -> Some currentIdx   
                | _     -> loop arr (currentIdx + stepSize) 
        loop arr startIdx 

    /// Returns a new array containing only the elements of the input array for which the given predicate returns true.
    let filteri (predicate : int -> 'T -> bool) (array : 'T []) =
        let mutable i = -1
        Array.filter (
            fun x ->
                i <- i + 1
                predicate i x
        ) array

    /// Returns the length of an array containing only the elements of the input array for which the given predicate returns true.
    let countByPredicate (predicate : 'T -> bool) (array : 'T []) =
        let mutable counter = 0
        for i = 0 to array.Length - 1 do 
            if predicate array.[i] then counter <- counter + 1
        counter
    
    /// Returns the length of an array containing only the elements of the input array for which the given predicate returns true.
    let countiByPredicate (predicate : int -> 'T -> bool) (array : 'T []) =
        let mutable counter = 0
        for i = 0 to array.Length - 1 do 
            if predicate i array.[i] then counter <- counter + 1
        counter
    
    /// Applies the given function to each element of the array. Returns the array comprised of the results x for each element where the function returns Some x.
    let choosei (chooser: int -> 'T -> 'U Option) (array : 'T []) =
        checkNonNull "array" array    
        let inline subUnchecked startIndex count (array : _ []) =
            let res = Array.zeroCreate count
            if count < 64 then 
                for i = 0 to res.Length - 1 do res.[i] <- array.[startIndex + i]
            else Array.Copy (array, startIndex, res, 0, count)
            res
        let mutable i = 0
        let mutable first = Unchecked.defaultof<'U>
        let mutable found = false
        while i < array.Length && not found do
            let element = array.[i]
            match chooser i element with 
            | None -> i <- i + 1
            | Some b -> 
                first <- b
                found <- true                            
        if i <> array.Length then
            let chunk1 : 'U [] = Array.zeroCreate ((array.Length >>> 2) + 1)
            chunk1.[0] <- first
            let mutable count = 1            
            i <- i + 1                                
            while count < chunk1.Length && i < array.Length do
                let element = array.[i]                                
                match chooser i element with
                | None -> ()
                | Some b -> 
                    chunk1.[count] <- b
                    count <- count + 1                            
                i <- i + 1
            if i < array.Length then                            
                let chunk2 : 'U [] = Array.zeroCreate (array.Length-i)                        
                count <- 0
                while i < array.Length do
                    let element = array.[i]                                
                    match chooser i element with
                    | None -> ()
                    | Some b -> 
                        chunk2.[count] <- b
                        count <- count + 1                            
                    i <- i + 1
                let res : 'U [] = Array.zeroCreate (chunk1.Length + count)
                Array.Copy (chunk1, res, chunk1.Length)
                Array.Copy (chunk2, 0, res, chunk1.Length, count)
                res
            else subUnchecked 0 count chunk1                
        else Array.empty
    
    /// Returns an array with the indices of the elements in the input array that satisfy the given predicate.
    let findIndices (predicate : 'T -> bool) (array : 'T []) =
        let mutable counter = 0
        for i = 0 to array.Length - 1 do if predicate array.[i] then counter <- counter + 1
        let mutable outputArr = Array.zeroCreate counter
        counter <- 0
        for i = 0 to array.Length - 1 do if predicate array.[i] then outputArr.[counter] <- i; counter <- counter + 1
        outputArr
    
    /// Returns a reversed array with the indices of the elements in the input array that satisfy the given predicate.
    let findIndicesBack (predicate : 'T -> bool) (array : 'T []) =
        let mutable counter = 0
        for i = 0 to array.Length - 1 do if predicate array.[i] then counter <- counter + 1
        let mutable outputArr = Array.zeroCreate counter
        counter <- 0
        for i = array.Length - 1 downto 0 do
            if predicate array.[i] then 
                outputArr.[counter] <- i
                counter <- counter + 1
        outputArr
    
    /// Returns an array comprised of every nth element of the input array.
    let takeNth (n : int) (array : 'T []) = filteri (fun i _ -> (i + 1) % n = 0) array
    
    /// Returns an array without every nth element of the input array.
    let skipNth (n : int) (array : 'T []) = filteri (fun i _ -> (i + 1) % n <> 0) array

    /// Iterates over elements of the input array and groups adjacent elements.
    /// A new group is started when the specified predicate holds about the element
    /// of the array (and at the beginning of the iteration).
    ///
    /// For example: 
    ///    Array.groupWhen isOdd [|3;3;2;4;1;2|] = [|[|3|]; [|3; 2; 4|]; [|1; 2|]|]
    let groupWhen f (array : 'T []) =
        let inds = findIndices f array
        if inds.Length = 0 then [|array|]
        else 
            Array.init (inds.Length) (
                fun i ->
                    if i + 1 = inds.Length then
                        array.[Array.last inds ..]
                    else
                        array.[inds.[i] .. inds.[i + 1] - 1]
                )

    /// Computes the intersection of two arrays.
    let intersect (arr1 : 'T []) (arr2 : 'T []) =
        let smallerArr, largerArr =
            if arr1.Length >= arr2.Length then arr2, arr1
            else arr1, arr2
        let hsSa = HashSet<'T>(HashIdentity.Structural<'T>)
        smallerArr |> Array.iter (hsSa.Add >> ignore)
        hsSa.IntersectWith largerArr
        Array.ofSeq hsSa

    /// Computes the outersection (known as "symmetric difference" in mathematics) of two arrays.
    let outersect arr1 (arr2 : 'T []) =
        let hsS1 = HashSet<'T>(HashIdentity.Structural<'T>)
        arr1 |> Array.iter (hsS1.Add >> ignore)
        hsS1.SymmetricExceptWith arr2
        Array.ofSeq hsS1

// ########################################
// Static extensions

    type 'T ``[]`` with
        
        /// Look up an element in an array by index, returning a value if the index is in the domain of the array and default value if not
        member this.TryFindDefault  (arr : _ []) (zero : 'value) (index : int) =
            match arr.Length > index with
            | true  -> arr.[index]
            | false -> zero
    



//    /// Adds two arrays pointwise
//    let inline (.+) a b = Array.map2 (+) a b
//    /// Substracts two arrays pointwise
//    let inline (.-) a b = Array.map2 (-) a b
//    /// Multipies two arrays pointwise
//    let inline (.*) a b = Array.map2 (*) a b
//    /// Divides two arrays pointwise
//    let inline (./) a b = Array.map2 (/) a b

//    type 'T ``[]`` with
//        static member inline (+) (a b = Array.map2 (+) a b
