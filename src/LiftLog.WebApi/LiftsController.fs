namespace LiftLog.WebApi.Controllers

open System.Net
open LiftLog
open Microsoft.AspNetCore.Mvc
open LiftLog.Models

[<Route("api/[controller]")>]
[<ApiController>]
type LiftsController () =
    inherit ControllerBase()

    let duplicateErrorMessage (logEntry: LiftLogEntry) =
        sprintf "Entry for date %s with weight %M already exists" (logEntry.Date.Date.ToShortDateString()) logEntry.Weigth
        
    [<HttpGet>]
    member this.Get() =
        let liftLog = LiftLog.Service.getAllEntries ()
        this.Ok(liftLog)

    [<HttpPost>]
    member this.Post([<FromBody>] logEntry: LiftLogEntry): ActionResult =
        let addResult = LiftLog.Service.addEntry logEntry
        match addResult with
        | Ok _ -> this.StatusCode((int)HttpStatusCode.Created) :> _
        | Error Service.EntryAddError.DuplicateEntry -> this.BadRequest(duplicateErrorMessage logEntry) :> _
