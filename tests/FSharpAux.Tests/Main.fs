module Plotly.NET.Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

let all =
    testList "All"
        [
            SeqTests.seqTests
            ArrayTests.arrayTests
            #if FABLE_COMPILER
            #else
            Array2DTests.array2dTests
            #endif
            JaggedArrayTest.main
            ListTests.listTests
            MathTests.mathTests
            StringTests.stringTests
        ]

[<EntryPoint>]
let main argv = 
    #if FABLE_COMPILER
    Mocha.runTests all
    #else
    Tests.runTestsWithCLIArgs [] argv all
    #endif
