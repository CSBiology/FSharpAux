namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FSharp.Care.IO")>]
[<assembly: AssemblyProductAttribute("FSharp.Care")>]
[<assembly: AssemblyDescriptionAttribute("Auxiliary functions and data structures for F# programming language")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
    let [<Literal>] InformationalVersion = "1.0"
