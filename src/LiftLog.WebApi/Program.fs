namespace LiftLog.WebApi
#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Serilog
open Serilog.Debugging
open Serilog.Events

module Program =
    [<Literal>]
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Host.UseSerilog()
        Log.Logger <- LoggerConfiguration()
                         .ReadFrom.Configuration(builder.Configuration)
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                         .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                         .CreateLogger()
        SelfLog.Enable(System.Console.Error)

        builder.Services.AddControllers()
        builder.Services.AddCors()
        builder.Services.AddSwaggerGen()

        let app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()
        app.UseHttpsRedirection()

        let frontendHost = builder.Configuration["FrontendHost"]
        app.UseCors(fun x -> x.SetIsOriginAllowed(fun o -> o = frontendHost) |> ignore)
        app.UseAuthorization()
        app.MapControllers()

        let appVersion = builder.Configuration["AppVersion"]
        try
            Log.Information("Starting web host. AppVersion: {AppVersion}", appVersion)
            
            app.Run()
            exitCode
        with ex ->
            Log.Fatal(ex, "Host terminated unexpectedly. AppVersion: {AppVersion}", appVersion)
            -1
