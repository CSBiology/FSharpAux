namespace FSharp.Care.IO.Web


module WebServer =

    open System
    open System.Net
    open System.Text
    open System.IO
 
    /// Starts a minimal loxal webservice    
    let start siteRoot hostName (port:int) =
        
        //let host = "http://localhost:8080/"
        let host = sprintf "http://%s:%i/" hostName port
 
        let listener (handler:(HttpListenerRequest->HttpListenerResponse->Async<unit>)) =
            let hl = new HttpListener()
            hl.Prefixes.Add host
            hl.Start()
            let task = Async.FromBeginEnd(hl.BeginGetContext, hl.EndGetContext)
            async {
                while true do
                    let! context = task
                    Async.Start(handler context.Request context.Response)
            } |> Async.Start
 
        let output (req:HttpListenerRequest) =
            let file = Path.Combine(siteRoot,
                                    Uri(host).MakeRelativeUri(req.Url).OriginalString)
            printfn "Requested : '%s'" file
            if (File.Exists file)
                then File.ReadAllText(file)
                else "File does not exist!"
 
        listener (fun req resp ->
            async {
                let txt = Encoding.ASCII.GetBytes(output req)
                resp.ContentType <- "text/html"
                resp.OutputStream.Write(txt, 0, txt.Length)
                resp.OutputStream.Close()
            })
    

