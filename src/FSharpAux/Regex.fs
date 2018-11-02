namespace FSharpAux

open System.Text.RegularExpressions

/// Regex module for functional use of regular expressions
// Test regex under: http://regexstorm.net/tester
module Regex =

    //http://stackoverflow.com/questions/5684014/f-mapping-regular-expression-matches-with-active-patterns
    module Active = 
        /// Returns the first occurencing match of the pattern 
        let (|RegexMatchValue|_|) (regex:Regex) input =
            let m = regex.Match(input)    
            if m.Success then Some m.Value
            else None



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



    /// Returns the first occurencing value of the match 
    let tryParseValue regexStr line = 
        let m = Regex.Match(line,regexStr)
        match m.Success with
        | true -> Some m.Value
        | false -> None

    /// Returns the group values of the first occurencing match
    let parse regexStr line = 
        let m = Regex.Match(line,regexStr)
        match m.Success with
        | true  -> (List.tail [ for g in m.Groups -> g.Value ])
        | false -> []

    /// Returns a seq of group values matching the pattern 
    let parseAll regexStr line = 
        let rec loop (m:Match) =
            seq {
            match m.Success with
            | true -> 
                yield (List.tail [ for g in m.Groups -> g.Value ])
                yield! loop (m.NextMatch())
            | false -> () }
        let m = Regex.Match(line,regexStr)
        loop m


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

    

