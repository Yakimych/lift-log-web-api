[<RequireQualifiedAccess>]
module LiftLog.BoardService

open LiftLog.Models

let mutable private inMemoryBoards: Board list = []

let private isDuplicate (otherBoard: Board) (board: Board) =
    otherBoard.Id = board.Id

let private getNewBoardId (existingBoards: Board list) (preferredBoardId: string) =
    let duplicateEntries = existingBoards |> List.filter (fun b -> b.Id = preferredBoardId)
    match duplicateEntries with
    | [] -> preferredBoardId
    | _ -> System.Guid.NewGuid().ToString()

let addBoard (boardCreateModel: BoardCreateModel) =
    let newBoardId = getNewBoardId inMemoryBoards boardCreateModel.PreferredBoardId
    let newBoard: Board = { Id = newBoardId; Name = boardCreateModel.Name; LiftLog = { Name = boardCreateModel.Name; Entries = [] } }
    
    inMemoryBoards <- List.append inMemoryBoards [newBoard]
    { CreatedBoardId = newBoardId }
    
let getById (boardId: string): Board =
    // TODO: Error handling
    inMemoryBoards |> List.find (fun b -> b.Id = boardId)
    
let getAll (): Board list = inMemoryBoards
