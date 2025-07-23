

using System.Text.Json;

public class RssView : DefaultView
{
    string source = "";
    public RssController cNController;
    public RssController aCController;
    public RssController elController;
    
    public RssView(string source)
    {
        this.source = source;
        if (source == "cnews")
        {
            cNController = new("https://www.cnews.ru/inc/rss/news.xml", source);
        }
        if (source == "academcity")
        {
            aCController = new("https://academcity.org/rss.xml", source);
        }
        if (source == "elementy")
        {
            elController = new("https://elementy.ru/rss/news/it", source);
        }
    }
   

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
        return Results.Content(HtmlPage.GetHtml("", source), "text/html");
    }
    public string Search(HttpRequest request){
        if (request.Query.ContainsKey("search")){
            string query_search = request.Query["search"];
            if (query_search != null && query_search != "")
                {
                    if (source == "cnews")
                    {
                        return this.cNController.Search(query_search.Split(" "));
                    }
                    if (source == "academcity")
                    {
                        return this.aCController.Search(query_search.Split(" "));
                    }
                    if (source == "elementy")
                    {
                        return this.elController.Search(query_search.Split(" "));
                    }
                    return "";
                    }
             }
        return JsonSerializer.Serialize("");
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