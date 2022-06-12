module LiftLog.Models

open System
open System.ComponentModel.DataAnnotations
open LiftLog.Validation

[<Measure>]
type kg

type Set = {
    [<Range(1, 999)>]
    NumberOfReps: int
    [<Rpe>]
    Rpe: decimal option
}

type Link = {
    [<StringLength(20)>]
    Text: string
    [<StringLength(200)>]
    Url: string
}

type LiftLogEntry = {
    [<StringLength(30)>]
    Name: string
    [<Range(0, 999)>]
    WeightLifted: decimal<kg>
    Date: DateTime 
    Sets: Set seq
    [<StringLength(400)>]
    Comment: string
    [<MaxLinks>]
    Links: Link seq
}

type LiftLog = {
    Title: string
    Name: string
    Entries: LiftLogEntry seq
}

type LogCreateModel = {
    [<StringLength(50)>]
    Title: string
    [<StringLength(20, MinimumLength = 2)>]
    Name: string
}

type LogCreateResult = {
    Id: string
}
