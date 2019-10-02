namespace FSharpAux.IO

open System
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
        [<Obsolete("This function is deprecated. Use Seq.CSV instead")>]
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
        
            //array of functions to extract values
            let valFuncs =
                //first element of data taken as sample to be "analyzed"
                let firstElement = data |> Seq.head
                valFunc firstElement
        
            //values returned by valFuncs
            let values =
                data
                |> Seq.map (fun entry ->
                    valFuncs
                    |> Seq.map (fun func -> func entry)
                    )

            //array of functions to format values
            let strFuncs : seq<(obj -> string)> =
                //first element of values taken as sample to be "analyzed"
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
                        //| ty when (try ty.GetGenericTypeDefinition() = typedefof<_ list>
                        //           with
                        //           | _ -> false) ->
                        // list type
                        | ty when ty.Name = (typeof< _ list>).Name ->
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
                            //apply different formatting to first element of the sequence to prevent separator as first character
                            let stringBuilder = new System.Text.StringBuilder()
                            stringBuilder.Append ((Seq.head strFuncs)(Seq.head x)) |> ignore
                            (Seq.tail x)
                            //fold over string builder to format rest of sequence with correct separator
                            |> Seq.fold2 (fun (sb: System.Text.StringBuilder) (func: obj -> string) value ->
                                sb.AppendFormat (sprintf "%s{0}" separator, (func value))
                                ) stringBuilder (Seq.tail strFuncs)
        
                        let res = sb.ToString()
                        sb.Clear() |> ignore
                        res
                        )
                yield! strings
            }

        ///Returns an array of functions to extract the values of the given types
        static member inline valueFunction (dataEntry: 'a) =
            //giving the datatype as parameter returns an error
            let dataType = typeof<'a>
        
            match dataType with
            |ty when ty.IsValueType             -> [|box|]
            |ty when ty = typeof<string>        -> [|box|]
            |ty when ty = typeof<System.Enum>   -> [|box|]
            |ty when ty.IsArray                 -> [|box|]
            |ty when FSharpType.IsUnion ty      -> [|box|]
            //returns values of the tuple as array, gets also formatted the same as an array in the following functions,
            //except when they are in record types
            |ty when FSharpType.IsTuple ty      -> [|fun (entry: 'a) -> box (FSharpValue.GetTupleFields entry)|]
            |ty when FSharpType.IsRecord ty     ->
        
                Reflection.FSharpType.GetRecordFields(dataType)
                |> Array.map (fun field -> (box >> Reflection.FSharpValue.PreComputeRecordFieldReader field))
            //lists also match with this case
            |_ ->
                [|fun entry ->
                    let a =
                        dataType.GetProperties()
                        |> Array.map (fun prop ->
                                                prop.GetValue(box entry, null))
                    box a|]
        
        ///Returns a function to format a given value as string
        static member inline stringFunction (separator: string) (flatten: bool) (input: 'a) =
            let o = box input
            match o with
            //match string first so that it doesn't get treated as a char array
            | :? string -> 
                fun (x: obj) ->
                    let sb = new System.Text.StringBuilder()
                    sb.Append x |> ignore
                    let res = sb.ToString()
                    sb.Clear() |> ignore
                    res
            | :? System.Collections.IEnumerable ->
                if flatten then
                    fun x ->
                        let sb = new System.Text.StringBuilder()
                        let a = x :?> System.Collections.IEnumerable
                        //iterates over Collections.IEnumerable to get entries as objects for the string builder
                        let b = [for i in a do yield box i]
                        b
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

        /// Convertes a generic sequence to a sequence of seperated string
        /// use write afterwards to save to file
        static member inline CSV (separator: string) (header: bool) (flatten: bool) (data: seq<'a>) =

            Seq.CSVwith Seq.valueFunction Seq.stringFunction separator header flatten data