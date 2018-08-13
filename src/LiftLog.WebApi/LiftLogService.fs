[<RequireQualifiedAccess>]
module LiftLog.Service

open LiftLog.Models

type EntryAddError = DuplicateEntry

let mutable private inMemoryEntries: LiftLogEntry list = []

let private isDuplicate (otherEntry: LiftLogEntry) (liftEntry: LiftLogEntry) =
    otherEntry.Name = liftEntry.Name &&
    otherEntry.Date.Date = liftEntry.Date.Date &&
    otherEntry.Weigth = liftEntry.Weigth

let addEntry (liftEntry: LiftLogEntry) =
    let byDuplicate = liftEntry |> isDuplicate
    let duplicateEntries = inMemoryEntries |> List.filter byDuplicate
    match duplicateEntries with
    | [] ->
        inMemoryEntries <- List.append inMemoryEntries [liftEntry]
        Ok ()
    | _ -> Error DuplicateEntry
    
let getAllEntries (): LiftLog =
    { Entries = inMemoryEntries }
