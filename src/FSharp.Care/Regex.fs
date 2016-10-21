namespace FSharp.Care

open System.Net
open System.Text.RegularExpressions

/// Regex module for functional use of regular expressions
// Test regex under: http://regexstorm.net/tester
module Regex =

    open FSharp.Care.Monads

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

    

    /// Returns the first occurencing match of the pattern 
    let (|RegexMatchValue|_|) (regex:Regex) input =
        let m = regex.Match(input)    
        if m.Success then Some m.Value
        else None

    /// Try to parse str with pattern returns Either monoid. On success f is applied on result string.
    let tryEitherParse f pattern =
        let regex = createRegex RegexOptions.Compiled pattern
        fun str -> 
            match str with
            | RegexMatchValue regex (v) -> Success (f v)
            | _ -> Failure str


    //http://stackoverflow.com/questions/5684014/f-mapping-regular-expression-matches-with-active-patterns

    // Returns the matching group of the first occurence of the pattern 
    let (|FirstRegexGroup|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g ])
        else None

    /// Returns the matching group of all occurences of the pattern
    // For example see Mgf Parser
    let (|RegexGroups|_|) pattern input =
        let m = Regex.Matches(input, pattern)
        if m.Count > 0 then Some([ for m' in m -> m'.Groups ])
        else None


    /// Returns the matching values the first occurence of the pattern 
    let (|RegexValue|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None

    /// Returns the matching values of all occurences of the pattern
    let (|RegexValues|_|) pattern input =
        let m = Regex.Matches(input, pattern)
        if m.Count > 0 then Some([ for g in m -> g.Value ])
        else None


    /// Returns the matching group values of all occurences of the pattern
    let (|RegexGroupValues|_|) pattern input =
        let m = Regex.Matches(input, pattern)
        if m.Count > 0 then Some([ for m' in m -> [for g in m'.Groups -> g.Value] ])
        else None


//    let matchExample =
//        match "input" with
//            | RegexValue @"Cre(?<chromosome>[\d]*)\.(?<locusId>g[\d]*)\.t(?<spliceId>[\d]*)\.(?<version>[\d]*)" [ chromosome; locusId; spliceId; version; ] -> Some( chromosome, locusId, spliceId, version)
//            | _ -> None


    /// Try to parse source string with pattern returns Either monoid with match and remaining string. On success f is applied on both result string.
    let tryEitherMatchReplace f pattern =
        let replaceMatch (m:Match) (source:string) (replacement:string) =
            source.Substring(0, m.Index) + replacement + source.Substring(m.Index + m.Length)    
        let regex = createRegex RegexOptions.Compiled pattern
        fun replacement source -> 
            let m = regex.Match(source) 
            if m.Success then Success (f m.Value (replaceMatch m source replacement))
            else Failure source
