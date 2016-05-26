namespace FSharp.Care.Collections

/// .Net Dictionary
module Dictionary = 
    
    open System
    open System.Collections.Generic
    
    let toSeq d = d |> Seq.map (fun (KeyValue(k,v)) -> (k,v))
    let toArray (d:IDictionary<_,_>) = d |> toSeq |> Seq.toArray
    let toList (d:IDictionary<_,_>) = d |> toSeq |> Seq.toList

    let ofMap (m:Map<'k,'v>) = new Dictionary<'k,'v>(m) :> IDictionary<'k,'v>
    let ofList (l:('k * 'v) list) = new Dictionary<'k,'v>(l |> Map.ofList) :> IDictionary<'k,'v>
    let ofSeq (s:('k * 'v) seq) = new Dictionary<'k,'v>(s |> Map.ofSeq) :> IDictionary<'k,'v>
    let ofArray (a:('k * 'v) []) = new Dictionary<'k,'v>(a |> Map.ofArray) :> IDictionary<'k,'v>
