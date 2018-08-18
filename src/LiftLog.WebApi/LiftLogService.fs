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
    let lowerCaseName = logCreateModel.Name.ToLowerInvariant()
    let collection = getCollection<LiftLogObject>()
    
    let logWithSameNameExists = collection.Find(fun l -> l.Name = lowerCaseName).Any()
    if not logWithSameNameExists then
        collection.InsertOne({ logCreateModel with Name = lowerCaseName } |> toMongoDbObject)
        Ok ()
    else
        Error (sprintf "Lift Log with name '%s' already exists" lowerCaseName)
    
let getByName (logName: string): LiftLog option =
    let lowerCaseName = logName.ToLowerInvariant()
    let collection = getCollection<LiftLogObject>()
    let foundObjects = collection.Find(fun l -> l.Name = lowerCaseName).ToList()
    
    match foundObjects |> List.ofSeq with
    | [liftLogObject] -> Some (liftLogObject |> toLiftLog)
    | _ -> None
    
let addEntry (logName: string) (logEntry: LiftLogEntry): Result<unit, string> =
    let lowerCaseName = logName.ToLowerInvariant()
    let logEntryObject = logEntry |> toLiftLogEntryObject
    let collection = getCollection<LiftLogObject>()
    
    let filter = Builders<LiftLogObject>.Filter.Eq((fun l -> l.Name), lowerCaseName)
    let update = Builders<LiftLogObject>.Update.Push((fun e -> e.Entries), logEntryObject)
    
    try
        collection.FindOneAndUpdate(filter, update) |> ignore
        Ok ()
    with
        | ex -> Error ex.Message
