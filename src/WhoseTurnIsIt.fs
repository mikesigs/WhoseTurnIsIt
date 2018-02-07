namespace WhoseTurnIsIt
open FSharp.Data

type WhoseTurnIsIt() = 
    member this.X = "F#"

type JsonTest = JsonProvider<"./sample.json">

