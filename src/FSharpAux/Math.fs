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

    

        
