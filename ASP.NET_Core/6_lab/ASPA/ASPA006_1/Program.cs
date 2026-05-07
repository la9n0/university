using System.Text.Json.Serialization.Metadata;
using DAL_Celebrity;
using DAL_Celebrity_MSSQL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace ASPA006_1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("Celebrities.config.json");
            builder.Services.Configure<CelebritiesConfig>(
                builder.Configuration.GetSection("CelebritiesConfig"));

            builder.Services.AddScoped<IRepository>(p => {
                var config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
                return new Repository(config.ConnectionString);
            });
            
            var app = builder.Build();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    ctx.Response.ContentType = "application/problem+json";
                    var problem = Results.Problem(
                        title: "Exception",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        instance: ctx.TraceIdentifier
                    );
                    await problem.ExecuteAsync(ctx);
                }
            });
            
            var celebrities = app.MapGroup("/api/Celebrities");
            celebrities.MapGet("/", (IRepository repo) => repo.GetAllCelebrities());
            
            celebrities.MapGet("{id:int:min(1)}", (IRepository repo, int id) =>
            {
                var celebrity = repo.GetCelebrityById(id); 
                if(celebrity == null) return Results.NotFound($"Celebrity id={id} not found");
                return Results.Ok(celebrity);
            });
            
            celebrities.MapGet("/Lifeevents/{id:int:min(1)}", (IRepository repo, int id) =>
            {
                var celebrity = repo.GetCelebrityByLifeeventId(id); 
                if(celebrity == null) return Results.NotFound($"Celebrity id={id} not found");
                return Results.Ok(celebrity);
            });
            
            celebrities.MapDelete("{id:int:min(1)}", (IRepository repo, int id) => repo.DelCelebrity(id)? 
                Results.Ok($"Celebrity id={id} deleted") : Results.NotFound($"Celebrity id={id} not found"));
            
            celebrities.MapPost("/", (IRepository repo, Celebrity celebrity) => repo.AddCelebrity(celebrity)? 
                Results.Ok($"Celebrity id={celebrity.Id} added") : Results.BadRequest("Celebrity details entered are incorrect"));
            
            celebrities.MapPut("{id:int:min(1)}", (IRepository repo, int id, Celebrity celebrity) =>
            {
                var cel = repo.GetCelebrityById(id); 
                if(cel == null) return Results.NotFound($"Celebrity id={id} not found");
                return repo.UpdCelebrity(id, celebrity)
                    ? Results.Ok($"Celebrity id={id} updated")
                    : Results.BadRequest($"The celebrity details entered are incorrect");
            });
            
            celebrities.MapGet("/photo/{fname}", (string fname, IOptions<CelebritiesConfig> config, IWebHostEnvironment context) => {
                var path = Path.Combine(config.Value.PhotoDirectory, fname);
                if (File.Exists(path)) return Results.File(path, "image/jpg");

                var fallback = Path.Combine(context.WebRootPath, "no-photo.svg");
                return File.Exists(fallback)
                    ? Results.File(fallback, "image/svg+xml")
                    : Results.NotFound(new { detail = $"Photo not found: {fname}" });
            });

            var lifeevents = app.MapGroup("/api/Lifeevents");
            lifeevents.MapGet("/", (IRepository repo) => repo.GetAllLifeevents());
            
            lifeevents.MapGet("{id:int:min(1)}", (IRepository repo, int id) =>
            {
                var lifeevent = repo.GetLifeeventById(id); 
                if(lifeevent == null) return Results.NotFound($"Life event id={id} not found");
                return Results.Ok(lifeevent);
            });
            
            lifeevents.MapGet("/Celebrities/{id:int:min(1)}", (IRepository repo, int id) =>
            {
                if(repo.GetCelebrityById(id) == null)
                    return Results.NotFound($"Life event id={id} not found");
                return Results.Ok(repo.GetLifeeventsByCelebrityId(id));
            });
            
            lifeevents.MapDelete("{id:int:min(1)}", (IRepository repo, int id) => repo.DelCelebrity(id)? 
                Results.Ok($"Life event id={id} deleted") : Results.NotFound($"Life event id={id} not found"));
            
            lifeevents.MapPost("/", (IRepository repo, Lifeevent lifeevent) =>repo.AddLifeevent(lifeevent)? 
                Results.Ok($"Life event id={lifeevent.Id} added") : Results.BadRequest("Life event details entered are incorrect"));
            
            lifeevents.MapPut("{id:int:min(1)}", (IRepository repo, int id, Lifeevent lifeevent) =>
            {
                var lif = repo.GetCelebrityById(id);
                if(lif == null) return Results.NotFound($"Life event id={id} not found");
                return repo.UpdLifeevent(id, lifeevent)
                    ? Results.Ok($"Life event id={id} updated")
                    : Results.BadRequest($"Life event details entered are incorrect");
            });
            app.Run();
        }
    }
}