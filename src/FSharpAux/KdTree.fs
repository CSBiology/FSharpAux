namespace FSharpAux

/// Module for k -dimension trees  
module KdTree =

    /// k -dimension tree    
    type KdTree<'dataRecord,'coordinates> =
        | Node of KdTree<'dataRecord,'coordinates> * KdTree<'dataRecord,'coordinates> * 'coordinates  * int * 'dataRecord 
        | Leaf 




    /// Create KD Tree from list
    // converter -> converts 'dataRecord to dimension array ('coordinates)
    // getCoordinateByPos -> returns coordinate at position
    // k -> number of dimension
    let ofArrayBy (converter:'dataRecord -> 'coordinates) (getCoordinateByPos:'coordinates -> int -> 'coordinate) k arr =
        let rec loop data d k =
            match Array.isEmpty data with
            | true  -> Leaf
            | false ->            
                let axis = d % k
                Array.sortInPlaceBy (fun item -> (getCoordinateByPos (converter item) axis)) data
                let halfIndex = Array.length data / 2
                if halfIndex = 0 then
                    let dataPoint = data.[halfIndex]
                    Node (Leaf, Leaf, (converter dataPoint), d, dataPoint)
                else
                    let dataPoint = data.[halfIndex]
                    let lf, r =     data.[0..halfIndex - 1], data.[halfIndex+1..]
                    Node(loop lf  (d + 1) k, loop r (d + 1) k, (converter dataPoint), d, dataPoint)
        //let arr = data |> Array.ofSeq 
        loop arr 0 k



    /// Flattens KdTree
    let rec toList (tree:KdTree<'a,'b>) =
        match tree with
        | Leaf        -> []
        | Node(left, right, _, _, value) -> toList left  @ [value] @ toList right 
        

    /// Prints kd tree
    let print (kdTree:KdTree<'a,'b>) =    
            let rec printLoop (c) = seq {    
                match c with
                | Node (l,r,s,d,dt)        ->   yield  sprintf "\"Depth\" : %i \"Position\" : %A \"Data\" : %A, \n" d s dt 
                                                yield! printLoop l                                            
                                                yield! printLoop r                                             
                | Leaf                     ->   yield  sprintf ""
            }
            printLoop kdTree


    type Distance<'a> = 'a -> 'a -> float


    /// Searches query coordinates within a given radius
    let rangeSearch (distance:Distance<'coordinates>) (getCoordinateByPos:'coordinates -> int -> 'coordinate) (tree:KdTree<'dataRecord,'coordinates>) (radius) (queryCoordinates:'coordinates)  =
        let rec loop tree (acc:'a list) =
            match tree with 
            | Leaf -> acc
            | Node(cLeft, cRight, cCoor, cK, cValue) ->
                let d = distance cCoor queryCoordinates
                let acc' = if (d <= radius) then (cValue::acc) else acc
                let axis  = getCoordinateByPos queryCoordinates cK
                let cAxis = getCoordinateByPos cCoor cK
                match compare axis cAxis with
                | c when c < 0 -> 
                    if (abs (axis - cAxis) <= radius) then
                        loop cLeft (loop cRight acc')
                    
                    else
                        loop cLeft acc'
                
                | _ -> 
                    if (abs (axis - cAxis) <= radius) then
                        loop cRight (loop cLeft acc')
                    else
                        loop cRight acc'
        loop tree [] 



//let a = [|[|2.;1.;3.;|]; [|2.5;1.5;3.5;|]; [|1.;2.;1.;|]; [|12.;5.;2.4;|]; [|6.;1.;3.;|]; [|0.5;0.5;1.;|]|]
//
//
//let toCoordinates item = item
//let toPosition (item:float array) n = item.[n]
//let kd = ofArrayBy toCoordinates toPosition 3 a

//let euclidean v1 v2 = 
//    Seq.zip v1 v2
//    |> Seq.fold (fun d (e1,e2) -> d + ((e1 - e2) * (e1 - e2))) 0.
//    |> sqrt
//
//let test = rangeSearch euclidean toPosition kd 3. [|0.2; 0.2; 0.9|] 

