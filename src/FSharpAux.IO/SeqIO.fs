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
            let csvReader = SchemaReader.Csv.CsvReader<'schema>(schemaMode=schemaMode)
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
        static member inline toCSV (separator:string) (header:bool) (data:'a seq) =

            let inline toPrettyString sep input =
                let o = box input
                match o with
                | :? string as s -> sprintf "%s" s
                | :? System.Enum as en -> string en
                | :? System.Collections.IEnumerable as e -> seq { for i in e do yield sprintf "%A" i } |> String.concat sep
                | _ -> sprintf "%A" input


            let toPrettyHeaderString sep input fieldName  =
                let o = box input
                match o with
                | :? string       -> fieldName
                | :? System.Enum  -> fieldName
                | :? System.Collections.IEnumerable as e -> let count = seq {for i in e do yield i.ToString() } |> Seq.length
                                                            seq { for c = 1 to count do yield (sprintf "%s%i" fieldName c) } |> String.concat sep
                | _               -> fieldName

            seq {
                let dataType=typeof<'a>

                if header && (Seq.length(data) > 0) then
                    let firstElement = Seq.head data
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
                            data |> Seq.map (fun x -> toPrettyHeaderString separator x dataType.Name) |> String.concat separator
                        | ty when ty = typeof<System.Enum> -> dataType.Name
                        // union type
                        | ty when FSharpType.IsUnion ty -> dataType.Name
                        // record type
                        | ty when FSharpType.IsRecord ty ->
                            let fields = Reflection.FSharpType.GetRecordFields(dataType)
                            fields
                            |> Seq.map(fun field -> toPrettyHeaderString separator (FSharpValue.GetRecordField(firstElement,field)) field.Name)
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
                    // simple value type to string
                    | ty when ty.IsValueType ->
                        data |> Seq.map (fun x -> sprintf "%A" x)
                    // string to string ::
                    | ty when ty = typeof<string>      -> data |> Seq.map (fun x -> x.ToString())
                    // enum type
                    | ty when ty = typeof<System.Enum> -> data |> Seq.map (fun x -> x.ToString())
                    // array type to string
                    | ty when ty.IsArray ->
                        data |> Seq.map (toPrettyString separator)
                    | ty when ty = typeof<System.Enum> -> data |> Seq.map (fun x -> x.ToString())
                    // union type
                    | ty when FSharpType.IsUnion ty -> data |> Seq.map (fun x -> sprintf "%A" x)
                    // record type
                    | ty when FSharpType.IsRecord ty ->
                        let fields = Reflection.FSharpType.GetRecordFields(dataType)
                        let elemToStr (elem:'record) =
                            //for each field get value
                            fields
                            |> Seq.map(fun field -> toPrettyString separator (FSharpValue.GetRecordField(elem,field)) )
                            |> String.concat separator
                        data |> Seq.map elemToStr
                    // tuple type
                    | ty when FSharpType.IsTuple ty ->
                        data |> Seq.map FSharpValue.GetTupleFields |> Seq.map (toPrettyString separator)
                    // objects
                    | _ ->
                        let props = dataType.GetProperties()
                        data |> Seq.map ( fun line ->
                                    props |> Array.map ( fun prop ->
                                    prop.GetValue(line, null) )) |> Seq.map (toPrettyString separator)

                yield! lines
            }