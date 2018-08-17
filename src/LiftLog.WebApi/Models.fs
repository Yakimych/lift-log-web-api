module LiftLog.Models

open System

[<Measure>]
type kg

type Rep = {
    Number: int
    Rpe: decimal option
}

type LiftLogEntry = {
    Name: string
    Date: DateTime 
    WeightLifted: decimal<kg>
    Reps: Rep list
}

type LiftLog = {
    Title: string
    Name: string
    Entries: LiftLogEntry seq
}

// API only
type LogCreateModel = {
    Name: string
    Title: string
}

type LogCreateResult = {
    Id: string
}
