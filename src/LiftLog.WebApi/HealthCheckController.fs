namespace LiftLog.WebApi.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
[<ApiController>]
type HealthCheckController () =
    inherit ControllerBase()

    [<HttpGet>]
    member this.HealthCheck(): ActionResult =
        this.Ok ()
