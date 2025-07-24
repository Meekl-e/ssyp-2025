using System.Text.Json;
using Nestor;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();


VkView vkView = new();
app.MapGet("/vk/", (HttpRequest request) => vkView.GetResult(request));
app.MapGet("/vk_field/", (HttpRequest request) => vkView.GetFieldResult(request));
app.MapGet("/vk_main_field/", (HttpRequest request) => vkView.GetMainFieldResult(request));

TgView tgView = new();
app.MapGet("/tg/", (HttpRequest request) => tgView.GetResult(request));
app.MapGet("/tg_field/", (HttpRequest request) => tgView.GetFieldResult(request));
app.MapGet("/tg_main_field/", (HttpRequest request) => tgView.GetMainFieldResult(request));


OldBaseView oldBaseView = new();
app.MapGet("/old_base/", (HttpRequest request) => oldBaseView.GetResult(request));
app.MapGet("/old_base_field/", (HttpRequest request) => oldBaseView.GetFieldResult(request));
app.MapGet("/old_base_main_field/", (HttpRequest request) => oldBaseView.GetMainFieldResult(request));


RssView cNView = new("cnews");
app.MapGet("/cnews/", (HttpRequest request) => cNView.GetResult(request));
app.MapGet("/cnews_field/", (HttpRequest request) => cNView.GetFieldResult(request));
app.MapGet("/cnews_main_field/", (HttpRequest request) => cNView.GetMainFieldResult(request));

RssView aCView = new("academcity");
app.MapGet("/academcity/", (HttpRequest request) => aCView.GetResult(request));
app.MapGet("/academcity_field/", (HttpRequest request) =>aCView.GetFieldResult(request));
app.MapGet("/academcity_main_field/", (HttpRequest request) =>aCView.GetMainFieldResult(request));

RssView elView = new("elementy");
app.MapGet("/elementy/", (HttpRequest request) => elView.GetResult(request));
app.MapGet("/elementy_field/", (HttpRequest request) => elView.GetFieldResult(request));
app.MapGet("/elementy_main_field/", (HttpRequest request) => elView.GetMainFieldResult(request));

ErshovArchiveView ershovArchiveView = new();
app.MapGet("/ershov_archive/", (HttpRequest request) => ershovArchiveView.GetResult(request));
app.MapGet("/ershov_archive_field/", (HttpRequest request) => ershovArchiveView.GetFieldResult(request));
app.MapGet("/ershov_archive_main_field/", (HttpRequest request) => ershovArchiveView.GetMainFieldResult(request));


View view = new();
app.MapGet("/syp_search", (HttpRequest request) => view.GetResult(request));

app.MapGet("/person", (HttpRequest request) => view.GetPersonResult(request));

app.MapGet("/studio", (HttpRequest request) => view.GetStudioResult(request));


MainPageView mainPageView = new(ref vkView, ref tgView,ref oldBaseView, ref cNView, ref aCView, ref elView, ref ershovArchiveView );
app.MapGet("/", (HttpRequest request) => mainPageView.GetResult(request));

SearchView searchView = new(ref vkView, ref tgView, ref oldBaseView, ref cNView, ref aCView, ref elView, ref ershovArchiveView );
app.MapGet("/search", (HttpRequest request) => searchView.GetResult(request));
app.Run();