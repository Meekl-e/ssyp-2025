using System.Text.Json;
using Nestor;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();


MainPageView mainPageView = new();
app.MapGet("/", (HttpRequest request) => mainPageView.GetResult(request));

VkView vkView = new();
app.MapGet("/vk/", (HttpRequest request) => vkView.GetResult(request));
app.MapGet("/vk_field/", (HttpRequest request) => vkView.GetFieldResult(request));

TgView tgView = new();
app.MapGet("/tg/", (HttpRequest request) => tgView.GetResult(request));
app.MapGet("/tg_field/", (HttpRequest request) => tgView.GetFieldResult(request));

OldBaseView oldBaseView = new();
app.MapGet("/old_base/", (HttpRequest request) => oldBaseView.GetResult(request));
app.MapGet("/old_base_field/", (HttpRequest request) => oldBaseView.GetFieldResult(request));

RssView cNView = new("cnews");
app.MapGet("/cnews/", (HttpRequest request) => cNView.GetResult(request));
app.MapGet("/cnews_field/", (HttpRequest request) => cNView.GetFieldResult(request));

RssView aCView = new("academcity");
app.MapGet("/academcity/", (HttpRequest request) => aCView.GetResult(request));
app.MapGet("/academcity_field/", (HttpRequest request) =>aCView.GetFieldResult(request));

RssView elView = new("elementy");
app.MapGet("/elementy/", (HttpRequest request) => elView.GetResult(request));
app.MapGet("/elementy_field/", (HttpRequest request) => elView.GetFieldResult(request));

ErshovArchiveView ershovArchive = new();
app.MapGet("/ershov_archive/", (HttpRequest request) => ershovArchive.GetResult(request));
app.MapGet("/ershov_archive_field/", (HttpRequest request) => ershovArchive.GetFieldResult(request));

app.MapGet("/search/", (HttpRequest request) => ershovArchive.GetResult(request));

View view = new();
app.MapGet("/syp_search", (HttpRequest request) => view.GetResult(request));

app.MapGet("/person", (HttpRequest request) => view.GetPersonResult(request));

app.MapGet("/studio", (HttpRequest request) => view.GetStudioResult(request));

app.Run();