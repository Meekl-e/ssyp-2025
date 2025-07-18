using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();
const int port = 5118;
app.MapGet("/", () => {
    return Results.Content(
$@"<!DOCTYPE html>
<html>
    <head><meta charset='utf-8'>    </head>
    <body>
<ul>
        <li><a href='/html'> Hello html! </a>
        <li><a href='/xml'> Hello xml! </a>
        <li><a href='/plain'> Hello plain! </a>
        <li><a href='/json'> Hello json! </a>
        <li><a href='/jpg'> Hello jpg! </a>
        <li><a href='/mp4'> Hello mp4! </a>
        <li><a href='/pdf'> Hello pdf! </a>
<ul>
    </body>
</html", "text/html");
});
app.MapGet("/html", () => {
    return Results.Content(
$@"<!DOCTYPE html>
<html>
    <head><meta charset='utf-8'>    </head>
    <body>
        <h1>Hello html!</h1>
        
    </body>
</html>", "text/html");
});
app.MapGet("/xml", () => {
    return Results.Content(
$@"<text>Hello xml!</text>", "text/xml");
});
app.MapGet("/plain", () => {
    return Results.Content(
$@"Hello plain!", "text/plain");
});
app.MapGet("/json", () => {
    return Results.Content(
        @"{
'text': 'Hello json!'
   
}", "text/json");
});
app.MapGet("/jpg", () => {
    return Results.File(
    "images/675.jpg", "image/jpg");
});
app.MapGet("/mp4", () => {
    return Results.File(
    "videos/674.mp4", "video/mp4");
});
app.MapGet("/pdf", () => {
    return Results.File(
    "docs/257.pdf", "document/pdf");
});










app.MapGet("/entry/{x}/{y}", (int x, String y) => $"���� ���������: \n����� {x} \n������ {y}");
app.MapGet("/user/{id}", (int id) => Results.Content(Users.GetUser(id), "text/json"));
app.MapGet("/users/", async (HttpRequest request) =>
{
    string html = "<ul>";
    HttpClient client = new HttpClient(); // ������� ������ ��� �������� �������
    for (int i = 1230; i < 1236; i++)
    {
        string Url = request.Headers["Host"]; // ��������� ����� localhost
        HttpResponseMessage response = await client.GetAsync($"http://{Url}/user/{i}"); // ���������� ������ ���������� 
        string json = await response.Content.ReadAsStringAsync(); // ������ ����� ����� ����������
        html += $"<li>{json}</li>"; // ��������� � html
    }
    html += "</ul>";
    return Results.Content(html, "text/html"); // ������� ���������
});






app.MapGet("/wikipedia/", async (HttpRequest request) =>
{

    string? search = request.Query["search"];

    using (HttpClient client = new HttpClient()) // ��������� ���������� ������
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaSearchApp/1.0 (contact@example.com)"); // ��������� ������� (���������� ��� �����������)

        string url = $"https://ru.wikipedia.org/w/api.php?" + // ����� API
                     $"action=query&" + // �������� - ������
                     $"list=search&" + // ��� - �����
                     $"srsearch={Uri.EscapeDataString(search)}&" + // ������ � ���������� �������
                     $"utf8=1&" + // ��������� ������
                     $"format=json&" + // ������ ������
                     $"srlimit=1"; // ����� ������

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        WikiSearchResult result = JsonSerializer.Deserialize<WikiSearchResult>(json);
        string title = result.query.search[0].title; // �������� ��������
        string snippet = result.query.search[0].snippet; // �������� �������
        return Results.Content(HtmlPage.GetHtml(title, snippet), "text/html"); // ������� �������
    }
});
Controller controller = new Controller();
app.MapGet("/vk/", (HttpRequest request) => Results.Content(controller.CreateHtml(int.Parse(request.Query["start"]), int.Parse(request.Query["step"])), "text/html"));
// app.MapGet("/vk/", (HttpRequest request) => Results.Content(controller.CreateHtml(), "text/html"));

//app.MapGet("/vk/", (HttpRequest request) => Results.Content(controller.CreateHtml(), "text/html"));

app.MapGet("/sheets/", (HttpRequest request) => Results.Content(GoogleSheetsReader.CreateHtml(), "text/html"));
app.MapGet("/old_base/", (HttpRequest request) => Results.Content(OldBaseReader.CreateHtml(), "text/html"));
app.Run();
