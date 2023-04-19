module Plotly.NET.Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto

[<Tests>]
#endif
let all =
    testList "All"
        [
            SeqTests.seqTests
            ArrayTests.arrayTests
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
