var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

View view = new();
app.MapGet("/search", (HttpRequest request) => view.GetResult(request));

app.MapGet("/person", (HttpRequest request) => view.GetPersonResult(request));

app.MapGet("/studio", (HttpRequest request) => view.GetStudioResult(request));

app.Run();
