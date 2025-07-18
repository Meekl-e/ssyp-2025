using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();
app.MapGet("/", () => {
    return Results.Content(
$@"<!DOCTYPE html>
<html>
    <head><meta charset='utf-8'>    </head>
    <body>
<ul>
        <li><a href='http://localhost:5088/html'> Hello html! </a>
        <li><a href='http://localhost:5088/xml'> Hello xml! </a>
<ul>
    </body>
</html>", "text/html");
});
app.MapGet("/html", () => {
    return Results.Content(
$@"<!DOCTYPE html>
<html>
    <head><meta charset='utf-8'>    </head>
    <body>
        <h1>Hello HTML!</h1>
        <p> 
            <img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTuFAoHUEQ7PBsW9cQCtqWbgcuJaYyCUk47_Q&s' align='right'/>
        </p>
<a href='http://localhost:5088'> Назад </a>
    </body>
</html>", "text/html");
});
app.MapGet("/xml", () => {
    return Results.Content(
$@"<!DOCTYPE html>
<html>
    <head><meta charset='utf-8'>    </head>
    <body>
        <h1>Hello XML!</h1>
        <p> 
            <img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR1e0j0LTVpSq97QqJayWg_yc-DZ_C92cOYGA&s' align='right'/> 
            </p>
<a href='http://localhost:5088'> Назад </a>
    </body>
</html>", "text/html");
});
app.MapGet("/entry/{x}/{y}", (int x, String y) => $"Ваши параметры: \nЦелое {x} \nСтрока {y}");
app.MapGet("/user/{id}", (int id) => Results.Content(Users.GetUser(id), "text/json"));
app.MapGet("/users/", async (HttpRequest request) =>
{
    string html = "<ul>";
    HttpClient client = new HttpClient(); // создаем клиент для отправки запроса
    for (int i = 1230; i < 1236; i++)
    {
        string Url = request.Headers["Host"]; // считываем адрес localhost
        HttpResponseMessage response = await client.GetAsync($"http://{Url}/user/{i}"); // отправляем запрос асинхронно 
        string json = await response.Content.ReadAsStringAsync(); // читаем ответ также асинхронно
        html += $"<li>{json}</li>"; // вставляем в html
    }
    html += "</ul>";
    return Results.Content(html, "text/html"); // выводим результат
});
app.MapGet("/wikipedia/", async (HttpRequest request) =>
{

    string? search = request.Query["search"];

    using (HttpClient client = new HttpClient()) // безопасно используем клиент
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaSearchApp/1.0 (contact@example.com)"); // Заголовки запроса (необходимы для авторизации)

        string url = $"https://ru.wikipedia.org/w/api.php?" + // адрес API
                     $"action=query&" + // действие - запрос
                     $"list=search&" + // тип - поиск
                     $"srsearch={Uri.EscapeDataString(search)}&" + // запрос в безопасном формате
                     $"utf8=1&" + // кодировка ответа
                     $"format=json&" + // формат ответа
                     $"srlimit=1"; // лимит поиска

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        WikiSearchResult result = JsonSerializer.Deserialize<WikiSearchResult>(json);
        string title = result.query.search[0].title; // получаем название
        string snippet = result.query.search[0].snippet; // получаем сниппет
        return Results.Content(HtmlPage.GetHtml(title, snippet), "text/html"); // красиво выводим
    }
});
app.Run();
