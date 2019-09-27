namespace FSharpAux.IO

open System.IO
open Microsoft.FSharp.Reflection

[<AutoOpen>]
module SeqIO =

   type Seq =


        /// Reads a file line by line
        static member fromFile (filePath)  =
            FileIO.readFile filePath


        /// This function builds an IEnumerable object that enumerates the file
        /// and splits lines of the given file on-demand
        static member fromFileWithSep (separator:char) (filePath) =
            // The function is implemented using a sequence expression
            seq { let sr = System.IO.File.OpenText(filePath)
                  while not sr.EndOfStream do
                        let line = sr.ReadLine()
                        let words = line.Split separator//[|',';' ';'\t'|]
                        yield words }


        /// Reads a file following a given type record schema
        /// Uses SchemaReader.Csv.CsvReader<'schema>()
        static member fromFileWithCsvSchema<'schema> (filePath,separator:char, firstLineHasHeader:bool, ?skipLines:int, ?skipLinesBeforeHeader:int,?schemaMode) =
            let skipLines             = (defaultArg skipLines 0)
            let skipLinesBeforeHeader = (defaultArg skipLinesBeforeHeader 0)
            let schemaMode = (defaultArg schemaMode SchemaReader.Csv.Exact)
            let csvReader = SchemaReader.Csv.CsvReader<'schema>(SchemaMode=schemaMode)
            csvReader.ReadFile(filePath,separator,firstLineHasHeader,skipLines, skipLinesBeforeHeader)

        /// Writes a sequence to file path
        static member write (path:string) (data:'a seq) = //: 'result =
            use writer = new StreamWriter(path)
            data
            |> Seq.iter writer.WriteLine

        /// Writes a sequence to file path (creates a new file or appends file)
        static member writeOrAppend (path:string) (data:'a seq) = //: 'result =
            use writer = new StreamWriter(path,true)
            data
            |> Seq.iter writer.WriteLine

        /// Convertes a generic sequence to a sequence of seperated string
        /// use write afterwards to save to file

        static member inline toCSVwith (separator:string) (header:bool) (data:'a seq) (formatFunctionsFst: string -> 'b -> (obj -> string)) (formatFunctionsRest: string -> 'b -> (obj -> string)) (formatFunctionHeader: string -> 'b -> string -> string) =

            seq {
                let dataType=typeof<'a>

                if header && (Seq.length(data) > 0) then
                    let (firstElement: 'a) = Seq.head data
                    let header =
                        match dataType with
                        // simple value type to string
                        | ty when ty.IsValueType -> dataType.Name
                        // string to string ::
                        | ty when ty = typeof<string>      -> dataType.Name
                        // enum type
                        | ty when ty = typeof<System.Enum> -> dataType.Name
                        // array type to string
                        | ty when ty.IsArray ->
                            data |> Seq.map (fun x -> formatFunctionHeader separator (box x) dataType.Name) |> String.concat separator
                        | ty when ty = typeof<System.Enum> -> dataType.Name
                        // union type
                        | ty when FSharpType.IsUnion ty -> dataType.Name
                        // record type
                        | ty when FSharpType.IsRecord ty ->
                            let fields = Reflection.FSharpType.GetRecordFields(dataType)
                                         |> Array.map (fun field -> FSharpValue.GetRecordField(firstElement, field), field.Name)
                            fields
                            |> Seq.map(fun (field,name) -> formatFunctionHeader separator field name)
                            |> String.concat separator
                        // tuple type
                        | ty when FSharpType.IsTuple ty ->
                            FSharpType.GetTupleElements dataType
                            |> Seq.mapi (fun idx info -> (sprintf "%s_%i" info.Name idx) ) |> String.concat separator
                        // objects
                        | _ -> dataType.GetProperties()
                                |> Seq.map (fun info -> info.Name) |> String.concat separator
                    yield header


                let lines =
                    match dataType with
                    //simple value type to string
                    | ty when ty.IsValueType ->
                        data |> Seq.map (fun x -> sprintf "%A" x)
                    // string to string ::
                    | ty when ty = typeof<string>      -> data |> Seq.map (fun x -> x.ToString())
                    // enum type
                    | ty when ty = typeof<System.Enum> -> data |> Seq.map (fun x -> x.ToString())
                    // array type to string
                    | ty when ty.IsArray ->
                        let stringFunc = 
                            let fstRecord = data |> Seq.head
                            [|formatFunctionsFst separator fstRecord; formatFunctionsRest separator fstRecord|]
                        data
                        |> Seq.mapi (fun i value ->
                                        if i = 0 then
                                            stringFunc.[0] value
                                        else
                                            stringFunc.[1] value
                                    )
                    | ty when ty = typeof<System.Enum> -> data |> Seq.map (fun x -> x.ToString())
                    // union type
                    | ty when FSharpType.IsUnion ty -> data |> Seq.map (fun x -> sprintf "%A" x)
                    // record type
                    | ty when FSharpType.IsRecord ty ->
                        let fields = Reflection.FSharpType.GetRecordFields(dataType)
                                     |> Array.map (fun field -> Reflection.FSharpValue.PreComputeRecordFieldReader field)
                                     |> Array.toList
                        let stringBuilder = new System.Text.StringBuilder()

                        let stringFuncs =
                            let fstRecord = data |> Seq.head
                            let fieldTypes = 
                                fields 
                                |> List.map (fun field -> field fstRecord)

                            let rec loop n list =
                                if n = fieldTypes.Length then
                                    list |> List.rev
                                elif n = 0 then
                                    loop (n + 1) ((formatFunctionsFst separator fieldTypes.[n])::list)
                                else
                                    loop (n + 1) ((formatFunctionsRest separator fieldTypes.[n])::list)
                            loop 0 []

                        let elemToStr (elem:'record) =
                            //for each field get value
                            fields
                            |> Seq.fold2(fun (sb:System.Text.StringBuilder) (stringFunc:(obj -> string)) fieldFunc -> 
                                sb.Append(stringFunc (fieldFunc elem))) stringBuilder stringFuncs |> ignore
                            let res = stringBuilder.ToString()
                            stringBuilder.Clear() |> ignore
                            res

                        data |> Seq.map elemToStr
                     //tuple type
                    | ty when FSharpType.IsTuple ty ->
                        data |> Seq.map FSharpValue.GetTupleFields 
                        |> Seq.mapi (fun i value ->
                                        if i = 0 then
                                            (formatFunctionsFst separator value)value
                                        else
                                            (formatFunctionsRest separator value)value
                                    )
                    // objects
                    | _ ->
                        let props = dataType.GetProperties()
                                    |> List.ofArray
                        let stringFunc = 
                            let firstElememt = data |> Seq.head
                            props |> List.map ( fun prop ->
                                prop.GetValue(firstElememt, null) )
                                |> List.mapi (fun i value ->
                                    if i = 0 then
                                        formatFunctionsFst separator value
                                    else
                                        formatFunctionsRest separator value
                                        )
                        data |> Seq.map ( fun line ->
                                    props |> List.map ( fun prop ->
                                    prop.GetValue(line, null) ))
                                    |> Seq.map2 (fun func value ->
                                                     func value) stringFunc

                yield! lines
            }

        static member inline toCSV (separator:string) (header:bool) (data:'a seq) =
            
            let inline toPrettyHeaderString sep input fieldName  =
                let o = box input
                match o with
                | :? string       -> fieldName
                | :? System.Enum  -> fieldName
                | :? System.Collections.IEnumerable as e -> let count = seq {for i in e do yield i.ToString() } |> Seq.length
                                                            seq { for c = 1 to count do yield (sprintf "%s%i" fieldName c) } |> String.concat sep
                | _               -> fieldName

            let inline funcPrecHead sep input =
                let o = box input
                match o with
                | :? string -> fun (x: obj) -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat("{0}", x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | :? System.Enum -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat("{0}", x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | :? System.Collections.Generic.IEnumerable<'T> -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    let a = x :?>  System.Collections.Generic.IEnumerable<'T>
                    a
                    |> Seq.iteri (fun i x -> 
                        if i = 0 then 
                            sb.AppendFormat("{0}", x) |> ignore
                        else
                            sb.AppendFormat(sprintf "%s{0}"sep, x) |> ignore
                                )
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | _ -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat("{0}", x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res

            let inline funcPrec sep input =
                let o = box input
                match o with
                | :? string -> fun (x: obj) -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat(sprintf "%s{0}"sep, x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | :? System.Enum -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat(sprintf "%s{0}"sep, x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | :? System.Collections.IEnumerable -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    let a = x :?> System.Collections.IEnumerable
                    for i in a do
                        sb.AppendFormat(sprintf "%s{0}"sep, i) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
                | _ -> fun x -> 
                    let sb = new System.Text.StringBuilder()
                    sb.AppendFormat(sprintf "%s{0}"sep, x) |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res

            Seq.toCSVwith separator header data funcPrecHead funcPrec toPrettyHeaderString