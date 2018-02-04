namespace WhoseTurnIsIt

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host

type Name = {
    First: string
    Last: string
}

type Greeting = {
    Greeting: string
}

module IncomingWebHook =
    let Run(req: HttpRequest, log: TraceWriter) =
        async {
            log.Info("Webhook was triggered!")
            let content = req.Body
            log.Info (sprintf "%A" content)
            // let! jsonContent = req.Body.ToString() |> Async.AwaitTask
            // log.Info(sprintf "Content: %s" jsonContent)

            // try
            //     let name = JsonConvert.DeserializeObject<Name> jsonContent
            //     let greeting = sprintf "Hello %s %s!" name.First name.Last
            //     log.Info greeting
            //     ContentResult "hi"// JsonConvert.SerializeObject { Greeting = greeting })
            // with ex ->
            //     log.Error ex.Message
            //     return req.CreateResponse (HttpStatusCode.BadRequest)
        } |> Async.StartAsTask
