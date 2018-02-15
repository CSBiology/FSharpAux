namespace FSharpAux

[<AutoOpen>]
module Array2D =

    /// Create an empty array2D
    let empty<'a> =
        Array2D.create 0 0 (Unchecked.defaultof<'a>)        

    /// Converts a array2D with one row to array
    let toArray (arr:_[,]) = 
        Array.init arr.Length (fun i -> arr.[0, i])

    /// Converts a array2D to jagged array
    let toJaggedArray (arr:_[,]) =         
        Array.init (Array2D.length1(arr)) (fun i -> toArray(arr.[i..i,0..]))

    /// Converts a jagged array (twodimensional) into a array2D
    let ofJaggedArray (arr:_[][]) = 
        if arr.Length > 0 then 
            let colSize = (arr |> Array.maxBy (fun t -> t.Length)).Length
            Array2D.init (arr.Length) colSize (fun rowI colI -> (arr.[rowI].[colI]))
        else
            Array2D.create 0 0 (Unchecked.defaultof<'a>)

    /// concate the columns of an Array2D into a long column array
    let toLongColumnArray (arr:_[,]) =
        let n = arr |> Array2D.length1
        let m = arr |> Array2D.length2
        Array.init (n*m) (fun i-> arr.[i%n, i/n])
    
    /// Creates a Array2D out of a column array [a1;b1;c1;a2;b2;c2;...]
    let ofLongColumnArray (arr:_[]) nrow ncol =
        if arr.Length <> nrow * ncol then failwith "array length != nrow * ncol "
        Array2D.init nrow ncol (fun i j -> arr.[j*nrow+i])

    // ############################################
    // Flattens a 2D array into a sequence
    let array2D_to_seq (arr:_[,]) =  
        seq {for i in 0..Array2D.length1 arr - 1 do
                for j in 0..Array2D.length2 arr - 1 do yield arr.[i,j]}

    ///Returns the element of the array which is the biggest after projection according to the Operators.max operator
    let maxBy projection (arr: _ [,])  =
        let n,m = arr |> Array2D.length1, arr |> Array2D.length2
        let rec compareMax i j max =
            if j = m then max
            else
                let value = arr.[i,j]
                if (projection value) < (projection max) then compareMax i (j+1) max 
                else compareMax i (j+1) value 
        let rec countRow max i = 
            if i = n then max
            else countRow (compareMax i 0 max) (i+1)
        countRow arr.[0,0] 0

    ///Returns the index of the element which is the biggest after projection according to the Operators.max operator in the array
    let indexMaxBy projection (arr: _ [,])  =
        let n,m = arr |> Array2D.length1, arr |> Array2D.length2
        let rec compareMax i j max indexMax =
            if j = m then max,indexMax
            else
                let value = arr.[i,j]
                if (projection value) < (projection max) then compareMax i (j+1) max indexMax
                else compareMax i (j+1) value (i,j)
        let rec countRow i (max,indexMax) = 
            if i = n then indexMax
            else countRow  (i+1) (compareMax i 0 max indexMax)
        countRow 0 (arr.[0,0],(0,0))        

    /// Transpose the array
    let transpose (arr:_[,]) =
         Array2D.init (arr.GetLength(1)) (arr.GetLength(0)) (fun i j -> arr.[j,i])


    /// Returns the numbers of rows.
    let rowCount (arr: 'T [,]) =
        arr.GetLength(0)


    /// Returns the numbers of rows.
    let colCount (arr: 'T [,]) =
        arr.GetLength(1)


    /// Fold one column.
    let inline foldCol f state (arr: 'T [,]) colI =
        let rowCount = arr.GetLength(0)
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        let rec loop i acc =
            if (i < rowCount) then 
                loop (i+1) (f.Invoke(acc, arr.[i,colI]))
            else
                acc
        loop 0 state
    

    /// Fold one row.
    let inline foldRow f state (arr: 'T [,]) rowI =
        let colCount = arr.GetLength(1)
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        let rec loop i acc =
            if (i < colCount) then 
                loop (i+1) (f.Invoke(acc, arr.[rowI,i]))
            else
                acc
        loop 0 state


    /// Fold all columns into one row array.
    let inline foldByCol f state (arr: 'T [,]) =
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)
        let a = Array.zeroCreate colCount 
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to colCount-1 do
            let mutable macc = state
            for i=0 to rowCount-1 do
                macc <- f.Invoke(macc, arr.[i,k])
            a.[k] <- macc
        a


    /// Fold all rows into one column array.
    let inline foldByRow f state (arr: 'T [,]) =
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)
        let a = Array.zeroCreate rowCount
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to rowCount-1 do
            let mutable macc = state
            for i=0 to colCount-1 do
                macc <- f.Invoke(macc, arr.[k,i])
            a.[k] <- macc
        a



    /// Builds a new array whose elements are the results of applying the given function
    /// to each of the elements of the array. The integer indices passed to the
    /// function indicates the column index of the element being transformed.
    let mapColI f (arr: 'T [,]) =  
        Array2D.mapi (fun rowI colI v -> f colI v) arr


    /// Builds a new array whose elements are the results of applying the given function
    /// to each of the elements of the array. The integer indices passed to the
    /// function indicates the row index of the element being transformed.
    let mapRowI f (arr: 'T [,]) =  
        Array2D.mapi (fun rowI colI v -> f rowI v) arr



    /// Applying the given function to each of the elements of the array and returns the value in place.
    let mapInPlace f (arr: 'T [,]) =  
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)    
        for k=0 to colCount-1 do
            for i=0 to rowCount-1 do
                arr.[i,k] <- f arr.[i,k]
        arr 

    /// Applying the given function to each of the elements of the array and returns the value in place.
    /// The integer indices passed to the function indicates the element being transformed.
    let mapiInPlace f (arr: 'T [,]) =  
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to colCount-1 do
            for i=0 to rowCount-1 do
                arr.[i,k] <- f.Invoke(k, arr.[i,k])
        arr 

    /// Applying the given function to each of the elements of the array and returns the value in place.
    /// The integer indices passed to the function indicates the column index of the element being transformed.
    let mapInPlaceColi f (arr: 'T [,]) =  
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to colCount-1 do
            for i=0 to rowCount-1 do
                arr.[i,k] <- f.Invoke(k, arr.[i,k])
        arr 

    /// Applying the given function to each of the elements of the array and returns the value in place.
    /// The integer indices passed to the function indicates the row index of the element being transformed.
    let mapInPlaceRowi f (arr: 'T [,]) =  
        let rowCount,colCount = arr.GetLength(0),arr.GetLength(1)
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to colCount-1 do
            for i=0 to rowCount-1 do
                arr.[i,k] <- f.Invoke(i, arr.[i,k])
        arr 


    /// Builds a new array whose elements are the results of applying the given function
    /// to the corresponding pairs of elements from the two arrays. 
    let map2 f (arr1: 'T [,]) (arr2: 'T [,]) =  
        let rowCount,colCount = arr1.GetLength(0),arr1.GetLength(1)
        if rowCount <> arr2.GetLength(0) then failwith ""
        if colCount <> arr2.GetLength(1) then failwith ""
        let arr = Array2D.zeroCreate rowCount colCount
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        for k=0 to colCount-1 do
            for i=0 to rowCount-1 do
                arr.[i,k] <- f.Invoke(arr1.[i,k], arr2.[i,k])
        arr 


    /// Shuffels each column of the input Array2D separately  (method: Fisher-Yates)
    let shuffleColumnWise (arr:_[,]) =  
        let random      = new System.Random()
        let rowCount    = arr |> Array2D.length1
        let columnCount = arr |> Array2D.length2

        for ci = columnCount - 1 downto 0 do 
            for ri = rowCount downto  1 do
                // Pick random element to swap.
                let rj = random.Next(ri) // 0 <= j <= i-1
                // Swap.
                let tmp         =  arr.[rj,ci]
                arr.[rj,ci]     <- arr.[ri - 1,ci]
                arr.[ri - 1,ci] <- tmp
        arr


    /// Shuffels each row of the input Array2D separately  (method: Fisher-Yates)
    let shuffleRowWise (arr:_[,]) =  
        let random      = new System.Random()
        let rowCount    = arr |> Array2D.length1
        let columnCount = arr |> Array2D.length2

        for ri = rowCount - 1 downto  0 do
            for ci = columnCount downto 1 do 
                // Pick random element to swap.
                let cj = random.Next(ci) // 0 <= j <= i-1
                // Swap.
                let tmp         =  arr.[ri,cj]
                arr.[ri,cj]     <- arr.[ri,ci - 1]
                arr.[ri,ci - 1] <- tmp
        arr


    /// Shuffels the input Array2D (method: Fisher-Yates)
    let shuffle (arr:_[,]) =  
        let random      = new System.Random()
        let rowCount    = arr |> Array2D.length1
        let columnCount = arr |> Array2D.length2

        for ri = rowCount downto 1 do
            for ci = columnCount downto 1 do 
                // Pick random element to swap.
                let rj = random.Next(ri) // 0 <= j <= i-1
                let cj = random.Next(ci)
                // Swap.
                let tmp             =  arr.[rj,cj]
                arr.[rj,cj]         <- arr.[ri - 1,ci - 1]
                arr.[ri - 1,ci - 1] <- tmp
        arr

    ///Applies a keyfunction to each element and counts the amount of each distinct resulting key
    let countDistinctBy (keyf : 'T -> 'Key) (arr: 'T [,]) =
        let dict = System.Collections.Generic.Dictionary<_, ref<int>> HashIdentity.Structural<'Key>
        // Build the distinct-key dictionary with count
        Array2D.iter (fun v -> 
                let key = keyf v
                match dict.TryGetValue(key) with
                | true, count ->
                        count := !count + 1 //If it matches a key in the dictionary increment by one
                | _ -> 
                    dict.[key] <- ref 1 //If it doesnt match create a new count for this key
        ) arr
        //Write to array
        [|
        for v in dict do yield v.Key |> Operators.id,!v.Value
        |]


