namespace FSharp.Care.Collections

[<AutoOpen>]
module Array = 
    
    let inline contains x (arr: 'T []) =
        let mutable found = false
        let mutable i = 0
        let eq = LanguagePrimitives.FastGenericEqualityComparer

        while not found && i < arr.Length do
            if eq.Equals(x,arr.[i]) then
                found <- true
            else
                i <- i + 1
        found
    
    let removeIndex index (arr : 'T []) =
        if index < arr.Length then
            //Array.init (arr.Length - 2) (fun i -> if i <> index then arr.[i])
            Array.ofSeq ( seq { for i = 0 to arr.Length - 1  do if i <> index then yield arr.[i] } )
        else
            invalidArg "index" "index not present in array"

    let scanSubRight f (arr : _[]) start fin initState = 
        let mutable state = initState 
        let res = Array.create (2+fin-start) initState 
        for i = fin downto start do
            state <- f arr.[i] state;
            res.[i - start] <- state
        done;
        res

    let scanSubLeft f  initState (arr : _[]) start fin = 
        let mutable state = initState 
        let res = Array.create (2+fin-start) initState 
        for i = start to fin do
            state <- f state arr.[i];
            res.[i - start+1] <- state
        done;
        res


    let scanReduce f (arr : _[]) = 
        let arrn = arr.Length
        if arrn = 0 then invalidArg "arr" "the input array is empty"
        else scanSubLeft f arr.[0] arr 1 (arrn - 1)

    let scanReduceBack f (arr : _[])  = 
        let arrn = arr.Length
        if arrn = 0 then invalidArg "arr" "the input array is empty"
        else scanSubRight f arr 0 (arrn - 2) arr.[arrn - 1]

    
    /// Stacks an array of arrays horizontaly
    let stackHorizontal (arrs:array<array<'t>>) =
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init arrs.Length  aMinlength (fun i ii -> arrs.[i].[ii])
        else
            invalidArg "arr" "the input arrays are of different length"

    /// Stacks an array of arrays vertivaly
    let stackVertical (arrs:array<array<'t>>) =    
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init aMinlength arrs.Length (fun i ii -> arrs.[ii].[i])
        else
        invalidArg "arr" "the input arrays are of different length"


    /// Shuffels the input array (method: Fisher-Yates)
    let shuffleFisherYates (arr : _[]) =
        let random = new System.Random()
        for i = arr.Length downto 1 do
            // Pick random element to swap.
            let j = random.Next(i) // 0 <= j <= i-1
            // Swap.
            let tmp = arr.[j]
            arr.[j] <- arr.[i - 1]
            arr.[i - 1] <- tmp
        arr  


    /// Look up an element in an array by index, returning a value if the index is in the domain of the array and default value if not
    let tryFindDefault  (arr : _[]) (zero : 'value) (index : int) =
        match arr.Length > index with
        | true  -> arr.[index]
        | false -> zero


    /// Returns an array of sliding windows of data drawn from the source array.
    /// Each window contains the n elements surrounding the current element.
    let centeredWindow n (source: _ []) =    
        if n < 0 then invalidArg "n" "n must be a positive integer"
        let lastIndex = source.Length - 1
    
        let window i _ =
            let windowStartIndex = System.Math.Max(i - n, 0)
            let windowEndIndex = System.Math.Min(i + n, lastIndex)
            let arrSize = windowEndIndex - windowStartIndex + 1
            let target = Array.zeroCreate arrSize
            Array.blit source windowStartIndex target 0 arrSize
            target

        Array.mapi window source


// ########################################
// Static extensions

    type 'T ``[]`` with
        
        /// Look up an element in an array by index, returning a value if the index is in the domain of the array and default value if not
        member this.TryFindDefault  (arr : _[]) (zero : 'value) (index : int) =
            match arr.Length > index with
            | true  -> arr.[index]
            | false -> zero
    



//    /// Adds two arrays pointwise
//    let inline (.+) a b = Array.map2 (+) a b
//    /// Substracts two arrays pointwise
//    let inline (.-) a b = Array.map2 (-) a b
//    /// Multipies two arrays pointwise
//    let inline (.*) a b = Array.map2 (*) a b
//    /// Divides two arrays pointwise
//    let inline (./) a b = Array.map2 (/) a b

//    type 'T ``[]`` with
//        static member inline (+) (a b = Array.map2 (+) a b
