using DAL003;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider("D:\\university\\4_sem\\tpi\\3_lab\\Celebrities"),
	RequestPath = new PathString("/Photo")
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine("D:\\university\\4_sem\\tpi\\3_lab\\Celebrities")),
	RequestPath = "/Celebrities/download"
});

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine("D:\\university\\4_sem\\tpi\\3_lab\\Celebrities")),
	RequestPath = "/Celebrities/download",
	OnPrepareResponse = ctx =>
	{
		ctx.Context.Response.Headers.Append("Content-Disposition", "attachment");
	}
});

using (IRepository repository = Repository.Create("Celebrities"))
{
	app.MapGet("/Celebrities", () => repository.getAllCelebrities());
	app.MapGet("/Celebrities/{id:int}", (int id) => repository.GetCelebrityById(id));
	app.MapGet("/Celebrities/BySurname/{surname}", (string surname) => repository.GetCelebritiesBySurname(surname));
	app.MapGet("/Celebrities/PhotoPathById/{id:int}", (int id) => repository.getPhotoPathById(id));
	app.MapGet("/", () => "Hello World!");
	app.Run();
}
