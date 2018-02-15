namespace FSharp.Care

open System.Text.RegularExpressions

/// Regex module for functional use of regular expressions
// Test regex under: http://regexstorm.net/tester
module Regex =


    [<System.FlagsAttribute>]
    /// Provides enumerated values to use to set regular expressions options
    type RegexOptions =
        | None                     =   0
        | IgnoreCase               =   1
        | Multiline                =   2
        | ExplicitCapture          =   4
        | Compiled                 =   8
        | Singleline               =  16

        | IgnorePatternWhitespace  =  32
        | RightToLeft              =  64
        | ECMAScript               = 256
        | CultureInvariant         = 512

    /// Creates a regex 
    let createRegex (regexOptions:RegexOptions) pattern =
        new Regex(pattern, regexOptions |> box :?> System.Text.RegularExpressions.RegexOptions )

    

