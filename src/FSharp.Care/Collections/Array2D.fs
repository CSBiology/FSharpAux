namespace FSharp.Care.Collections

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

    /// Transpose the array
    let transpose (arr:_[,]) =
         Array2D.init (arr.GetLength(1)) (arr.GetLength(0)) (fun i j -> arr.[j,i])


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
