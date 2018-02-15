namespace FSharpAux

/// .Net Dictionary
module Dictionary = 
    
    open System
    open System.Collections.Generic

    /// <summary>Views the collection as an enumerable sequence of pairs.
    /// The sequence will be ordered by the keys of the dictionary.</summary>
    /// <param name="table">The input dictionary.</param>
    /// <returns>The sequence of key/value pairs.</returns>   
    let toSeq d = d |> Seq.map (fun (KeyValue(k,v)) -> (k,v))

    /// <summary>Returns a new dictionary made from the given bindings.</summary>
    /// <param name="elements">The input array of key/value pairs.</param>
    /// <returns>The resulting dictionary.</returns>  
    let toArray (d:IDictionary<_,_>) = d |> toSeq |> Seq.toArray
    
    /// <summary>Returns a new dictionary made from the given bindings.</summary>
    /// <param name="elements">The input list of key/value pairs.</param>
    /// <returns>The resulting dictionary.</returns>  
    let toList (d:IDictionary<_,_>) = d |> toSeq |> Seq.toList

    /// <summary>Builds a dictionary that contains the bindings of the given dictionary.</summary>
    /// <param name="elements">The input map.</param>
    /// <returns>The resulting dictionary.</returns> 
    let ofMap (m:Map<'k,'v>) = new Dictionary<'k,'v>(m) :> IDictionary<'k,'v>

    /// <summary>Builds a dictionary that contains the bindings of the given list.</summary>
    /// <param name="elements">The input list of key/value pairs.</param>
    /// <returns>The resulting dictionary.</returns> 
    let ofList (l:('k * 'v) list) = new Dictionary<'k,'v>(l |> Map.ofList) :> IDictionary<'k,'v>
    
    /// <summary>Builds a dictionary that contains the bindings of the given IEnumerable.</summary>
    /// <param name="elements">The input sequence of key/value pairs.</param>
    /// <returns>The resulting dictionary.</returns> 
    let ofSeq (s:('k * 'v) seq) = new Dictionary<'k,'v>(s |> Map.ofSeq) :> IDictionary<'k,'v>
    
    /// <summary>Builds a dictionary that contains the bindings of the given array.</summary>
    /// <param name="elements">The input array of key/value pairs.</param>
    /// <returns>The resulting dictionary.</returns> 
    let ofArray (a:('k * 'v) []) = new Dictionary<'k,'v>(a |> Map.ofArray) :> IDictionary<'k,'v>

        
    /// <summary>Returns the dictionary with the binding added to the given dictionary.
    /// If a binding with the given key already exists in the input dictionary, System.ArgumentException is thrown.</summary>
    /// <param name="key">The input key.</param>
    /// <returns>The dictionary with change in place.</returns>
    let addInPlace key value (table:IDictionary<_,_>) =
        table.Add(key,value)
        table

    /// <summary>Returns the dictionary with the binding added to the given dictionary.
    /// If a binding with the given key already exists in the input dictionary, the existing binding is replaced by the new binding in the result dictionary.</summary>
    /// <param name="key">The input key.</param>
    /// <returns>The dictionary with change in place.</returns>
    let addOrUpdateInPlace key value (table:IDictionary<_,_>) =
        match table.ContainsKey(key) with
        | true  -> table.[key] <- value
        | false -> table.Add(key,value)
        table

    /// <summary>Returns the dictionary with the binding added to the given dictionary.
    /// If a binding with the given key already exists in the input dictionary, the existing binding is replaced by the result of the given function.</summary>
    /// <param name="f">The function to aggregate old value and new value.</param>
    /// <param name="key">The input key.</param>    
    /// <returns>The dictionary with change in place.</returns>
    let addOrUpdateInPlaceBy f key value (table:IDictionary<_,_>) =
        match table.ContainsKey(key) with
        | true  -> 
            let value' = table.[key]
            table.[key] <- f value' value
        | false -> table.Add(key,value)
        table

    /// <summary>Returns true if there are no bindings in the dictionary.</summary>
    let isEmpty (table:IDictionary<_,_>) = table.Count < 1


//    /// <summary>The empty dictionary.</summary>
//    [<GeneralizableValueAttribute>]
//    [<CompiledName("Empty")>]
//    let empty<'Key,'T> : IDictionary<'Key,'T> when 'Key : comparison


    /// <summary>Tests if an element is in the domain of the dictionary.</summary>
    /// <param name="key">The input key.</param>
    /// <returns>True if the dictionary contains the given key.</returns>
    let containsKey key (table:IDictionary<_,_>) = table.ContainsKey(key)

    /// <summary>The number of bindings in the dictionary.</summary>
    let count (table:IDictionary<_,_>) = table.Count

    /// <summary>Lookup an element in the dictionary. Raise <c>KeyNotFoundException</c> if no binding
    /// exists in the dictionary.</summary>
    /// <param name="key">The input key.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown when the key is not found.</exception>
    /// <returns>The value mapped to the key.</returns>
    let item key (table:IDictionary<_,_>) = table.[key]

    /// <summary>Removes an element from the domain of the dictionary. No exception is raised if the element is not present.</summary>
    /// <param name="key">The input key.</param>
    /// <returns>The resulting dictionary.</returns>
    let remove (key:'Key) (table:IDictionary<'Key,_>) = 
        table.Remove(key) |> ignore
        table

    /// <summary>Lookup an element in the dictionary, returning a <c>Some</c> value if the element is in the domain 
    /// of the dictionary and <c>None</c> if not.</summary>
    /// <param name="key">The input key.</param>
    /// <returns>The mapped value, or None if the key is not in the dictionary.</returns>
    let tryFind key (table:IDictionary<_,_>) =
        match table.ContainsKey(key) with
        | true -> Some table.[key]
        | false -> None


//    /// <summary>Folds over the bindings in the dictionary </summary>
//    /// <param name="folder">The function to update the state given the input key/value pairs.</param>
//    /// <param name="state">The initial state.</param>
//    /// <param name="table">The input dictionary.</param>
//    /// <returns>The final state value.</returns>
//    //[<CompiledName("Fold")>]
//    let fold<'Key,'T,'State> (f: 'State -> 'Key -> 'T -> 'State) (x:'State) (source : IDictionary<'Key,'T>)  = 
//        checkNonNull "source" source
//        use e = source.GetEnumerator() 
//        let f = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt(f)
//        let mutable state = x 
//        while e.MoveNext() do
//            state <- f.Invoke(state,e.Current.Key, e.Current.Value)
//        state


    // f is the function how to handel key conflicts
    /// Merge two Dictionaries
    let merge (a : IDictionary<'k,'v>) (b : IDictionary<'k,'v>) (f : 'k -> 'v -> 'v -> 'v) =        
        let dict =  new Dictionary<'k,'v>(a)
        for kv in b do           
            addOrUpdateInPlaceBy (f kv.Key) kv.Key kv.Value dict |> ignore
        dict :> IDictionary<'k,'v>