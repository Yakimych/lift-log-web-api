namespace LiftLog.WebApi.Controllers

open System.Net
open LiftLog
open Microsoft.AspNetCore.Mvc
open LiftLog.Models

[<Route("api/[controller]")>]
[<ApiController>]
type BoardsController () =
    inherit ControllerBase()

    let duplicateErrorMessage (preferredBoardId: string) =
        sprintf "Board with id %s already exists" preferredBoardId
        
    [<HttpPost>]
    member this.Post([<FromBody>] boardCreateModel: BoardCreateModel) =
        let boardCreateResult = LiftLog.BoardService.addBoard boardCreateModel
        this.StatusCode((int)HttpStatusCode.Created, boardCreateResult)
