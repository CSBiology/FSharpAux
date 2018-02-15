namespace FSharpAux
  
open System
open System.Collections.Generic


type internal StructuralEqualityComparer<'a when 'a : equality >() = 
    interface IEqualityComparer<'a> with
        member x.Equals(a,b) = a = b
        member x.GetHashCode a = hash a




type BidirectionalDictionary< 'a,'b when 'a: equality and 'b: equality  > internal(forwardDictionary:Dictionary<'a,HashSet<'b>>,reverseDictionary:Dictionary<'b,HashSet<'a>>) = 
    let _forwardDictionary = forwardDictionary
    let _reverseDictionary = reverseDictionary

    let internalAdd (dict:Dictionary<'key,HashSet<'value>>) (k:'key) (v:'value) =
        match dict.TryGetValue(k) with
        | (true, container) ->
            container.Add(v) |> ignore
        | (false,_) -> 
            let tmp = HashSet<'value>(new StructuralEqualityComparer<'value>())
            tmp.Add(v) |> ignore
            dict.Add(k,tmp) 


    let internalAddRange (dict:Dictionary<'key,HashSet<'value>>) (k:'key) (vv:seq<'value>) =
        match dict.TryGetValue(k) with
        | (true, container) ->
            vv |> Seq.iter ( fun v -> container.Add(v) |> ignore )
        | (false,_) -> 
            let tmp = HashSet<'value>(new StructuralEqualityComparer<'value>())
            vv |> Seq.iter ( fun v -> tmp.Add(v) |> ignore )
            dict.Add(k,tmp) 


    new () = BidirectionalDictionary(new Dictionary<'a,HashSet<'b>>(new StructuralEqualityComparer<'a>()),new Dictionary<'b,HashSet<'a>>(new StructuralEqualityComparer<'b>()))

    member this.Add key value =
        internalAdd _forwardDictionary key value
        internalAdd _reverseDictionary value key

    // member this.AddRange a b =
    //     internalAddRange _forwardDictionary a b
    //     internalAddRange _reverseDictionary b a


    member this.ContainsKey key =
        _forwardDictionary.ContainsKey(key)

    member this.ContainsValue value =
        _forwardDictionary.ContainsKey(value)


    member this.TryGetByKey key =
         match _forwardDictionary.TryGetValue(key) with
         | (true, container) -> Some (seq  { for i in container -> i })
         | (false,_)         -> None 


    member this.TryGetByValue value =
         match _reverseDictionary.TryGetValue(value) with
         | (true, container) -> Some (seq  { for i in container -> i })
         | (false,_)         -> None 

    member this.GetArrayOfKeys = 
        Seq.toArray _forwardDictionary.Keys 

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

