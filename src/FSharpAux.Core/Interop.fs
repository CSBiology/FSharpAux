namespace FSharpAux


/// Module for facilitated .net interop erability
module Interop =
    
    /// Converts an fsharp function to a System.Func
    let toFunc (f:'a -> 'b) =
        new System.Func<'a,'b>(f)

    /// Converts System.Func to an fsharp function
    let ofFunc (f:System.Func<_,_>) =
        fun x -> f.Invoke(x)
