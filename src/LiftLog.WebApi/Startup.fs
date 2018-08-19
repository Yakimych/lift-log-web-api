namespace LiftLog.WebApi

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Swashbuckle.AspNetCore.Swagger
open LiftLog.WebApi.Converters
open Microsoft.AspNetCore.HttpOverrides

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration
        
    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.
            AddMvc().
            AddJsonOptions(fun options -> options.SerializerSettings.Converters.Add(new OptionConverter())).
            SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            |> ignore
        services.AddSwaggerGen(fun c -> c.SwaggerDoc("v1", new Info(Title = "Lifts API", Version = "v1"))) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app.UseForwardedHeaders(
                ForwardedHeadersOptions(
                    ForwardedHeaders = (ForwardedHeaders.XForwardedFor ||| ForwardedHeaders.XForwardedProto)))
            |> ignore
        
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseHsts() |> ignore
            
        app.UseSwagger() |> ignore
        app.UseSwaggerUI(fun c -> c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lifts API V1")) |> ignore

        app.UseCors(fun builder -> builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() |> ignore) |> ignore
        app.UseFileServer() |> ignore
        app.UseMvc() |> ignore

    member val Configuration : IConfiguration = null with get, set