using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Diagnostics;
using DAL004;

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
            var app = builder.Build();
            app.UseExceptionHandler("/Celebrities/Error");

            app.MapGet("/Celebrities", () => repository.getAllCelebrities());

            app.MapGet("/Celebrities/{id:int}", (int id) =>
            {
                Celebrity? celebrity = repository.GetCelebrityById(id);
                if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
                return celebrity;
            });

            app.MapPost("/Celebrities", (Celebrity? celebrity) =>
            {
                int? id = repository.addCelebrity(celebrity);
                if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
                if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
            });

            app.MapDelete("/Celebrities/{id:int}", (int id) =>
            {
                if (!repository.delCelebrityById(id)) throw new DelCelebrityException($"Celebrity Id = {id} not found");
                return "Celebrity with Id = {id} deleted";

            });

            app.MapFallback((HttpContext ctx) =>
                Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

            app.Map("/Celebrities/Error", (HttpContext ctx) => {
                Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
                if (ex != null)
                {
                    if (ex is DelCelebrityException)
                        rc = Results.NotFound(ex.Message);
                    if (ex is FileNotFoundException)
                        rc = Results.Problem(title: "ASPA004", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    if (ex is FoundByIdException)
                        rc = Results.NotFound(ex.Message);
                    if (ex is BadHttpRequestException)
                        rc = Results.BadRequest(ex.Message);
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

    public class FoundByIdException(string message) : Exception($"Found by Id: {message}");
    public class SaveException(string message) : Exception($"SaveChanges error: {message}");
    public class AddCelebrityException(string message) : Exception($"AddCelebrityException error: {message}");
    public class DelCelebrityException(string message) : Exception($"Delete by Id:DELETE error: {message}");
}