using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Diagnostics;
using DAL004;
using System.Web.Helpers;

namespace ASPA005_2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using (IRepository repository = Repository.Create("Celebrities"))
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.ConfigureHttpJsonOptions(options =>
                {
                    options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
                });
                builder.Services.AddScoped<IRepository>(sp => Repository.Create("Celebrities"));
                var app = builder.Build();
                var api = app.MapGroup("/Celebrities");

                app.UseExceptionHandler("/Celebrities/Error");

                api.MapGet("/", () => repository.getAllCelebrities());

                api.MapGet("/{id:int}", (int id) =>
                {
                    Celebrity? celebrity = repository.GetCelebrityById(id);
                    if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
                    return celebrity;
                });

                api.MapPost("/", (Celebrity? celebrity) =>
                {
                    int? id = repository.addCelebrity(celebrity);
                    if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
                    if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                    return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
                })
                    .AddEndpointFilter<SurnameFilter>()
                    .AddEndpointFilter<PhotoExistFilter>();

                api.MapDelete("/{id:int}", (int id) =>
                {
                    repository.delCelebrityById(id);
                    if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                    return "Celebrity with Id = {id} deleted";
                }).AddEndpointFilter<DeleteFilter>();

                api.MapPut("/{id:int}", (int id, Celebrity? celebrity) =>
                {
                    var i = repository.updCelebrityById(id, celebrity);
                    if (i == null) throw new UpdCelebrityException($"Celebrity Id = {id} not found or Celebrity == null", "/Celebrities");
                    if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                    return new Celebrity((int)i, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
                }).AddEndpointFilter<PutFilter>();

                app.MapFallback((HttpContext ctx) =>
                    Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

                api.Map("/Error", (HttpContext ctx) =>
                {
                    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                    IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
                    if (ex != null)
                    {
                        if (ex is DelCelebrityException)
                            rc = Results.NotFound(ex.Message);
                        if (ex is UpdCelebrityException)
                            rc = Results.NotFound(ex.Message);
                        if (ex is FoundByIdException)
                            rc = Results.NotFound(ex.Message);
                        if (ex is BadHttpRequestException)
                            rc = Results.BadRequest(ex.Message);
                        if (ex is ValueErrorException)
                            rc = Results.Conflict(ex.Message);
                        if (ex is SaveException)
                            rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                        if (ex is AddCelebrityException)
                            rc = Results.Problem(title: "ASPA004/AddCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    }
                    return rc;
                });

                app.Run();
            }
        }
    }

    public class FoundByIdException(string message) : Exception($"Found by Id: {message}");
    public class SaveException(string message) : Exception($"SaveChanges error: {message}");
    public class AddCelebrityException(string message) : Exception($"AddCelebrityException error: {message}");
    public class ValueErrorException(string message) : Exception($"Value:{message}");
    public class DelCelebrityException(string message, string path) : Exception($"Delete by Id:DELETE {path} error: {message}");
    public class UpdCelebrityException(string message, string path) : Exception($"Update by Id: UPDATE {path} error:  {message}");
}