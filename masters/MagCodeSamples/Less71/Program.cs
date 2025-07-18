using Less71;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();

Controller controller = new Controller();


app.MapGet("/", (HttpRequest request) => $"Hello, {Directory.GetCurrentDirectory()}");
app.MapGet("/vk/", (HttpRequest request) => Results.Content(controller.CreateHtml(), "text/html"));
app.MapGet("/f", () => Results.File("C:\\Home\\dev2025\\ssyp-2025-workshop-3\\masters\\MagCodeSamples\\Less71\\wwwroot\\photos/1000-0.jpg", "image/jpeg"));
app.MapGet("/v", () => Results.File("photos/1000-0.jpg", "image/jpeg"));

app.Run();


