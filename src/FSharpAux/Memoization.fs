namespace FSharpAux

// https://en.wikibooks.org/wiki/F_Sharp_Programming/Caching
// Although dictionary lookups occur in constant time, the hash function used by the dictionary can take an arbitrarily long time to execute (this is especially true with strings, where
// the time it takes to hash a string is proportional to its length). For this reason, it is wholly possible for a memoized function to have less performance than an unmemoized function. Always profile
// code to determine whether optimization is necessary and whether memoization genuinely improves performance.

/// Memoization pattern
module Memoization =

    /// Memoizes the return value of function f (Attenetion: not thread-safe)
    let memoize f =
        let dict = new System.Collections.Generic.Dictionary<_,_>()
        fun n ->
            match dict.TryGetValue(n) with
            | (true, v) -> v
            | _ ->
                let temp = f(n)
                dict.Add(n, temp)
                temp

    /// Memoizes the return value of function f (thread-safe)
    let memoizeP f =
        let dict = new  System.Collections.Concurrent.ConcurrentDictionary<'a,'b>()
        fun n ->             
            match dict.TryGetValue(n) with
            | (true, v) -> v
            | _ ->
                let temp = f(n)
                dict.TryAdd(n, temp) |> ignore
                temp

        


//    // Example            
//    let rec fib = memoize(fun n ->
//        if n = 0I then 0I
//        elif n = 1I then 1I
//        else fib (n - 1I) + fib (n - 2I) )
