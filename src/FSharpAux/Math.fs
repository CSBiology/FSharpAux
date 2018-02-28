namespace FSharpAux

[<AutoOpen>]
module Math =      


    /// Helper for System.Numerics.Complex
    module Complex =
        
        /// Creates a complex number (System.Numerics.Complex)
        let inline toComplex real imaginary = 
            System.Numerics.Complex(float real,float imaginary)

        /// Creates a complex number from only the real part (System.Numerics.Complex)
        let inline toComplexFromReal real = 
            System.Numerics.Complex(float real,0.)

        /// Retruns real part of a complex number (System.Numerics.Complex)
        let fromComplexReal (complex : System.Numerics.Complex) = 
            complex.Real

        /// Retruns real * imaginary (System.Numerics.Complex)
        let fromComplex (complex : System.Numerics.Complex) = 
            (complex.Real,complex.Imaginary)

        
        let toComplexFloatArray (data:seq<float>) =
            data 
            |> Seq.map toComplexFromReal
            |> Seq.toArray
    
        let fromComplexFloatArray (data:System.Numerics.Complex []) =
            data 
            |> Array.map fromComplexReal

//    /// Active pattern returns Even or Odd
//    let inline (|Even|Odd|) (input) = if input % 2G = 0G then Even else Odd
//
//    let inline isEven input =
//        match input with
//        | Even -> true
//        | Odd  -> false




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
    

        
