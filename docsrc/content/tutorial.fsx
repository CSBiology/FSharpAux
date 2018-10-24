(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
//#r @"C:\Users\Kevin\source\repos\kMutagene\FSharp.Formatting\bin\FSharp.Plotly.dll"
//#r @"C:\Users\Kevin\source\repos\CSBiology\FSharpAux\lib\FSharp.Plotly.dll"
//#r @"C:\Users\Kevin\source\repos\CSBiology\FSharpAux\packages\formatting\FSharp.Formatting.CommandTool\tools\FSharp.Plotly.dll"
//open FSharp.Plotly
#r "netstandard"
(**
Introducing your project
========================

Say more

*)
#r "FSharpAux.dll"
open FSharpAux

Library.hello 0
(**
Some more info
*)
//[<AutoOpen>]
//module CustomTransformations = 
//    let transformPlotlyChart (value:obj, typ:System.Type) =
//        match value with
//        | :? int as i -> false
//        | :? FSharp.Plotly.GenericChart.GenericChart as ch ->
//            printfn "Chart detected"
//            // Just return the inline HTML for a Plotly chart        
//            let html = GenericChart.toChartHtmlWithSize 700 500 ch
//            true
//        | _ ->
//            printfn "no chart detected : %A" typ
//            false

//let content = """
//let a = 10
//(*** include-value:a ***)"""

//let testData = [0.,1.;2.,5.]


//let testChart = testData |> Chart.Line

//let testType = {Feld = 1337}

//transformPlotlyChart (testChart,testChart.GetType())

(** The result of the TestType snippet is: *)
(*** include-value:testType ***)

//(testChart,testChart.GetType()) |> transformPlotlyChart
(** The result of the Chart snippet is: *)
(*** include-value:testChart ***)

(** 
### Evaluation demo 
The following is a simple calculation: *)
let test = 40 + 2

(** We can print it as follows: *)
(*** define-output:test ***)
printf "Result is: %d" test

(** The result of the previous snippet is: *)
(*** include-output:test ***)

(** And the variable `test` has the following value: *)
(*** include-value: test ***)
