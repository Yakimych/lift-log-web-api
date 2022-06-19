namespace LiftLog.WebApi
#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

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

        app.Run()

        exitCode
