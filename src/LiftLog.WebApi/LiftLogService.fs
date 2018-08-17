[<RequireQualifiedAccess>]
module LiftLog.Service

open MongoDB.Driver
open LiftLog.Models
open LiftLog.MongoDbModels
open Mapping

// TODO: Read from configuration
let mongoDbServerUrl = "mongodb://localhost:27017"
let databaseName = "test"
let collectionName = "fsharpliftlogcollection"

let getCollection<'a> () =
    let client = new MongoClient(mongoDbServerUrl)
    let database = client.GetDatabase(databaseName)
    
    database.GetCollection<'a>(collectionName)

let addLog (logCreateModel: LogCreateModel): Result<unit, string> =
    let collection = getCollection<LiftLogObject>()
    
    let logWithSameNameExists = collection.Find(fun l -> l.Name = logCreateModel.Name).Any()
    if not logWithSameNameExists then
        collection.InsertOne(logCreateModel |> toMongoDbObject)
        Ok ()
    else
        Error (sprintf "Lift Log with name '%s' already exists" logCreateModel.Name)
    
let getByName (logName: string): LiftLog option =
    let collection = getCollection<LiftLogObject>()
    let foundObjects = collection.Find(fun l -> l.Name = logName).ToList()
    
    match foundObjects |> List.ofSeq with
    | [liftLogObject] -> Some (liftLogObject |> toLiftLog)
    | _ -> None
    
let addEntry (logName: string) (logEntry: LiftLogEntry): Result<unit, string> =
    let logEntryObject = logEntry |> toLiftLogEntryObject
    let collection = getCollection<LiftLogObject>()
    
    let filter = Builders<LiftLogObject>.Filter.Eq((fun l -> l.Name), logName)
    let update = Builders<LiftLogObject>.Update.Push((fun e -> e.Entries), logEntryObject)
    
    try
        collection.FindOneAndUpdate(filter, update) |> ignore
        Ok ()
    with
        | ex -> Error ex.Message
