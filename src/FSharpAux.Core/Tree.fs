namespace FSharpAux

module Tree =
    

    /// Generic tree node containing member list and child map
    type GenericNode<'key, 'data when 'key : comparison> = {
        Member   : List<'data>
        Children : Map<'key, GenericNode<'key,'data>> 
        }

    /// Generic tree node empty
    let empty = { Member = [] ; Children = Map.empty }


    /// Add item to tree having keyPath as list of keys
    let rec add keyPath item tree =
        match keyPath with
        | [] -> { tree with Member = item::tree.Member}
        | k::key -> 
                let tree' =
                    match Map.tryFind k tree.Children with
                    | Some tree' -> tree'
                    | None -> empty
                {tree with  Children = Map.add k (add key item tree') tree.Children }


    /// Create tree out of keyPath*item list 
    let createTree keyItemList = 
        List.foldBack (fun (elem1,elem2) acc -> add elem1 elem2 acc ) keyItemList empty

    /// Contains 
    let rec contains keyPath item tree = 
        match keyPath with
        | [] -> List.exists(fun i -> i = item) tree.Member
        | k::key ->             
            match Map.tryFind k tree.Children with
            | Some tree -> contains key item tree
            | None -> false


    let rec mapMember (f) (tree:GenericNode<'key, 'data>) : GenericNode<'key, 'ndata> =    
        let c = 
            tree.Children
            |> Map.map (fun _ nodes -> (mapMember f nodes)) 
        {Member = List.map f tree.Member ; Children = c}


    let rec toMemberSeq (tree:GenericNode<'key, 'data>) =
        seq {   match tree.Member with
                | [] -> for child in tree.Children do                                        
                            yield! toMemberSeq child.Value 
                | _  -> yield tree.Member
                        for child in tree.Children do                                        
                            yield! toMemberSeq child.Value 
            
            }


   
    let rec getDeepMemberSeqByKeyPath keyPath (tree:GenericNode<'key, 'data>) : seq<'data>=
        match keyPath with
        | [] -> toMemberSeq tree |> Seq.concat
        | k::key ->             
            match Map.tryFind k tree.Children with
            | Some tree -> getDeepMemberSeqByKeyPath key tree
            | None -> seq []


    let print (tree:GenericNode<'key, 'data>) =
        let rec printLoop depth key (tree:GenericNode<'key, 'data>) =        
            tree.Member |> List.iter (fun data -> for i=0 to depth do printf " -> "
                                                  printfn "%A : %A" (key |> List.rev) data)
            tree.Children
            |> Map.iter (fun key' nodes ->                                   
                                          (printLoop (depth+1) (key'::key) nodes)
                                             )
        printLoop 0 [] tree
