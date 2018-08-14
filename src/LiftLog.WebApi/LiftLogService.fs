[<RequireQualifiedAccess>]
module LiftLog.Service

open LiftLog.Models

type EntryAddError = DuplicateEntry

let private isDuplicate (otherEntry: LiftLogEntry) (liftEntry: LiftLogEntry) =
    otherEntry.Name = liftEntry.Name &&
    otherEntry.Date.Date = liftEntry.Date.Date &&
    otherEntry.Weigth = liftEntry.Weigth

let addEntry (boardId: string) (liftEntry: LiftLogEntry) =
    let board = BoardService.getById boardId
    let byDuplicate = liftEntry |> isDuplicate
    let duplicateEntries = board.LiftLog.Entries |> List.filter byDuplicate
    match duplicateEntries with
    | [] ->
        board.LiftLog.Entries  <- List.append board.LiftLog.Entries [liftEntry]
        Ok ()
    | _ -> Error DuplicateEntry
    
let getAllEntries (boardId: string): LiftLog =
   let board = BoardService.getAll () |> List.find (fun b -> b.Id = boardId)
   { Name = board.LiftLog.Name; Entries = board.LiftLog.Entries }
