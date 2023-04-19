namespace FSharpAux

[<AutoOpen>]
module JaggedArray =

    /// Creates a jagged array from a 2D array.
    let ofArray2D (arr : 'T [,]) = Array2D.toJaggedArray arr
