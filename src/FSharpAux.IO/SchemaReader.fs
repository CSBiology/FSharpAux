namespace FSharpAux.IO


module SchemaReader =

    //#region open
    open System
    open System.IO
    open System.Reflection
    open Microsoft.FSharp.Reflection

    open FSharpAux
    //#endregion open



    //#region type definitions
    /// Function alias to converter function
    type Converter =
        | Single      of (string     -> obj)
        | Collection  of (string seq -> obj)

    type Index =
        | Int    of int
        | String of string

    type FieldIndex = 
        | Single      of Index
        | Collection  of Index seq
    

    /// Represents a field description for obj generation
    type SchemaItem = {
        FieldIndex     : int
        FieldName      : string; 
        Tag            : FieldIndex;
        Deserialize    : Converter;}


    /// Creates a SchemaItem record type
    let createSchemaItem fieldIndex fieldName tag deserialize =
        { FieldIndex = fieldIndex; FieldName = fieldName; Tag = tag; Deserialize = deserialize; }


    //#endregion type definitions


    // Default type converter
    let defaultTypeConverter _type =
        match _type with
        | t when t = typeof<float>              -> Converter.Single (String.tryParseFloatDefault nan >> box)
        | t when t = typeof<int>                -> Converter.Single (String.tryParseIntDefault -1 >> box)        
        | t when t = typeof<string>             -> Converter.Single (fun(s:string) -> box s)
        | t when t = typeof<bool>               -> Converter.Single (String.tryParseBoolDefault false >> box)        
        | t when t = typeof<Guid>               -> Converter.Single (String.tryParseGuidDefault Guid.Empty >> box)
        | t when t = typeof<char>               -> Converter.Single (String.tryParseCharDefault ' ' >> box)
        ///| t when t = typeof<float[]>         -> Converter.Collection (fun (strs:seq<string>) -> (strs |> Seq.map (fun s -> Strings.tryParseFloatDefault nan s) |> Seq.toArray ) |> box )
        | t -> failwithf "Default type converter: Unknown type %A" t 

        

    //#region FieldAttributes

    
    /// Attributes to be added to fields of a schema record
    module Attribute =
        
        /// An attribute to be added to fields of a schema record type to indicate the
        /// column used in the data format for the schema as index
        type FieldAttribute(indexTag:FieldIndex) =
            inherit Attribute()    
            new (index:int)           = FieldAttribute(FieldIndex.Single (Index.Int index))
            new (index:string)        = FieldAttribute(FieldIndex.Single (Index.String index))    
            new (indices:int[])       = FieldAttribute(FieldIndex.Collection (indices |> Array.map (fun item -> Index.Int item))) 
            new (indices:string[])    = FieldAttribute(FieldIndex.Collection (indices |> Array.map (fun item -> Index.String item)))
            member this.IndexTag = indexTag

 
        
        [<AbstractClass>] 
        /// Abstract class of converter attribute
        /// Implemenation can be used to convert from field type/s to obj
        type ConverterAttribute() =
            inherit Attribute()        
            abstract member convertToObj : Converter 


        /// An attribute to be added to fields of a schema record type to indicate that it should be ignored
        type IgnoreAttribute() =
            inherit Attribute()
    
    //#endregion FieldAttributes



    /// Returns given attribute from property info as optional 
    let private tryGetCustomAttribute<'a> (findAncestor:bool) (propInfo :PropertyInfo) =   
        let attributeType = typeof<'a>
        let attrib = propInfo.GetCustomAttribute(attributeType, findAncestor)
        match box attrib with
        | (:? 'a) as customAttribute -> Some(unbox<'a> customAttribute)
        | _ -> None


    /// Returns Schema as an array of SchemaItems 
    let private getSchema<'Schema> (typeConverter:Type -> Converter) (verbose) (fields:PropertyInfo []) =
        let schemaType = typeof<'Schema>
        let getConverter (defaultTypeConverter: Type -> Converter) (propertyInfo:PropertyInfo) =
            match (tryGetCustomAttribute<Attribute.IgnoreAttribute> false propertyInfo) with
                | Some attrib -> Converter.Single( fun str -> box (Unchecked.defaultof<_>)) // TODO: implement get default function
                | None        -> match (tryGetCustomAttribute<Attribute.ConverterAttribute> true propertyInfo) with
                                 | Some attrib -> attrib.convertToObj
                                 | None        -> defaultTypeConverter propertyInfo.PropertyType


        let getFieldInfo verbose (propertyInfo:PropertyInfo) =
            match (tryGetCustomAttribute<Attribute.FieldAttribute> false propertyInfo) with
                | Some attrib -> (propertyInfo.Name,attrib.IndexTag)
                | _ ->  if verbose = true then printfn "Warning: No SchemaTag attribute"
                        (propertyInfo.Name,(FieldIndex.Single (Index.String propertyInfo.Name)))
 
        

        fields |> Array.mapi( fun fieldIndex field -> 
            let propertyInfo = schemaType.GetProperty(field.Name)
            let propertyInfoName,fieldTag = getFieldInfo verbose propertyInfo
            let deserialize = getConverter typeConverter propertyInfo
            createSchemaItem fieldIndex propertyInfoName fieldTag deserialize)

    
    ///
    type SchemaValues = Map<Index,string> 

        

    //#region RecordSchemaReader
    ///
    type RecordSchemaReader<'Schema>(typeConverter:Type -> Converter, ?verbose) =

        // Grab the object for the type that describes the schema
        let schemaType = typeof<'Schema>
        // Grab the fields from that type
        let fields = FSharpType.GetRecordFields(schemaType)
        // Compute a function to build instances of the schema type. This uses an
        // F# library function.
        let objectBuilder = FSharpValue.PreComputeRecordConstructor(schemaType)  
        
        let verbose = defaultArg verbose false                     
  
        let schema           = getSchema<'Schema> typeConverter verbose fields
        
        let countSchemaIndices (schema:SchemaItem[]) =
            schema |> Seq.sumBy (fun item -> match item.Tag with
                                             | Single(index)       -> 1
                                             | Collection(indices) -> (Seq.length indices))
        
        let schemaIndexCount = countSchemaIndices schema

        /// Returns Schema as an array of SchemaItems
        member this.GetSchema = schema
         

        /// Converts a input value map into the record type
        member this.Convert (words:SchemaValues) =        
            let deserializedData =
                schema 
                |> Array.map( fun schemaItem -> 
                        match schemaItem.Tag,schemaItem.Deserialize with
                        | FieldIndex.Single(index)    , Converter.Single(converter)     ->  words.TryFindDefault String.Empty index   |> converter
                        | FieldIndex.Single(index)    , Converter.Collection(converter) ->  [words.TryFindDefault String.Empty index] |> converter
                        | FieldIndex.Collection(multi), Converter.Collection(converter) ->  multi |> Seq.map  (fun index -> words.TryFindDefault String.Empty index ) |> converter
                        | FieldIndex.Collection(multi), Converter.Single(converter)     ->  failwithf "Type converter: Needs to match collection type"
                                                                                          //multi |> Seq.map  (fun index -> words.TryFindDefault(index,String.Empty)) |> Seq.head |> converter
                                                                                      
                        )
                    
            let obj = objectBuilder(deserializedData)
            unbox<'Schema>(obj)

       
        member this.ConvertExact (words:SchemaValues) =                                                         
            if schemaIndexCount <> (Seq.length(words)) then failwithf "Number of columns do not match the number of attributes"       
            
            // Get value from schemaValues but failes if there is key not found exception
            let getvalue (words:SchemaValues) key = match words.TryFind(key) with
                                                    | Some(value) -> value
                                                    | None -> failwithf "No value in Column %A" key 

            let valueToObject schemaItem =
                match schemaItem.Tag,schemaItem.Deserialize with
                | FieldIndex.Single(index)    , Converter.Single(converter)     -> (getvalue words index)   |> converter
                | FieldIndex.Single(index)    , Converter.Collection(converter) ->  [(getvalue words index)] |> converter
                | FieldIndex.Collection(multi), Converter.Collection(converter) ->  multi |> Seq.map  (fun index -> (getvalue words index)) |> converter
                | FieldIndex.Collection(multi), Converter.Single(converter)     ->  failwithf "Type converter: Needs to match collection type"                           

            
            let deserializedData =
                schema 
                |> Array.map( fun schemaItem -> valueToObject schemaItem)
            let obj = objectBuilder(deserializedData)
            unbox<'Schema>(obj)


    //#endregion RecordSchemaReader



    //#region Csv reader
    module Csv = 

        type SchemaMode = 
            | Exact
            | Fill



        /// 
        type CsvReader<'Schema>(?typeConverter:Type -> Converter, ?schemaMode:SchemaMode,?verbose) =

             // Split string by delimiter
            let split (delim:char) (line:string) = 
               line.Split([|delim|]) |> Array.map( fun s -> s.Trim())

            // Default SchemaMode
            let schemaMode = defaultArg schemaMode SchemaMode.Exact
            let typeConverter = defaultArg typeConverter defaultTypeConverter
            let verbose = defaultArg verbose false

            // Converts header line to header map
            let convertHeaderLine (separator) (headerLine:string) =
                headerLine
                |> split separator
                |> Array.filter (fun name -> not (String.IsNullOrWhiteSpace name))
                |> Array.mapi (fun i name -> (Index.String name, i))
                |> Map.ofArray
           
           
            let schemaReader = new RecordSchemaReader<'Schema>(typeConverter,verbose)  


            /// Converts a seperated string according to the schema
            member this.ReadLine (header:Map<Index,int>) (separator:char) (line) =             
                let splitLine = line |> split separator
                let words     = if header.IsEmpty then
                                    splitLine |> Array.mapi (fun i v -> (Index.Int i,v)) |> Map.ofArray
                                else
                                     header |> Seq.map (fun (kv) -> (kv.Key,splitLine.[kv.Value])) |> Map.ofSeq
                            
                match schemaMode with
                    | SchemaMode.Exact ->  schemaReader.ConvertExact words 
                    | SchemaMode.Fill  ->  schemaReader.Convert words

      
            /// Reads in a file and returns typed rows in a sequence according to the schema
            member this.ReadFile(file, separator:char, firstLineHasHeader:bool, ?skipLines:int, ?skipLinesBeforeHeader:int) =                                   
                let reader = File.OpenText(file)
                let skipLines             = (defaultArg skipLines 0) 
                let skipLinesBeforeHeader = (defaultArg skipLinesBeforeHeader 0) 
                let header = match firstLineHasHeader with
                             | true  -> for i = 1 to skipLinesBeforeHeader do reader.ReadLine() |> ignore
                                        let tmpLine = reader.ReadLine()
                                        if tmpLine = null then 
                                            reader.Close()
                                            Map.empty
                                        else
                                            // Convert header according to SchemaReader Index type
                                            convertHeaderLine separator tmpLine
                             | false -> Map.empty
            
            
                Seq.unfold(fun line -> 
                    if line = null then 
                        reader.Close() 
                        None 
                    else 
                        Some(line,reader.ReadLine())) (reader.ReadLine())
                |> Seq.skip skipLines
                |> Seq.filter ( fun line -> not (String.IsNullOrEmpty line) )
                |> Seq.map    ( fun line -> this.ReadLine header separator line ) 

            /// Reads in a file and returns typed rows in a sequence according to the schema
            member this.ReadFile(file, separator:char, lineHasHeader:string, ?skipLines:int, ?skipLinesBeforeHeader:int) =                                   
                let reader = File.OpenText(file)
                let skipLines             = (defaultArg skipLines 0) 
                let skipLinesBeforeHeader = (defaultArg skipLinesBeforeHeader 0) 
                let header = convertHeaderLine separator lineHasHeader
                             
            
            
                Seq.unfold(fun line -> 
                    if line = null then 
                        reader.Close() 
                        None 
                    else 
                        Some(line,reader.ReadLine())) (reader.ReadLine())
                |> Seq.skip skipLines
                |> Seq.filter ( fun line -> not (String.IsNullOrEmpty line) )
                |> Seq.map    ( fun line -> this.ReadLine header separator line ) 

        //#endregion Csv reader







