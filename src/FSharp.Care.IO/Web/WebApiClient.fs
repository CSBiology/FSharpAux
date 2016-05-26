namespace FSharpAux.IO

open System
open FSharpAux

open Newtonsoft.Json
//open FSharp.Data
//open FSharp.Data.HttpRequestHeaders

module WebApiClient =

    module JSON =    
        
        let fromJSON<'a>  = 
            let js = new JsonSerializerSettings() 
            js.ContractResolver <- new Newtonsoft.Json.Serialization.DefaultContractResolver()

            fun jstring -> JsonConvert.DeserializeObject<'a>(jstring,js)

        let toJSON<'a> = 
            let js = new JsonSerializerSettings() 
            js.ContractResolver <- new Newtonsoft.Json.Serialization.DefaultContractResolver() 

            fun (o:'a) -> JsonConvert.SerializeObject(unbox o,js)

    let a = 1
//    let generateWebRequest login =
//        Either.tryCatch 
//            (fun urlString ->  Http.Request( urlString ,cookieContainer = login , silentHttpErrors = false))
//            (fun exn -> exn.Message )// "Fail to create Request") 
//
//    // switch 
//    let validateHttpResponseText (response:HttpResponse) =
//        if response.StatusCode > 199 && response.StatusCode < 300 then
//            match response.Body with
//            | Text text ->     Either.succeed text
//            | Binary binary -> Either.fail (sprintf "Expecting text, but got a binary response (%d bytes)" binary.Length)
//        else
//                               Either.fail (sprintf "Error: %i" response.StatusCode)
//    
//    let httpResponseToJSON<'a> login  =    
//        (generateWebRequest login)
//        >> Either.bind validateHttpResponseText
//        >> Either.bind (Either.tryCatch JSON.fromJSON<'a> (fun exn -> "Fail to convert object to JSON") ) 