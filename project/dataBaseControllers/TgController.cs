using System.Text.Json;
using System.Xml.Linq;

public class TgController : DefaultController
{
    APIResults? tgResults;
    List<DefaultObject> docs_to_search;
    public WordsSearcher<string, int> searcher;

    public TgController()
    {
        tgResults = Read().Result;
        this.docs_to_search = tgResults.values.Skip(1).Where(row => row.Count == 6).Select(x=>new DefaultObject("tg"){description=x[5], id=int.Parse(x[0])}).ToList();
        DataSourceList dsl = new DataSourceList([.. docs_to_search.Select(o => o.description)]);
        this.searcher = new WordsSearcher<string, int>(dsl);
        Console.WriteLine("TG Loaded");
    }

    public List<int> GetIds()
    {
        List<int> ids = new();
        foreach (List<string> row in tgResults.values)
        {
            if (row.Count == 6 && row[0] != "ID")
            {
                ids.Add(Int32.Parse(row[0]));
            }
        }
        return ids;
    }

    public string GenerateImage(string file_id)
    {
        if (file_id == "None") return "";
        return $"<img style='width:465px;' src=\"https://drive.google.com/thumbnail?authuser=0&id={file_id}&sz=w1000\" />";
    }
    public async Task<APIResults?> Read()
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

    public string CreateField(int id, bool onMainPage)
    {
        if (tgResults is null) return "";
        string html = "";
        foreach (List<string> row in tgResults.values)
        {
            if (row.Count == 6 && row[0] == id.ToString())
            {
                DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1]));
                html = $"{row[5]} <i>{time.Year}.{time.Month}.{time.Day}</i><br>{GenerateImage(row[3])}";
            }
        }
        if (onMainPage)
        {
            return html.ToString();
        }
        return HtmlPage.GetHtml("Телеграм", html.ToString());
    }

    public string CreateHtml(int start, int step)
    {

        if (tgResults is null) return "";
        string html = $@"<a href='/tg?start={start - step}&step={step}'>Назад</a>
        <a href='/tg?start={start + step}&step={step}'>Вперёд</a>
        <br>
        <ul>";
        int absPostsCount = 0;
        int postsCount = 0;
        foreach (List<string> row in tgResults.values)
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

    public string Search(string[] query_search)
    {
        var search_result = searcher.SearchForKey(query_search).Select(x => docs_to_search[x.Item1]);

        return JsonSerializer.Serialize(search_result);
    }
}