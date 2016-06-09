
let arr2D = Array2D.create 3 5 0

/// Returns the number of elements in a row
let rowCount (arr:_[,]) =
    arr.GetLength(0)

/// Returns the number of elements in a column
let colCount (arr:_[,]) =
    arr.GetLength(1)


let foldRows (f:'State -> 'T -> 'State) (state:'State []) (arr:'State [,]) =  
    let rowLen = arr.GetLength(0)
    let colLen = arr.GetLength(1)
    if colLen <> state.Length then failwithf "state needs to be a row length"
    let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
    state 
    |> Array.map (fun s -> 
        
    
        )     
    while e.MoveNext() do
        x <- f.Invoke(state, e.Current)
    state        
    arr

