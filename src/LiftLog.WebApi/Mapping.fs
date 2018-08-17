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

let toRep (repObject: RepObject): Rep =
    { Number = repObject.Number;
      Rpe = repObject.Rpe |> nullableToOption }
    
let toRepObject (rep: Rep): RepObject =
    { Number = rep.Number;
     Rpe = rep.Rpe |> optionToNullable }
    
let toLiftLogEntry (liftLogEntryObject: LiftLogEntryObject): LiftLogEntry =
    { Name = liftLogEntryObject.Name;
      Date = liftLogEntryObject.Date;
      WeightLifted = liftLogEntryObject.WeightLifted;
      Reps = (liftLogEntryObject.Reps) |> Seq.map toRep }
    
let toLiftLogEntryObject (liftLogEntry: LiftLogEntry): LiftLogEntryObject =
    { Name = liftLogEntry.Name;
      Date = liftLogEntry.Date;
      WeightLifted = liftLogEntry.WeightLifted;
      Reps = (liftLogEntry.Reps) |> Seq.map toRepObject }
    
let toLiftLog (liftLogObject: LiftLogObject) =
    { Name = liftLogObject.Name;
      Title = liftLogObject.Title;
      Entries = liftLogObject.Entries |> Seq.map toLiftLogEntry }
