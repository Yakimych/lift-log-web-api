module LiftLog.Models

open System

[<Measure>]
type kg

type Rep = {
    Number: int
    // TODO: Make optional and fix serialization (OptionConverter)
    Rpe: decimal
}

type LiftLogEntry = {
    Name: string
    Date: DateTime 
    Weigth: decimal<kg>
    Reps: Rep list
}

type LiftLog = {
    Name: string
    mutable Entries: LiftLogEntry list
}

type Board = {
    Id: string
    Name: string
    LiftLog: LiftLog
}

type BoardCreateModel = {
    Name: string
    PreferredBoardId: string
}

type BoardCreateResult = {
    CreatedBoardId: string
}
