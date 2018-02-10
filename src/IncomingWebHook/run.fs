namespace WhoseTurnIsIt

open Newtonsoft.Json
open System.Text
module IncomingWebHook =

    open System.Net
    open System.Net.Http
    open Microsoft.Azure.WebJobs.Host
    open Model

    let jsonResponse response =
        let httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        httpResponse.Content <- new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
        httpResponse

    let Run(req: HttpRequestMessage, log: TraceWriter) =
        async {
            try
                let! json = req.Content.ReadAsStringAsync() |> Async.AwaitTask
                let request = JsonConvert.DeserializeObject<WhoPickedWhatRequest>(json)

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

