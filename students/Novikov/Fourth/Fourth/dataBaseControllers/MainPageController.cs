

public class MainPageContoller
{
    private readonly Random rand = new();
    public IResult GetResult(HttpRequest request)
    {
        bool redirect = false;
        if (!int.TryParse(request.Query["vkN"], out int vkN))
        {
            vkN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["tgN"], out int tgN))
        {
            tgN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["oBN"], out int oBN))
        {
            oBN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["cNewsN"], out int cNewsN))
        {
            cNewsN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["academCN"], out int academCN))
        {
            academCN = rand.Next();
            redirect = true;
        }
        if (!int.TryParse(request.Query["elementyN"], out int elementyN))
        {
            elementyN = rand.Next();
            redirect = true;
        }
        if (redirect)
        {
            return Results.Redirect($"/?vkN={vkN}&tgN={tgN}&oBN={oBN}&cNewsN={cNewsN}&academCN={academCN}&elementyN={elementyN}");
        }
        return Results.Content(CreateHtml(vkN, tgN, oBN, cNewsN, academCN, elementyN), "text/html");
    }
    public MainPageContoller()
    {

    }

    VkController vkController = new();
    RssController cNController = new("https://www.cnews.ru/inc/rss/news.xml");
    RssController aCController = new("https://academcity.org/rss.xml");
    RssController elController = new("https://elementy.ru/rss/news/it");
    public string CreateHtml(int vkN, int tgN, int oBN, int cNN, int aCN, int elN)
    {
        //return HtmlPage.GetHtml("", aCController.CreateField(academCN));
        return HtmlPage.GetMainHtml(vkN, tgN, oBN, cNN, aCN, elN);
    }

}