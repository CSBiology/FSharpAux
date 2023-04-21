namespace FSharpAux

[<AutoOpen>]
module Math =      

//    /// Active pattern returns Even or Odd
//    let inline (|Even|Odd|) (input) = if input % 2G = 0G then Even else Odd
//
//    let inline isEven input =
//        match input with
//        | Even -> true
//        | Odd  -> false


    /// Returns the nth root of x.
    let inline nthRoot n x = x ** (1. / float n)

    /// Returns the logarithm of x to base 2
    let log2 x = System.Math.Log(x,2.0)

    /// Returns the reverted log2 (2^x)
    let revLog2 x = 2.**x

    /// Returns x squared (x^2)
    let inline square x = x * x


    let arsinh x =  
        x + sqrt(square x + 1.) |> log


    /// Rounds a double-precision floating-point value to a specified number of fractional digits.  
    let round (digits:int) (x:float) =
        System.Math.Round(x, digits)
    

        
