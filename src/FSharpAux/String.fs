namespace FSharpAux

// #####
// The code is take from Fsharpx project and modified
// Special thanks to the original authors under (https://github.com/fsprojects/fsharpx)


open System
open System.Globalization

module String =

    /// Try to parse char else return default value    
    let tryParseCharDefault defaultValue (str:string) =
        match Char.TryParse(str) with
        | (true,c) -> c
        | _ -> defaultValue