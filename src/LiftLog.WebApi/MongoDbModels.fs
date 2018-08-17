module LiftLog.MongoDbModels

open System
open Models
open MongoDB.Bson

type RepObject = {
    Number: int
    Rpe: Nullable<decimal>
}

type LiftLogEntryObject = {
    Name: string
    Date: DateTime 
    WeightLifted: decimal<kg>
    Reps: RepObject seq
}

type LiftLogObject = {
    Id: ObjectId
    Title: string
    Name: string
    Entries: LiftLogEntryObject seq
}
