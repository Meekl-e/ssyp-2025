

public class MainPageContoller
{

    VkController vkController = new();
    RssController cNController = new("https://www.cnews.ru/inc/rss/news.xml");
    RssController aCController = new("https://academcity.org/rss.xml");
    RssController elController = new("https://elementy.ru/rss/news/it");
    public string CreateHtml(int vkN, int tgN, int oBN, int cNN, int aCN, int elN)
    {
        return HtmlPage.GetMainHtml(vkN, tgN, oBN, cNN, aCN, elN);
    }

}