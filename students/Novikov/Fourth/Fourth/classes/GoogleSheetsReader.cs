using System.Text.Json;

public class GoogleSheetsReader
{
    public static IResult GetResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["start"], out int start))
        {
            start = 0;
        }
        if (!int.TryParse(request.Query["step"], out int step))
        {
            step = 10;
        }
        if (start < 0)
        {
            return Results.Redirect($"/tg?start=0&step={step}");
        }

        return Results.Content(CreateHtml(start, step), "text/html");
    }
    public static string GenerateImage(string file_id)
    {
        if (file_id == "None") return "";
        return $"<img src=\"https://drive.google.com/thumbnail?authuser=0&id={file_id}&sz=w1000\" />";
    }
    public static async Task<APIResults?> Read()
    {
        string spreadsheetId = "12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI";
        string range = "Лист1!A:F";
        string apiKey = "AIzaSyDqDdzU6h-JUqxxLVXDeoL1ei9FZsj8IXA";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using HttpClient client = new HttpClient();
        string json = await client.GetStringAsync(url);
        APIResults? result = JsonSerializer.Deserialize<APIResults>(json);
        return result;
    }
    public static string CreateHtml(int start, int step)
    {
        APIResults? googleResults = Read().Result;
        if (googleResults is null) return "";
        string html = $@"<a href='/tg?start={start - step}&step={step}'>Назад</a>
        <a href='/tg?start={start + step}&step={step}'>Вперёд</a>
        <br>
        <ul>";
        int absPostsCount = 0;
        int postsCount = 0;
        foreach (List<string> row in googleResults.values)
        {
            if (absPostsCount <= start)
            {
                absPostsCount += 1;
                continue;
            }
            if (postsCount <= step)
            {
                if (row.Count == 6 && row[0] != "ID")
                {
                    postsCount += 1;
                    DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1]));
                    html += $"<li>{row[5]} <i>{time.Date}</i><br>{GenerateImage(row[3])}</li>";
                }
            }
        }

        html += "</ul>";

        return HtmlPage.GetHtml("Телеграм канал", html);
    }
}