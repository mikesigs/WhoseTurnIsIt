namespace WhoseTurnIsIt

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open System.IO
open Newtonsoft.Json
open System.Web.Http
open System.Threading.Tasks

type Name = {
    First: string
    Last: string
}

type Greeting = {
    Greeting: string
}

module HttpHelpers =
    let toJson (stream: System.IO.Stream) =
        async {
            use reader = new StreamReader(stream)
            return! reader.ReadToEndAsync() |> Async.AwaitTask
        }

module IncomingWebHook =
    let Run(req: HttpRequest, log: TraceWriter) =
        async {
            log.Info("Webhook was triggered!")
            let! jsonContent = req.Body |> HttpHelpers.toJson
            log.Info (sprintf "%s" jsonContent)

            try
                let name = JsonConvert.DeserializeObject<Name> jsonContent
                let greeting = sprintf "Hello %s %s!" name.First name.Last
                log.Info greeting
                return ContentResult(Content = JsonConvert.SerializeObject { Greeting = greeting }, ContentType = "application/json") :> IActionResult
            with ex ->
                log.Error (ex.ToString())
                return BadRequestErrorMessageResult(ex.Message) :> IActionResult
        }
        |> Async.StartAsTask
