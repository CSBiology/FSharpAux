namespace FSharpAux

[<AutoOpen>]
module Map = 
    
    /// Merge two maps of the same type 
    /// f is the function how to handel key conflicts
    let merge (a : Map<'a, 'b>) (b : Map<'a, 'b>) (f : 'a -> 'b * 'b -> 'b) =
        Map.fold (fun s k v ->
            match Map.tryFind k s with
            | Some v' -> Map.add k (f k (v, v')) s
            | None -> Map.add k v s) a b

    
    /// Compose a seq of key*value pairs with dublicate keys
    /// to a key*value list
    let compose (data:seq<'key*'value>) =    
        data 
        |> Seq.fold (fun map (key,value) -> 
                        match Map.tryFind key map with
                        | Some values -> Map.add key (value::values) map
                        | None -> Map.add key [value] map) Map.empty   



    /// Look up an element in a map, returning a value if the element is in the domain of the map and default value if not
    let tryFindDefault (a : Map<'key, 'value>) (zero : 'value) (key : 'key) =
        match a.TryFind(key) with
        | None -> zero
        | Some(value) -> value 


// ########################################
// Static extensions


[<AutoOpen>]
module FsharpMapExtensions =
  

    //-------------->
    //Map extensions

    
    type Microsoft.FSharp.Collections.Map<'Key,'Value when 'Key : comparison> with
        
            /// Look up an element in a map, returning a value if the element is in the domain of the map and default value if not
            member this.TryFindDefault (zero : 'Value) (key : 'Key) =
                match this.TryFind(key) with
                | None -> zero
                | Some(value) -> value 


