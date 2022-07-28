namespace LiftLog.WebApi.Controllers

open LiftLog
open System.Net
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open LiftLog.Models
open Microsoft.Extensions.Logging

[<Route("api/[controller]")>]
[<ApiController>]
type LiftLogsController private (logger: ILogger<LiftLogsController>) =
    inherit ControllerBase()
    new (configuration: IConfiguration, logger: ILogger<LiftLogsController>) as this =
        LiftLogsController(logger) then
        this.Configuration <- configuration

    member private this.getMongoDbSettings () =
        this.Configuration |> MongoDbSettings.toMongoDbSettings

    [<HttpPost>]
    member this.AddLiftLog([<FromBody>] logCreateModel: LogCreateModel): ActionResult =
        let logCreateResult = LiftLog.Service.addLog logCreateModel (this.getMongoDbSettings())
        match logCreateResult with
        | Ok _ -> this.StatusCode((int)HttpStatusCode.Created) :> _
        | Error message -> this.BadRequest(message) :> _

    [<HttpGet>]
    member this.GetAll (): ActionResult =
        logger.LogInformation("GetAll called")
        let allLiftLogs = LiftLog.Service.getAll (this.getMongoDbSettings())
        this.Ok(allLiftLogs) :> _

    [<HttpGet; Route("{logName}")>]
    member this.Get (logName: string): ActionResult =
        match LiftLog.Service.getByName logName (this.getMongoDbSettings()) with
        | Some liftLog -> this.Ok(liftLog) :> _
        | None -> this.NotFound() :> _

    [<HttpPost; Route("{logName}/Lifts")>]
    member this.AddLift (logName: string) ([<FromBody>] logEntry: LiftLogEntry): ActionResult =
        let addResult = LiftLog.Service.addEntry logName logEntry (this.getMongoDbSettings())
        match addResult with
        | Ok _ -> this.StatusCode((int)HttpStatusCode.Created) :> _
        | Error message -> this.BadRequest(message) :> _

    member val Configuration : IConfiguration = null with get, set
