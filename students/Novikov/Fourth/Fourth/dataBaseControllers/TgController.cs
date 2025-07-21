using System.Text.Json;
using System.Xml.Linq;

public class TgController
{

    public static string GenerateImage(string file_id)
    {
        if (file_id == "None") return "";
        return $"<img style='width:465px;' src=\"https://drive.google.com/thumbnail?authuser=0&id={file_id}&sz=w1000\" />";
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

    public static string CreateField(int rawNum)
    {
        APIResults? googleResults = Read().Result;
        if (googleResults is null) return "";
        int rowsCount = googleResults.values.Count;
        int num = rawNum % (rowsCount - 1);
        List<string> row = googleResults.values[num + 1];
        string html = "";
        if (row.Count == 6)
        {
            DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1]));
            html = $"{row[5]} <i>{time.Date}</i><br>{GenerateImage(row[3])}";
        }
        return html.ToString();
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