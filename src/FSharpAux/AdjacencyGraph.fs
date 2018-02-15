namespace FSharpAux


module Graph =

    /// Interface needs to be implemented by every node (vertex; point) in a graph
    type INode<'Tkey when 'Tkey : comparison> =
       // abstract method
       //[<XmlAttribute("id")>]
       abstract member Id   : 'Tkey   

    /// Interface needs to be implemented by every edge (connection; link; line; arc) in a graph
    type IEdge<'Tkey when 'Tkey : comparison> =
        // abstract method
       //[<XmlAttribute("id")>]
       abstract member Id       : 'Tkey
       //[<XmlAttribute("source")>]
       abstract member SourceId : 'Tkey
       //[<XmlAttribute("target")>]
       abstract member TargetId : 'Tkey
   

    /// Alias for a list of edges
    type Adjacency<'Tedge, 'Tkey when 'Tedge :> IEdge<'Tkey> and 'Tkey : comparison> = 'Tedge list

    /// Alias for a single node with its edges
    type AdjacencyNode<'Tnode, 'Tedge, 'Tkey when
                 'Tnode :> INode<'Tkey> and
                 'Tedge :> IEdge<'Tkey> and
                 'Tkey : comparison> =  'Tnode * Adjacency<'Tedge, 'Tkey>

    /// Gets node from adjacencyNode
    let getNodeFromAdjacencyNode (adjNode:AdjacencyNode<_,_,_>) = fst adjNode
    
    /// Gets node from adjacencyNode
    let getEdgesFromAdjacencyNode (adjNode:AdjacencyNode<_,_,_>) = snd adjNode

    /// A Graph is a Vertex list.  The nextNode allows for consistent addressing of nodes
    // A representation of a directed graph with n vertices using an array of n lists of vertices. List i contains vertex j if there is an edge from vertex i to vertex j.
    // A weighted graph may be represented with a list of vertex/weight pairs. An undirected graph may be represented by having vertex j in the list for vertex i and vertex i in the list for vertex j.
    type AdjacencyGraph<'Tnode, 'Tedge, 'Tkey when 'Tnode :> INode<'Tkey> and 'Tedge :> IEdge<'Tkey> and 'Tkey : comparison> =
        AdjacencyNode<'Tnode, 'Tedge, 'Tkey> list

    (* Helper methods for getting the data from a Vertex *)
    let nodeId (v:AdjacencyNode<'Tnode,'Tedge,'Tkey>) : 'Tkey  = (fst v).Id


    /// Tries to get a node and its edges from a graph by id 
    let tryGetAdjacencyNode v (g:AdjacencyGraph<_,_, _>)  =
        g |> List.tryFind (fun V -> nodeId V = v)

    /// Getting a node and its edges from a graph by id 
    let getAdjacencyNode v (g:AdjacencyGraph<_,_, _>)  =
        g |> List.find (fun V -> nodeId V = v)

    /// Tries to get a node from a graph by id 
    let tryGetNodeById v (g:AdjacencyGraph<_,_, _>)  =
        match tryGetAdjacencyNode v g with
        | Some(n,_) -> Some(n) 
        | None      -> None 


    /// Getting all edges (adjacency) from a graph by a vertex id
    let tryGetEdges (v:'Tkey) (g:AdjacencyGraph<_,_, _>) =
        match tryGetAdjacencyNode v g with
        | Some(_,a) -> Some(a) 
        | None      -> None  

    /// Add a new adjacency node
    let addAdjacencyNode (adjNode:AdjacencyNode<_,_,_>) (g:AdjacencyGraph<'Tnode, 'Tedge,'Tkey>) =
        let node = getNodeFromAdjacencyNode adjNode
        match tryGetNodeById node.Id g with
        | None -> adjNode :: g
        | _ -> g

    /// Add a new Edge
    let addEdge (e:'Tedge) (g:AdjacencyGraph<'Tnode, 'Tedge,'Tkey>) : AdjacencyGraph<'Tnode, 'Tedge,'Tkey> = 
        // Todo test if TargetId points to existing node
        g
        |> List.map (fun (v,adj) ->
             if v.Id = e.SourceId then
                (v,e::adj)
             else
                (v,adj))
    
    
    /// Removes an edge from a graph by id
    let removeEdge (eId:'Tkey) (g:AdjacencyGraph<'Tnode, 'Tedge,'Tkey>) : AdjacencyGraph<'Tnode, 'Tedge,'Tkey> = 
        g
        |> List.map (fun (v,adj) ->
            (v,adj
               |> List.filter (fun e -> e.Id <> eId)) )


    /// Removes a node from a graph by id and removes any related edges
    let removeVertex (nodeId:'Tkey) (g:AdjacencyGraph<'Tnode, 'Tedge,'Tkey>) : AdjacencyGraph<'Tnode, 'Tedge,'Tkey> =    
        g |> ([] |> List.fold (fun s' (v, a) ->
                        if v.Id = nodeId then 
                            s'
                        else
                            let f          = fun (x:'Tedge) -> (x.TargetId <> nodeId)
                            let newAdj     = a |> List.filter f
                            let newAdjNode = (v, newAdj)
                            newAdjNode::s'))



