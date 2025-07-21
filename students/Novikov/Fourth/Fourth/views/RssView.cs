

public class RssView : DefaultView
{
    string source = "";
    public RssView(string source)
    {
        this.source = source;
    }
    RssController cNController = new("https://www.cnews.ru/inc/rss/news.xml");
    RssController aCController = new("https://academcity.org/rss.xml");
    RssController elController = new("https://elementy.ru/rss/news/it");

    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/{source}_field?num={num}");
        }
        if (source == "cnews")
        {
            return Results.Content(cNController.CreateField(num), "text/html");
        }
        if (source == "academcity")
        {
            return Results.Content(aCController.CreateField(num), "text/html");
        }
        if (source == "elementy")
        {
            return Results.Content(elController.CreateField(num), "text/html");
        }
        return Results.Content(HtmlPage.GetHtml("", "Ошибка поиска"), "text/html");
    }


    public IResult GetResult(HttpRequest request)
    {
        if (source == "cnews")
        {
            return Results.Content(cNController.CreateHtml(), "text/html");
        }
        if (source == "academcity")
        {
            return Results.Content(aCController.CreateHtml(), "text/html");
        }
        if (source == "elementy")
        {
            return Results.Content(elController.CreateHtml(), "text/html");
        }
        return Results.Content(HtmlPage.GetHtml("", "Ошибка поиска"), "text/html");
    }
}