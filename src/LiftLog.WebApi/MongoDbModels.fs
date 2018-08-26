module LiftLog.MongoDbModels

open System
open Models
open MongoDB.Bson

type SetObject = {
    NumberOfReps: int
    Rpe: Nullable<decimal>
}

type LiftLogEntryObject = {
    Name: string
    Date: DateTime 
    WeightLifted: decimal<kg>
    Sets: SetObject seq
}

type LiftLogObject = {
    Id: ObjectId
    Title: string
    Name: string
    Entries: LiftLogEntryObject seq
}
