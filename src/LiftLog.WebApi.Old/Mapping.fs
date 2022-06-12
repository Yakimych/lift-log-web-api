module Mapping

open LiftLog
open Models
open MongoDbModels
open UtilFunctions
open MongoDB.Bson

let toMongoDbObject (logCreateModel: LogCreateModel) =
    { Id = ObjectId.GenerateNewId();
      Name = logCreateModel.Name;
      Title = logCreateModel.Title; Entries = [] }

let toSet (setObject: SetObject): Set =
    { NumberOfReps = setObject.NumberOfReps;
      Rpe = setObject.Rpe |> nullableToOption }
    
let toSetObject (set: Set): SetObject =
    { NumberOfReps = set.NumberOfReps;
     Rpe = set.Rpe |> optionToNullable }
    
let toLiftLogEntry (liftLogEntryObject: LiftLogEntryObject): LiftLogEntry =
    { Name = liftLogEntryObject.Name;
      Date = liftLogEntryObject.Date;
      WeightLifted = liftLogEntryObject.WeightLifted;
      Sets = (liftLogEntryObject.Sets) |> Seq.map toSet
      Comment = liftLogEntryObject.Comment
      Links = liftLogEntryObject.Links }
    
let toLiftLogEntryObject (liftLogEntry: LiftLogEntry): LiftLogEntryObject =
    { Name = liftLogEntry.Name;
      Date = liftLogEntry.Date;
      WeightLifted = liftLogEntry.WeightLifted;
      Sets = (liftLogEntry.Sets) |> Seq.map toSetObject
      Comment = liftLogEntry.Comment
      Links = liftLogEntry.Links }
    
let toLiftLog (liftLogObject: LiftLogObject) =
    { Name = liftLogObject.Name;
      Title = liftLogObject.Title;
      Entries = liftLogObject.Entries |> Seq.map toLiftLogEntry }
