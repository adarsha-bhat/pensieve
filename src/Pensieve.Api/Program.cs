var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// The built frontend (src/Pensieve.Web) is served from wwwroot.
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

// Unknown API routes are real 404s; every other unknown path belongs to the single-page app.
app.MapFallback("/api/{**path}", () => Results.NotFound());
app.MapFallbackToFile("index.html");

app.Run();

// Lets the test project start the app with WebApplicationFactory<Program>.
public partial class Program;
