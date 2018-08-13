module LiftLog.WebApi.Domain

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
    Weigth: decimal<kg>
    Reps: Rep list
}