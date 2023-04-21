namespace FSharpAux

open System.Collections.Generic

[<AutoOpen>]
module Seq =    
    
    /// Sorts sequence in descending order.
    [<System.Obsolete("Use Seq.sortByDescending instead")>]
    let sortByDesc f s = System.Linq.Enumerable.OrderByDescending(s, new System.Func<'a,'b>(f))

    /// Computes the outersection (known as "symmetric difference" in mathematics) of two sequences.
    let outersect seq1 (seq2 : seq<'T>) : seq<'T> = 
        let hsS1 = HashSet<'T>(HashIdentity.Structural<'T>)
        seq1 |> Seq.iter (hsS1.Add >> ignore)
        hsS1.SymmetricExceptWith seq2
        hsS1
