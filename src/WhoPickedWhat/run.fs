namespace WhoseTurnIsIt

open System
open System.Net
open System.Net.Http
open System.Text
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Newtonsoft.Json

module WhoPickedWhat =

    open Model
    
    let jsonResponse response =
        let httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        httpResponse.Content <- new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
        httpResponse

    type Pick = {        
        [<JsonIgnore>]
        PartitionKey: string;
        [<JsonIgnore>]
        RowKey: string;
        whoPicked: string;
        thingType: string;
        thingDescription: string;
    }
    let Run(req: HttpRequestMessage, pickDocument: IAsyncCollector<Pick>, log: TraceWriter) =
        async {
            try
                let! json = req.Content.ReadAsStringAsync() |> Async.AwaitTask
                let request = JsonConvert.DeserializeObject<WhoPickedWhatRequest>(json)

                do! 
                    pickDocument.AddAsync(
                        { PartitionKey = "Pick"
                          RowKey = Guid.NewGuid().ToString("N")
                          whoPicked = request.result.parameters.whoPicked
                          thingType = request.result.parameters.thingType
                          thingDescription = request.result.parameters.thingDescription }) |> Async.AwaitTask
                      
                log.Info (sprintf "Request: %A" request)

                let message = (sprintf "Thanks %s! I'll remember that you picked the %s last time." request.result.parameters.whoPicked request.result.parameters.thingType )
                let response = {
                    speech = message;
                    displayText = message;
                    source = "webhook" }
                log.Info (sprintf "Response: %A" response)

                return jsonResponse response
            with ex ->
                log.Error (ex.ToString())
                return req.CreateResponse(HttpStatusCode.BadRequest, ex.Message)
        } |> Async.StartAsTask

