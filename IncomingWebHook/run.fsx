#if INTERACTIVE

#I @"C:\Program Files\nodejs\node_modules\azure-functions-core-tools\bin\"

#r "Microsoft.Azure.Webjobs.Host.dll"
#r "System.Net.Http.Formatting.dll"
#r "System.Web.Http.dll"
#r "System.Net.Http.dll"
#r "Newtonsoft.Json.dll"

#else

#r "System.Net.Http"
#r "Newtonsoft.Json"

#endif

open System.Net
open System.Net.Http
open Newtonsoft.Json
open Microsoft.Azure.WebJobs.Host

type Name = {
    First: string
    Last: string
}

type Greeting = {
    Greeting: string
}

let Run(req: HttpRequestMessage, log: TraceWriter) =
    async {
        log.Info("Webhook was triggered yay!")
        let! jsonContent = req.Content.ReadAsStringAsync() |> Async.AwaitTask
        log.Info jsonContent
        try
            let name = JsonConvert.DeserializeObject<Name>(jsonContent)
            log.Info (sprintf "%A" name)
            let greeting = { Greeting = (sprintf "Hello %s %s!" name.First name.Last ) }
            log.Info (sprintf "%A" greeting)
            return req.CreateResponse(HttpStatusCode.OK, greeting)
        with ex ->
            log.Error (ex.Message)
            return req.CreateResponse(HttpStatusCode.BadRequest)
    } |> Async.StartAsTask
