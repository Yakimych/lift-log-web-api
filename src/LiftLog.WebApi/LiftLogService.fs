[<RequireQualifiedAccess>]
module LiftLog.Service

open LiftLog.Models
open LiftLog.MongoDbModels

type AddError = DuplicateEntry

let addLog (logCreateModel: LogCreateModel): Result<unit, AddError> =
    Ok ()
    
let getByName (logName: string): LiftLog option =
    let bobRep = { Number = 5; Rpe = None }
    let bobEntry = { Name = "Bob"; Date = System.DateTime.Now; WeightLifted = 82m<kg>; Reps = [bobRep; bobRep; { bobRep with Rpe = Some 9.5m } ]}
    Some { Name = "AnturaPD"; Title = "Bench Press: Road to 100"; Entries = [bobEntry] }
    
let addEntry (logName: string) (logEntry: LiftLogEntry): Result<unit, AddError> =
    Ok ()
