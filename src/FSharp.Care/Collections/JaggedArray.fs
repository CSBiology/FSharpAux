namespace FSharp.Care.Collections

[<AutoOpen>]
module JaggedArray =
    
    // Transposes a jagged array
    let transpose (arr: 'T [][]) =
        if arr.Length > 0 then 
            let colSize = arr.[0].Length
            Array.init (colSize) (fun rowI ->  Array.init (arr.Length) (fun colI -> (arr.[colI].[rowI])))
        else
            arr

    // Converts a jagged list into a jagged array
    let ofJaggedList (data: 'T list list) =
        data
        |> List.map (fun l -> l |> Array.ofList)
        |> Array.ofList


    // Converts a jagged array into a jagged list
    let toJaggedList (arr: 'T [][]) =
        arr
        |> Array.map (fun a -> a |> List.ofArray)
        |> List.ofArray

    // Converts a jagged Seq into a jagged array
    let ofJaggedSeq (data: seq<#seq<'T>>) =
        data
        |> Seq.map (fun s -> s |> Array.ofSeq)
        |> Array.ofSeq

    // Converts a jagged array into a jagged seq
    let toJaggedSeq (arr: 'T [][]) =
        arr
        |> Seq.map (fun s -> s |> Array.toSeq) 

    /// Shuffels each column of a jagged array separately  (method: Fisher-Yates)
    let shuffleColumnWise (arr: 'T [][]) =
        if arr.Length > 0 then 
            let random      = new System.Random()
            let rowCount    = arr.Length
            let columnCount = arr.[0].Length
            
            for ci = columnCount - 1 downto 0 do 
                for ri = rowCount downto  1 do
                    // Pick random element to swap.
                    let rj = random.Next(ri) // 0 <= j <= i-1
                    // Swap.
                    let tmp         =  arr.[rj].[ci]
                    arr.[rj].[ci]     <- arr.[ri - 1].[ci]
                    arr.[ri - 1].[ci] <- tmp
            arr            

        else
            arr



    /// Shuffels each row of a jagged array separately  (method: Fisher-Yates)
    let shuffleRowWise (arr: 'T [][]) =
        if arr.Length > 0 then 
            let random      = new System.Random()
            let rowCount    = arr.Length
            let columnCount = arr.[0].Length
            
            for ri = rowCount - 1 downto  0 do
                for ci = columnCount downto 1 do 
                    // Pick random element to swap.
                    let cj = random.Next(ci) // 0 <= j <= i-1
                    // Swap.
                    let tmp           =  arr.[ri].[cj]
                    arr.[ri].[cj]     <- arr.[ri].[ci - 1]
                    arr.[ri].[ci - 1] <- tmp
            arr            

        else
            arr


    /// Shuffels a jagged array (method: Fisher-Yates)
    let shuffle (arr: 'T [][]) =
        if arr.Length > 0 then 
            let random      = new System.Random()
            let rowCount    = arr.Length
            let columnCount = arr.[0].Length
            for ri = rowCount downto 1 do
                for ci = columnCount downto 1 do 
                    // Pick random element to swap.
                    let rj = random.Next(ri) // 0 <= j <= i-1
                    let cj = random.Next(ci)
                    // Swap.
                    let tmp               =  arr.[rj].[cj]
                    arr.[rj].[cj]         <- arr.[ri - 1].[ci - 1]
                    arr.[ri - 1].[ci - 1] <- tmp
            arr            

        else
            arr

[<AutoOpen>]
module JaggedList =
    

    // Transposes a jagged array
    let transpose (data: 'T list list) =
        let rec transpose = function
            | (_::_)::_ as M -> List.map List.head M :: transpose (List.map List.tail M)
            | _ -> []
        transpose data


    // Converts a jagged list into a jagged array     
    let toJaggedList (data: 'T list list) =
        data
        |> List.map (fun l -> l |> Array.ofList)
        |> Array.ofList


    // Converts a jagged array into a jagged list
    let ofJaggedArray (arr: 'T [][]) =
        arr
        |> Array.map (fun a -> a |> List.ofArray)
        |> List.ofArray

    // Converts a jagged Seq into a jagged list
    let ofJaggedSeq (data: seq<#seq<'T>>) =
        data
        |> Seq.map (fun s -> s |> List.ofSeq)
        |> List.ofSeq

    // Converts a jagged list into a jagged seq
    let toJaggedSeq (data: 'T list list) =
        data
        |> Seq.map (fun s -> s |> List.toSeq) 



