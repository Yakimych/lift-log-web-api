namespace LiftLog.WebApi.Controllers

open System.Net
open LiftLog
open Microsoft.AspNetCore.Mvc
open LiftLog.Models

[<Route("api/[controller]")>]
[<ApiController>]
type LiftLogsController () =
    inherit ControllerBase()

    let duplicateErrorMessage (logName: string) =
        sprintf "LiftLog with name '%s' already exists" logName
        
    let duplicateLogEntryErrorMessage (logEntry: LiftLogEntry) =
        sprintf "Entry for date %s with weight %M already exists" (logEntry.Date.Date.ToShortDateString()) logEntry.WeightLifted
        
    [<HttpPost>]
    member this.AddLiftLog([<FromBody>] logCreateModel: LogCreateModel): ActionResult =
        let logCreateResult = LiftLog.Service.addLog logCreateModel
        match logCreateResult with
        | Ok _ -> this.StatusCode((int)HttpStatusCode.Created, logCreateResult) :> _
        | Error Service.AddError.DuplicateEntry -> this.BadRequest(duplicateErrorMessage logCreateModel.Name) :> _
        
    [<HttpGet; Route("{logName}")>]
    member this.Get (logName: string): ActionResult =
        match LiftLog.Service.getByName logName with
        | Some liftLog -> this.Ok(liftLog) :> _
        | None -> this.NotFound() :> _

    [<HttpPost; Route("{logName}/Lifts")>]
    member this.AddLift (logName: string) ([<FromBody>] logEntry: LiftLogEntry): ActionResult =
        let addResult = LiftLog.Service.addEntry logName logEntry
        match addResult with
        | Ok _ -> this.StatusCode((int)HttpStatusCode.Created) :> _
        | Error Service.AddError.DuplicateEntry -> this.BadRequest(duplicateLogEntryErrorMessage logEntry) :> _
