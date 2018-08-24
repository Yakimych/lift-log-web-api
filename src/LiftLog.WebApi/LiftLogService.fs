[<RequireQualifiedAccess>]
module LiftLog.Service

open Microsoft.Extensions.Configuration
open MongoDB.Driver
open LiftLog.Models
open LiftLog.MongoDbModels
open LiftLog.MongoDbSettings
open Mapping

let getCollection<'a> (mongoDbSettings: Settings) =
    let client = new MongoClient(mongoDbSettings.mondoDbServer)
    let database = client.GetDatabase(mongoDbSettings.databaseName)
    
    database.GetCollection<'a>(mongoDbSettings.collectionName)
    
let addLog (logCreateModel: LogCreateModel) (mongoDbSettings: Settings): Result<unit, string> =
    let lowerCaseName = logCreateModel.Name.ToLowerInvariant()
    let collection = getCollection<LiftLogObject>(mongoDbSettings)
    
    let logWithSameNameExists = collection.Find(fun l -> l.Name = lowerCaseName).Any()
    if not logWithSameNameExists then
        collection.InsertOne({ logCreateModel with Name = lowerCaseName } |> toMongoDbObject)
        Ok ()
    else
        Error (sprintf "Lift Log with name '%s' already exists" lowerCaseName)
    
let getByName (logName: string) (mongoDbSettings: Settings): LiftLog option =
    let lowerCaseName = logName.ToLowerInvariant()
    let collection = getCollection<LiftLogObject>(mongoDbSettings)
    let foundObjects = collection.Find(fun l -> l.Name = lowerCaseName).ToList()
    
    match foundObjects |> List.ofSeq with
    | [liftLogObject] -> Some (liftLogObject |> toLiftLog)
    | _ -> None
    
let addEntry (logName: string) (logEntry: LiftLogEntry) (mongoDbSettings: Settings): Result<unit, string> =
    let lowerCaseName = logName.ToLowerInvariant()
    let logEntryObject = logEntry |> toLiftLogEntryObject
    let collection = getCollection<LiftLogObject>(mongoDbSettings)
    
    let filter = Builders<LiftLogObject>.Filter.Eq((fun l -> l.Name), lowerCaseName)
    let update = Builders<LiftLogObject>.Update.Push((fun e -> e.Entries), logEntryObject)
    
    try
        collection.FindOneAndUpdate(filter, update) |> ignore
        Ok ()
    with
        | ex -> Error ex.Message
