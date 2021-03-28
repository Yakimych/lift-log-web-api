open System
open Farmer
open Farmer.Builders

let commandLineArgs = Environment.GetCommandLineArgs()

match commandLineArgs |> List.ofArray with
| [] | [_] -> Console.WriteLine("No command line argument specified.")
| _applicationName :: publishedAppFolder :: _ ->

    let resourceGroupName = "LiftLogTestGroup"
    let webApplicationName = "LiftLogTestWebApplication"

    let myWebApp = webApp {
        name webApplicationName
        zip_deploy publishedAppFolder
        app_insights_off
    }

    let deployment = arm {
        location Location.NorthEurope
        add_resource myWebApp
    }

    deployment
    |> Deploy.execute resourceGroupName Deploy.NoParameters
    |> ignore
| _ -> Console.WriteLine("There should be exactly 1 command line argument specified: publishedAppFolder.")
