namespace FSharpAux
  
open System
open System.Collections.Generic


type internal StructuralEqualityComparer<'a when 'a : equality >() = 
    interface IEqualityComparer<'a> with
        member x.Equals(a,b) = a = b
        member x.GetHashCode a = hash a



/// The bidirectional dictionary allows effective search of keys and values
type BidirectionalDictionary< 'a,'b when 'a: equality and 'b: equality  > internal(forwardDictionary:Dictionary<'a,HashSet<'b>>,reverseDictionary:Dictionary<'b,HashSet<'a>>) = 
    let _forwardDictionary = forwardDictionary
    let _reverseDictionary = reverseDictionary

    ///Adds a value v to the set of values already associated with key k
    let internalAdd (dict:Dictionary<'key,HashSet<'value>>) (k:'key) (v:'value) =
        match dict.TryGetValue(k) with
        | (true, container) ->
            container.Add(v) |> ignore
        | (false,_) -> 
            let tmp = HashSet<'value>(new StructuralEqualityComparer<'value>())
            tmp.Add(v) |> ignore
            dict.Add(k,tmp) 

    ///Adds a range of values vv to the set of values already associated with key k
    let internalAddRange (dict:Dictionary<'key,HashSet<'value>>) (k:'key) (vv:seq<'value>) =
        match dict.TryGetValue(k) with
        | (true, container) ->
            vv |> Seq.iter ( fun v -> container.Add(v) |> ignore )
        | (false,_) -> 
            let tmp = HashSet<'value>(new StructuralEqualityComparer<'value>())
            vv |> Seq.iter ( fun v -> tmp.Add(v) |> ignore )
            dict.Add(k,tmp) 

    ///Removes key k and all values associated with it
    let internalRemove (dict:Dictionary<'key,HashSet<'value>>) (k:'key) =
        dict.Remove k

    ///Removes all values associated with key k for which the conditional function returns true
    let internalRemoveConditional (f: 'key -> 'value -> bool) (dict:Dictionary<'key,HashSet<'value>>) (k:'key) =
        try 
            match dict.TryGetValue(k) with
            | (true, container) ->
                let vals = container |> Seq.toArray
                Array.iter (fun v -> if f k v then container.Remove(v) |> ignore) vals
                if container.Count = 0 then dict.Remove k |> ignore
            | (false,_) -> ()
        with
        | _ -> ()

    ///Creates a new empty bidirectional dictionary
    new () = BidirectionalDictionary(new Dictionary<'a,HashSet<'b>>(new StructuralEqualityComparer<'a>()),new Dictionary<'b,HashSet<'a>>(new StructuralEqualityComparer<'b>()))

    ///Adds a value v to the set of values already associated with key k to the forward dictionary. Adds a value k to the set of values already associated with key v to the reverse dictionary. 
    member this.Add key value =
        internalAdd _forwardDictionary key value
        internalAdd _reverseDictionary value key

    // member this.AddRange a b =
    //     internalAddRange _forwardDictionary a b
    //     internalAddRange _reverseDictionary b a

    ///Returns true if the forward dictionary contains the key k
    member this.ContainsKey key =
        _forwardDictionary.ContainsKey(key)

    ///Returns true if the reverse dictionary contains the key k
    member this.ContainsValue value =
        _reverseDictionary.ContainsKey(value)

    ///Returns all values of the forward dictionary associated with key k
    member this.TryGetByKey (key:'a) =
         match _forwardDictionary.TryGetValue(key) with
         | (true, container) -> Some (seq  { for i in container -> i })
         | (false,_)         -> None

    ///Returns all values of the reverse dictionary associated with key k
    member this.TryGetByValue value =
         match _reverseDictionary.TryGetValue(value) with
         | (true, container) -> Some (seq  { for i in container -> i })
         | (false,_)         -> None

    ///Removes the key key and all values associated to it from forward dictionary. Removes the value key from all keys from reverse dictionary.
    member this.RemoveKey (key:'a) =
        match this.TryGetByKey key with
        | Some (vals:seq<'b>) ->
            _forwardDictionary.Remove key |> ignore
            let f _ v = v = key
            Seq.iter (internalRemoveConditional f _reverseDictionary) vals
        | None -> failwithf "Cannot remove key %A. Key not found in dictionary" key

    member this.RemoveValue (value:'b) =
        match this.TryGetByValue value with
        | Some (keys:seq<'a>) ->
            _reverseDictionary.Remove value |> ignore
            let f _ k = k = value
            Seq.iter (internalRemoveConditional f _forwardDictionary) keys
        | None -> failwithf "Cannot remove key %A. Key not found in dictionary" value

    ///Retuns all keys of the forward dictionary
    member this.GetArrayOfKeys = 
        Seq.toArray _forwardDictionary.Keys 

    ///Returns all keys of the reverse dictionary
    member this.GetArrayOfValues =    
        Seq.toArray _reverseDictionary.Keys     




//// #######################Testing BidirectionalDictionary

//let testData =
//    [
//        "peptideA","protA"
//        "peptideB","protA"
//        "peptideA","protB"
//        "peptideC","protC"
//    ]

//let biDict = new BidirectionalDictionary<string,string>()
//testData |> Seq.iter (fun (k,v) -> biDict.Add k v ) 

//biDict.TryGetByKey "peptideA"
//biDict.GetArrayOfKeys
//biDict.GetArrayOfValues
//biDict.TryGetByValue "protC"

