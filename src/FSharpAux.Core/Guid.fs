namespace FSharpAux


/// Wrapper to use System Guid
module Guid =
    
    open System
    
    /// Empty System.Guid
    let empty = Guid.Empty
    
    /// Parse a string to a System.Guid
    let ofString (s:string) =  new Guid(s)

    /// Creates a new unique Syste.Guid
    let create = Guid.NewGuid()

    
