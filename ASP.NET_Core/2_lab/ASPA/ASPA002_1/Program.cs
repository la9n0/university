var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseWelcomePage("/aspnetcore");
app.UseStaticFiles();
app.MapGet("/fil", () => "Hello World!");

app.Run();
