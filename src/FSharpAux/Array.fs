namespace FSharpAux

open System.Collections.Generic

[<AutoOpen>]
module Array = 

    open System
    
    /// Stacks an array of arrays horizontally.
    let stackHorizontal (arrs : array<array<'t>>) =
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init arrs.Length aMinlength (fun i ii -> arrs.[i].[ii])
        else
            invalidArg "arr" "the input arrays are of different length"

    /// Stacks an array of arrays vertically.
    let stackVertical (arrs : array<array<'t>>) =    
        let alength = arrs |> Array.map Array.length
        let aMinlength = alength |> Array.min
        let aMaxlength = alength |> Array.max
        if (aMinlength = aMaxlength) then        
            Array2D.init aMinlength arrs.Length (fun i ii -> arrs.[ii].[i])
        else
        invalidArg "arr" "the input arrays are of different length"

    /// Computes the outersection (known as "symmetric difference" in mathematics) of two arrays.
    let outersect arr1 (arr2 : 'T []) =
        let hsS1 = HashSet<'T>(HashIdentity.Structural<'T>)
        arr1 |> Array.iter (hsS1.Add >> ignore)
        hsS1.SymmetricExceptWith arr2
        Array.ofSeq hsS1