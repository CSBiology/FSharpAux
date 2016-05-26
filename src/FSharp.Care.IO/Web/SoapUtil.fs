namespace FSharpAux.IO.WebServices

module SoapUtil =

    open System.Xml.Serialization
    open System.Runtime.Serialization
    open System.IO
    open System.Xml
    open System.Net
    open System

    [<CLIMutable>]
    [<SoapType(TypeName="Fault",Namespace ="http://schemas.xmlsoap.org/soap/envelope/")>]
    type SoapFault = {
        [<SoapElement(ElementName="faultcode")>]
        FaultCode:string;
        [<SoapElement(ElementName="faultstring")>]
        FaultString:string;
        [<SoapElement(ElementName="faultactor")>]
        FaultActor:string;
        [<SoapIgnore>]        
        mutable Detail:string
        }       
   
    type SoapRequest<'a> = {Body:'a}
    let createSoapRequest o = {Body=o}

    type SoapResponseBody<'a> = {Body:'a;Xml:string}
    type SoapResponse<'a> = 
        | Body of SoapResponseBody<'a>
        | Fault of SoapFault

    let createSoapResponse o xml = SoapResponse<'b>.Body({Body=o; Xml=xml}) 
    let createSoapFault fault = SoapResponse<'b>.Fault(fault) 

    let xmlns_xmlns = "http://www.w3.org/2000/xmlns/"
    let xmlns_soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
    let xmlns_soapenc="http://schemas.xmlsoap.org/soap/encoding/"
    let xmlns_xsi="http://www.w3.org/2001/XMLSchema-instance"
    let xmlns_xsd="http://www.w3.org/2001/XMLSchema"

    let copyStream (s:Stream) = 
        let ms = new MemoryStream()
        s.CopyTo(ms)
        new MemoryStream(ms.ToArray())

    let toString (s:Stream) =
        let ms = copyStream s 
        System.Text.Encoding.UTF8.GetString(ms.ToArray())

    let printStream (s:Stream) =               
        printfn "%s" (toString s)

    let toXmlBody<'b>( s:obj) = s :?> 'b

    let serialize(msg:SoapRequest<'a>) =           
        let serializer = new XmlSerializer(typedefof<'a>)  
        let settings = new XmlWriterSettings()
        settings.Encoding <- System.Text.Encoding.UTF8
        settings.ConformanceLevel <- ConformanceLevel.Document
        settings.Indent <- true
        settings.OmitXmlDeclaration <- true
        let stream = new MemoryStream()
        let writer = XmlTextWriter.Create(stream,settings)
        writer.WriteStartDocument()       
        //<soap:Envelope>
        writer.WriteStartElement("soapenv", "Envelope", xmlns_soapenv)
        writer.WriteAttributeString("xmlns", "xsi", xmlns_xmlns, xmlns_xsi);
        writer.WriteAttributeString("xmlns", "xsd", xmlns_xmlns, xmlns_xsd);
        writer.WriteAttributeString("xmlns", "soapenc", xmlns_xmlns, xmlns_soapenc)    
        //<soap:Body>
        writer.WriteStartElement("soapenv", "Body", xmlns_soapenv)
        serializer.Serialize(writer, msg.Body) 
        writer.WriteEndElement() //<soap:Body/>
        writer.WriteEndElement() //<soap:Envelope/>       
        writer.WriteEndDocument()   
        writer.Close() 
        let xml = System.Text.Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()  
        xml

    let deserialize<'b>(stream:Stream) =        
        let importer = new SoapReflectionImporter()
        let myMapping = importer.ImportTypeMapping(typedefof<'b>)
        let deserializer = new XmlSerializer(myMapping)       
        use reader = XmlTextReader.Create(stream)
        if reader.ReadToFollowing("Body", xmlns_soapenv) then
            if reader.Read() then                                                      
                deserializer.Deserialize(reader) :?> 'b                
            else
                raise( new System.Net.WebException("Invalid soap xml: empty body element.")) 
        else 
            raise( new System.Net.WebException("Invalid soap xml: body element not found."))                   

    let private readFault(ex:System.Net.WebException) =
        let importer = new SoapReflectionImporter()
        let myMapping = importer.ImportTypeMapping(typedefof<SoapFault>)
        let deserializer = new XmlSerializer(myMapping)   
        let cpResponse = copyStream (ex.Response.GetResponseStream())    
        let reader = XmlTextReader.Create(new StreamReader(cpResponse, System.Text.Encoding.UTF8))
        let fault = if reader.ReadToFollowing("Fault", xmlns_soapenv) then
                        deserializer.Deserialize(reader) :?> SoapFault
                    else
                        {FaultCode=ex.GetType().FullName ; FaultString=ex.Message; FaultActor="SoapUtil.getResponse"; Detail=""}
        cpResponse.Seek(0L, SeekOrigin.Begin) |> ignore
        fault.Detail <- toString cpResponse
        fault

    let getResponse<'a,'b>(msg:SoapRequest<'a>, serviceUrl:string, httpHeaders:seq<string*string>,verbose:bool) =
        let xml = serialize(msg)
        if verbose then            
            printfn "%A" xml
        let webRequest = WebRequest.Create(serviceUrl) :?> HttpWebRequest 
        let (|StringEquals|_|) (s1:string) s2 = 
            if s1.Equals(s2, StringComparison.OrdinalIgnoreCase) 
            then Some () else None
        
        let hasContentType = ref false       
        for key,value in httpHeaders do
            match key with
            | StringEquals "accept" -> webRequest.Accept <- value
            | StringEquals "content-type" -> hasContentType := true; webRequest.ContentType <- value
            | StringEquals "user-agent" -> webRequest.UserAgent <- value
            | StringEquals "referer" -> webRequest.Referer <- value
            | StringEquals "host" -> webRequest.Host <- value     
            | _ -> webRequest.Headers.[key] <- value

        webRequest.Method <- "POST"
        webRequest.Timeout <- 60000
        webRequest.Proxy <- null

        let ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml))
        webRequest.ContentLength <- ms.Length
        let reqStream = webRequest.GetRequestStream()
        ms.WriteTo(reqStream) |> ignore
        let resp = try
                        let webResponse = webRequest.GetResponse()
                        let respStream = webResponse.GetResponseStream()                        
                        if typedefof<'b>.Equals(typedefof<System.String>) then   
                            let xmlText = toString respStream
                            let xmlBody = toXmlBody xmlText               
                            createSoapResponse xmlBody xmlText
                        else     
                            let cpResponse = copyStream (respStream)                                          
                            let o = deserialize<'b>(cpResponse)
                            cpResponse.Seek(0L, SeekOrigin.Begin) |> ignore
                            createSoapResponse o (toString cpResponse)    
                        with
                        | :? System.Net.WebException as ex -> createSoapFault(readFault ex)
        if verbose then
            match resp with
                | SoapResponse.Body d -> printfn "%A" d.Xml
                | SoapResponse.Fault f -> printfn "%A" f.FaultString 
        resp