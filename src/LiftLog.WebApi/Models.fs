module LiftLog.Models

open System

[<Measure>]
type kg

type Set = {
    NumberOfReps: int
    Rpe: decimal option
}

type LiftLogEntry = {
    Name: string
    Date: DateTime 
    WeightLifted: decimal<kg>
    Sets: Set seq
}

type LiftLog = {
    Title: string
    Name: string
    Entries: LiftLogEntry seq
}

type LogCreateModel = {
    Name: string
    Title: string
}

type LogCreateResult = {
    Id: string
}
