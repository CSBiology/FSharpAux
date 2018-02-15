namespace FSharpAux

/// Read-only view of the dictionary called dict
module Dict = 
    
    open System
    open System.Collections.Generic
    
    let toSeq d = d |> Seq.map (fun (KeyValue(k,v)) -> (k,v))
    let toArray (d:IDictionary<_,_>) = d |> toSeq |> Seq.toArray
    let toList (d:IDictionary<_,_>) = d |> toSeq |> Seq.toList
    
    let private notMutable = "This value may not be mutated"

    // From fslib-extra-pervasives.fs
    let ofSeq l = 
        // Use a dictionary (this requires hashing and equality on the key type)
        // Wrap keys in an Some(_) option in case they are null 
        // (when System.Collections.Generic.Dictionary fails). Sad but true.
        let t = new Dictionary<Option<_>,_>(HashIdentity.Structural)
        for (k,v) in l do 
            t.[Some(k)] <- v
        let d = (t :> IDictionary<_,_>)
        let c = (t :> ICollection<_>)
        let ieg = (t :> IEnumerable<_>)
        let ie = (t :> System.Collections.IEnumerable)
        // Give a read-only view of the dictionary
        { new IDictionary<'Key, 'a> with 
                member s.Item 
                    with get x = d.[Some(x)]            
                    and  set x v = raise (NotSupportedException(
                                                "This value may not be mutated"))
                member s.Keys = 
                    let keys = d.Keys
                    { new ICollection<'Key> with 
                          member s.Add(x) = raise (NotSupportedException(notMutable));
                          member s.Clear() = raise (NotSupportedException(notMutable));
                          member s.Remove(x) = raise (NotSupportedException(notMutable));
                          member s.Contains(x) = keys.Contains(Some(x))
                          member s.CopyTo(arr,i) = 
                              let mutable n = 0 
                              for k in keys do 
                                  arr.[i+n] <- k.Value
                                  n <- n + 1
                          member s.IsReadOnly = true
                          member s.Count = keys.Count
                      interface IEnumerable<'Key> with
                            member s.GetEnumerator() = (keys |> Seq.map (fun v -> v.Value)).GetEnumerator()
                      interface System.Collections.IEnumerable with
                            member s.GetEnumerator() = ((keys |> Seq.map (fun v -> v.Value)) :> System.Collections.IEnumerable).GetEnumerator() }

                member s.Values = d.Values
                member s.Add(k,v) = raise (NotSupportedException(notMutable))
                member s.ContainsKey(k) = d.ContainsKey(Some(k))
                member s.TryGetValue(k,r) = 
                    let key = Some(k)
                    if d.ContainsKey(key) then (r <- d.[key]; true) else false
                member s.Remove(k : 'Key) = (raise (NotSupportedException(notMutable)) : bool) 
                    
          interface ICollection<KeyValuePair<'Key, 'T>> with 
                member s.Add(x) = raise (NotSupportedException(notMutable));
                member s.Clear() = raise (NotSupportedException(notMutable));
                member s.Remove(x) = raise (NotSupportedException(notMutable));
                member s.Contains(KeyValue(k,v)) = c.Contains(KeyValuePair<_,_>(Some(k),v))
                member s.CopyTo(arr,i) = 
                    let mutable n = 0 
                    for (KeyValue(k,v)) in c do 
                        arr.[i+n] <- KeyValuePair<_,_>(k.Value,v)
                        n <- n + 1
                member s.IsReadOnly = true
                member s.Count = c.Count
          interface IEnumerable<KeyValuePair<'Key, 'T>> with
                member s.GetEnumerator() = 
                    (c |> Seq.map (fun (KeyValue(k,v)) -> KeyValuePair<_,_>(k.Value,v))).GetEnumerator()
          interface System.Collections.IEnumerable with
                member s.GetEnumerator() = 
                    ((c |> Seq.map (fun (KeyValue(k,v)) -> KeyValuePair<_,_>(k.Value,v))) :> System.Collections.IEnumerable).GetEnumerator() }
                //
          

    let ofMap (m:Map<'k,'v>)      = m |> Map.toSeq |> ofSeq
    let ofList (l:('k * 'v) list) = ofSeq l
    let ofArray (a:('k * 'v) [])  = ofSeq a

//    let ofMap (m:Map<'k,'v>) = new Dictionary<'k,'v>(m) :> IDictionary<'k,'v>
//    let ofList (l:('k * 'v) list) = new Dictionary<'k,'v>(l |> Map.ofList) :> IDictionary<'k,'v>
//    let ofSeq (s:('k * 'v) seq) = new Dictionary<'k,'v>(s |> Map.ofSeq) :> IDictionary<'k,'v>
//    let ofArray (a:('k * 'v) []) = new Dictionary<'k,'v>(a |> Map.ofArray) :> IDictionary<'k,'v>
