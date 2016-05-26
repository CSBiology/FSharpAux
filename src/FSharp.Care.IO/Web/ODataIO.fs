namespace FSharpAux.IO.WebServices

open System
open System.Collections.Generic
open System.Runtime.Serialization
open System.Net
open System.Net.Http   

module ODataIO =

    [<DataContract>]
    type ODataResponse<'a> =
        { 
            [<DataMember(Name = "@odata.context")>]
            MetaDataUri : Uri;
            [<DataMember(Name = "value")>]
            Value : 'a
      
        }    



    /// Creates HttpClient containing login container
    let loginAsync(loginUri:string) (user:string) (password:string) =
        async {
            let requestHandler = new WebRequestHandler();
            requestHandler.UseCookies <- true;
            let client = new HttpClient(requestHandler)
            client.BaseAddress <- new Uri(loginUri)
            let loginData = [ new KeyValuePair<string, string>("UserName", user); new KeyValuePair<string, string>("Password", password)  ]
            let content = new FormUrlEncodedContent(loginData);
            let! response = client.PostAsync("/MzRemoteApi/Login", content) |> Async.AwaitTask
            response.EnsureSuccessStatusCode() |> ignore
            return client
        }


    let getAsync (httpClient:HttpClient) (url:string) = 
        async {            
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode() |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }


    
    let getODataResponseAsync<'a> (httpClient:HttpClient) (url:string) = 
        async {            
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode() |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            let resp = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResponse<'a>>(content)
            return resp
        }

