using System.Text.Json;
using Nestor;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();

NestorMorph morph = new();

MainPageView mainPageView = new();
app.MapGet("/", (HttpRequest request) => mainPageView.GetResult(request));

//const int port = 5118;
// app.MapGet("/", () => {
//     return Results.Content(
// $@"<!DOCTYPE html>
// <html>
//     <head><meta charset='utf-8'>    </head>
//     <body>
// <ul>
//         <li><a href='/vk?start=0&step=10'> Посты ВКонтакте ЛШЮПа </a>
//         <li><a href='/old_base?year=2017'> Старые мастерские ЛШЮПа </a>
//         <li><a href='/cnews'> cnews </a>
//         <li><a href='/academcity'> academcity </a>
//         <li><a href='/elementy'> elementy </a>
//         <li><a href='/tg?start=0&step=10'> Телеграм ЛШЮПа </a>
// <ul>
//     </body>
// </html", "text/html");
// });

VkView vkView = new();
app.MapGet("/vk/", (HttpRequest request) => vkView.GetResult(request));
app.MapGet("/vk_field/", (HttpRequest request) => vkView.GetFieldResult(request));

TgView tgView = new();
app.MapGet("/tg/", (HttpRequest request) => tgView.GetResult(request));
app.MapGet("/tg_field/", (HttpRequest request) => tgView.GetFieldResult(request));

OldBaseView oldBaseView = new();
app.MapGet("/old_base/", (HttpRequest request) => oldBaseView.GetResult(request));
app.MapGet("/old_base_field/", (HttpRequest request) => oldBaseView.GetFieldResult(request));

RssView cNView = new("https://www.cnews.ru/inc/rss/news.xml");
app.MapGet("/cnews/", (HttpRequest request) => cNView.GetResult(request));
app.MapGet("/cnews_field/", (HttpRequest request) => cNView.GetFieldResult(request));

RssView aCView = new("https://academcity.org/rss.xml");
app.MapGet("/academcity/", (HttpRequest request) => aCView.GetResult(request));
app.MapGet("/academcity_field/", (HttpRequest request) =>aCView.GetFieldResult(request));

RssView elView = new("https://elementy.ru/rss/news/it");
app.MapGet("/elementy/", (HttpRequest request) => elView.GetResult(request));
app.MapGet("/elementy_field/", (HttpRequest request) => elView.GetFieldResult(request));

ErshovArchiveView ershovArchive = new(morph);
app.MapGet("/ershov_archive/", (HttpRequest request) => ershovArchive.GetResult(request));
// app.MapGet("/old_base_field/", (HttpRequest request) => OldBaseReader.GetFieldResult(request));

app.MapGet("/search/", (HttpRequest request) => ershovArchive.GetResult(request));

app.Run();