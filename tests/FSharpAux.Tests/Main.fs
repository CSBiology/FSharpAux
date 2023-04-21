module Plotly.NET.Tests

open Expecto

[<Tests>]
let all =
    testList "All"
        [
            SeqTests.seqTests
            ArrayTests.arrayTests
            Array2DTests.array2dTests
            JaggedArrayTest.main
            ListTests.listTests
        ]

[<EntryPoint>]
let main argv = Tests.runTestsWithCLIArgs [] argv all
