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

        static member inline CSVwith (valFunc: 'a -> ('a -> obj)[]) (strFunc:string -> bool -> obj -> (obj -> string)) (separator: string) (header: bool) (flatten: bool) (data: seq<'a>)=
            
            let inline toPrettyHeaderString sep input fieldName flatten =
                let o = box input
                match o with
                | :? string       -> fieldName
                | :? System.Enum  -> fieldName
                | :? System.Collections.IEnumerable as e -> 
                    if flatten then
                        let count = seq {for i in e do yield i.ToString() } |> Seq.length
                        seq { for c = 1 to count do yield (sprintf "%s%i" fieldName c) } |> String.concat sep
                    else
                        fieldName
                | _               -> fieldName
        
            let valFuncs =
                let firstElement = data |> Seq.head
                valFunc firstElement
        
            let values =
                data
                |> Seq.map (fun entry ->
                    valFuncs
                    |> Seq.map (fun func -> func entry)
                    )
            let strFuncs : seq<(obj -> string)> =
                let firstElement = values |> Seq.head
                firstElement
                |> Seq.map (fun x -> strFunc separator flatten x)
            
            seq {
                let dataType=typeof<'a>
        
                if header && (Seq.length(data) > 0) then
                    let (firstElement: 'a) = Seq.head data
                    //let ty2 = firstElement.GetType()
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
                            data |> Seq.map (fun x -> toPrettyHeaderString separator (box x) dataType.Name flatten) |> String.concat separator
                        | ty when (try ty.GetGenericTypeDefinition() = typedefof<_ list>
                                   with
                                   | _ -> false) ->
                                                    data |> Seq.map (fun x -> toPrettyHeaderString separator (box x) dataType.Name flatten) |> String.concat separator
                        // union type
                        | ty when FSharpType.IsUnion ty -> dataType.Name
                        // record type
                        | ty when FSharpType.IsRecord ty ->
                            let fields = Reflection.FSharpType.GetRecordFields(dataType)
                                         |> Array.map (fun field -> FSharpValue.GetRecordField(firstElement, field), field.Name)
                            fields
                            |> Seq.map(fun (field,name) -> toPrettyHeaderString separator field name flatten)
                            |> String.concat separator
                        // tuple type
                        | ty when FSharpType.IsTuple ty ->
                            if flatten then
                                FSharpType.GetTupleElements dataType
                                |> Seq.mapi (fun idx info -> (sprintf "%s_%i" info.Name idx) ) |> String.concat separator
                            else
                                FSharpType.GetTupleElements dataType
                                |> Seq.mapi (fun idx info -> (sprintf "%s_%i_Tuple" info.Name idx) ) |> String.concat "_"
                        // objects
                        | _ -> dataType.GetProperties()
                               |> Seq.map (fun info -> info.Name) |> String.concat separator
                    yield header
        
        
                let strings =
                    values
                    |> Seq.map (fun x ->
                        let sb =
                            let stringBuilder = new System.Text.StringBuilder()
                            stringBuilder.Append ((Seq.head strFuncs)(Seq.head x)) |> ignore
                            (Seq.tail x)
                            |> Seq.fold2 (fun (sb: System.Text.StringBuilder) (func: obj -> string) value ->
                                sb.AppendFormat (sprintf "%s{0}" separator, (func value))
                                ) stringBuilder (Seq.tail strFuncs)
        
                        let res = sb.ToString()
                        sb.Clear() |> ignore
                        res
                        )
                yield! strings
            }

        
        static member inline valueFunction (dataEntry: 'a) =
        
            let dataType = typeof<'a>
        
            match dataType with
            |ty when ty.IsValueType             -> [|box|]
            |ty when ty = typeof<string>        -> [|box|]
            |ty when ty = typeof<System.Enum>   -> [|box|]
            |ty when ty.IsArray                 -> [|box|]
            |ty when FSharpType.IsUnion ty      -> [|box|]
            |ty when FSharpType.IsTuple ty      -> [|fun (entry: 'a) -> box (FSharpValue.GetTupleFields entry)|]
            |ty when FSharpType.IsRecord ty     ->
        
                Reflection.FSharpType.GetRecordFields(dataType)
                |> Array.map (fun field -> (box >> Reflection.FSharpValue.PreComputeRecordFieldReader field))
            |_ ->
                [|fun entry ->
                    let a =
                        dataType.GetProperties()
                        |> Array.map (fun prop ->
                                                prop.GetValue(box entry, null))
                    box a|]
        
        
        static member inline stringFunction (separator: string) (flatten: bool) (input: 'a) =
            let o = box input
            match o with
            | :? System.Collections.IEnumerable as tmp ->
                if flatten then
                    fun x ->
                        let sb = new System.Text.StringBuilder()
                        let a =
                            [for i in tmp do yield box i]
                        a
                        |> Seq.iteri (fun i x ->
                            if i = 0 then
                                sb.AppendFormat("{0}", x) |> ignore
                            else
                                sb.AppendFormat(sprintf "%s{0}" separator, x) |> ignore
                            )
                        let res = sb.ToString()
                        sb.Clear() |> ignore
                        res
                else
                    fun x -> sprintf "%A" x
            | _ -> 
                fun (x: obj) ->
                    let sb = new System.Text.StringBuilder()
                    sb.Append x |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res

        static member inline CSV (separator: string) (header: bool) (flatten: bool) (data: seq<'a>) =

            Seq.CSVwith Seq.valueFunction Seq.stringFunction separator header flatten data