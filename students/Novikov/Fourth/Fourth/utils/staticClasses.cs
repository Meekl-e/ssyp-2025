using System.IO;
public static class HtmlPage
{
    public static string GetHtml(string title, string snippet)
    {
        return File.ReadAllText(Directory.GetCurrentDirectory() + @"\wwwroot\htmls\2.html")
        .Replace("{{ snippet }}", snippet)
        .Replace("{{ title }}", title);
    }

    public static string GetHtml1(string title, string snippet)
        {
        return File.ReadAllText(Directory.GetCurrentDirectory() + @"\wwwroot\htmls\3.html")
        .Replace("{{ snippet }}", snippet)
        .Replace("{{ title }}", title);
        }

    public static string GetMainHtml(int vk, int tg, int oB, int cNews, int academC, int elementy)
    {

        return File.ReadAllText(Directory.GetCurrentDirectory() + @"\wwwroot\htmls\index.html")
        .Replace("{{ vk }}", vk.ToString())
        .Replace("{{ tg }}", tg.ToString())
        .Replace("{{ oB }}", oB.ToString())
        .Replace("{{ cNews }}", cNews.ToString())
        .Replace("{{ academC }}", academC.ToString())
        .Replace("{{ elementy }}", elementy.ToString());
    }
}

public class APIResults
{
    public required List<List<string>> values { get; set; } 
}
