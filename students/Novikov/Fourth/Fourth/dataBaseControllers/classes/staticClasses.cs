using System.IO;
public static class HtmlPage
{
    public static string GetHtml(string title, string snippet)
    {
        return File.ReadAllText("wwwroot/htmls/databasePage.html")
        .Replace("{{ snippet }}", snippet)
        .Replace("{{ title }}", title);
    }

    public static string GetField(string title, string snippet)
        {
        return File.ReadAllText("wwwroot/htmls/databaseField.html")
        .Replace("{{ snippet }}", snippet)
        .Replace("{{ title }}", title);
        }

    public static string GetMainHtml(int vk, int tg, int oB, int cNews, int academC, int elementy, int ershArch)
    {

        return File.ReadAllText("wwwroot/htmls/index.html")
        .Replace("{{ vk }}", vk.ToString())
        .Replace("{{ tg }}", tg.ToString())
        .Replace("{{ oB }}", oB.ToString())
        .Replace("{{ cNews }}", cNews.ToString())
        .Replace("{{ academC }}", academC.ToString())
        .Replace("{{ elementy }}", elementy.ToString())
        .Replace("{{ ershArch }}", ershArch.ToString());
    }
}

public class APIResults
{
    public required List<List<string>> values { get; set; }
}

public interface DefaultView
{
    IResult GetResult(HttpRequest request);
    IResult GetFieldResult(HttpRequest request);
}
