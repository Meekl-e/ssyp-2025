using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();

MainPageContoller mainPageContoller = new();
app.MapGet("/", (HttpRequest request) => mainPageContoller.GetResult(request));

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

VkController vkController = new();
app.MapGet("/vk/", (HttpRequest request) => vkController.GetResult(request));
app.MapGet("/vk_field/", (HttpRequest request) => vkController.GetFieldResult(request));

app.MapGet("/tg/", (HttpRequest request) => GoogleSheetsReader.GetResult(request));
app.MapGet("/tg_field/", (HttpRequest request) => GoogleSheetsReader.GetFieldResult(request));

app.MapGet("/old_base/", (HttpRequest request) => OldBaseReader.GetResult(request));
app.MapGet("/old_base_field/", (HttpRequest request) => OldBaseReader.GetFieldResult(request));

RssController cNController = new("https://www.cnews.ru/inc/rss/news.xml");
app.MapGet("/cnews/", (HttpRequest request) => Results.Content(cNController.CreateHtml(), "text/html"));
app.MapGet("/cnews_field/", (HttpRequest request) => cNController.GetFieldResult(request, "cnews"));

RssController aCController = new("https://academcity.org/rss.xml");
app.MapGet("/academcity/", (HttpRequest request) => Results.Content(aCController.CreateHtml(), "text/html"));
app.MapGet("/academcity_field/", (HttpRequest request) =>aCController.GetFieldResult(request, "academcity"));

RssController elController = new("https://elementy.ru/rss/news/it");
app.MapGet("/elementy/", (HttpRequest request) => Results.Content(elController.CreateHtml(), "text/html"));
app.MapGet("/elementy_field/", (HttpRequest request) => elController.GetFieldResult(request, "elementy"));

app.Run();