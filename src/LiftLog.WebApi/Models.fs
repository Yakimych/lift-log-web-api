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
    Entries: LiftLogEntry list
}