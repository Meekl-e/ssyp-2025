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

}