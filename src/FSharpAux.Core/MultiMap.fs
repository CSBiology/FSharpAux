namespace FSharpAux

/// A MultiMap is a special case of a map where value is a list of values
module MultiMap =

    /// Type abreviation for Map<'key,'value list>
    type MultiMap<'key,'value when 'key : comparison>  = Map<'key,'value list>


    
    /// Empty formula
    let emptyMultiMap<'key,'value when 'key : comparison> : MultiMap<'key,'value> = Map.empty 

    /// Returns a new MultiMap with binding added
    let add (key,value) map =
        match Map.tryFind key map with
        | Some values -> Map.add key (value::values) map
        | None -> Map.add key [value] map

    /// Lookup anb element in the map, returning a Some value list if the value is in the domain of the MultiMap else None
    let tryFind key map = 
        Map.tryFind key map
    

    /// Compose a seq of key*value pairs with dublicate keys to a key*value list
    let ofSeq (data:seq<'key*'value>) =    
        data 
        |> Seq.fold (fun map (key,value) -> add (key,value) map) Map.empty  




//    // ###################################
//    // Test multi map
//    let test = [(1,"eins");(2,"zwei");(3,"drei");(2,"zwei");(2,"zwei");] |> ofSeq |> add (4,"vier")

