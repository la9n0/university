using Microsoft.AspNetCore.Diagnostics;

namespace ASPA005_3
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseExceptionHandler("/Error");

            var appA = app.MapGroup("/A");
            var appB = app.MapGroup("/B");
            var appC = app.MapGroup("/C");
            var appD = app.MapGroup("/D");
            var appE = app.MapGroup("/E");
            var appF = app.MapGroup("/F");

            appA.MapGet("/{x:int:max(100)}", (int x) =>
                Results.Ok(new { x, y = (int?)null }));

            appA.MapPost("/{x:int:min(0):max(100)}", (int x) =>
                Results.Ok(new { x, y = (int?)null }));

            appA.MapPut("/{x:int:min(1)}/{y:int:min(1)}", (int x, int y) =>
                Results.Ok(new { x, y }));

            appA.MapDelete("/{x:int:min(1)}-{y:int:min(1):max(100)}", (int x, int y) =>
                Results.Ok(new { x, y }));

            appB.MapGet("/{x:float}", (float x) =>
                Results.Ok(new { x, y = (float?)null }));

            appB.MapPost("/{x:float}/{y:float}", (float x, float y) =>
                Results.Ok(new { x, y }));

            appB.MapDelete("/{x:float}-{y:float}", (float x, float y) =>
                Results.Ok(new { x, y }));

            appC.MapGet("/{x:bool}", (bool x) =>
                Results.Ok(new { x, y = (bool?)null }));

            appC.MapPost("/{x:bool},{y:bool}", (bool x, bool y) =>
                Results.Ok(new { x, y }));

            appD.MapGet("/{x:datetime}", (DateTime x) =>
                Results.Ok(new { x, y = (DateTime?)null }));

            appD.MapPost("/{x:datetime}|{y:datetime}", (DateTime x, DateTime y) =>
                Results.Ok(new { x, y }));

            appE.MapGet("/12-{x:minlength(1)}", (string x) =>
                Results.Ok(new { x, y = (string?)null }));

            appE.MapPut("/{x:alpha:minlength(2):maxlength(12)}", (string x) =>
                Results.Ok(new { x, y = (string?)null }));

            app.MapPut("/F/{x:regex(^[\\w\\.-]+@[\\w\\.-]+\\.by$)}", (string x) =>
            {
                return Results.Ok(new { x, y = (string?)null });
            });

            app.MapFallback((HttpContext ctx) =>
                Results.NotFound(new { message = $"{ctx.Request.Path.Value} not supported" }));

            app.Map("/Error", (HttpContext ctx) =>
            {
                var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                return Results.Ok(new { message = ex?.Message });
            });

            app.Run();
        }
    }
}