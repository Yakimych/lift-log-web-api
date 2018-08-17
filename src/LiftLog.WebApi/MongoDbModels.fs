module LiftLog.MongoDbModels

open System
open Models
open MongoDB.Bson

type LiftLogObject = {
    Id: ObjectId
    Title: string
    Name: string
    Entries: LiftLogEntry seq
}
