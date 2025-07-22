


class MainPageView : DefaultView
{

    private readonly Random rand = new();
    MainPageContoller mainPageContoller = new();

    public IResult GetResult(HttpRequest request)
    {
        bool redirect = false;
        if (!int.TryParse(request.Query["vkN"], out int vkN))
        {
            VkController vkController = new();
            List<int> ids = vkController.GetIds();
            vkN = ids[rand.Next(0, ids.Count- 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["tgN"], out int tgN))
        {
            List<int> ids = TgController.GetIds();
            tgN = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["oBN"], out int oBN))
        {
            List<int> ids = OldBaseController.GetIds();
            oBN = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["cNewsN"], out int cNewsN))
        {
            RssController cNController = new("https://www.cnews.ru/inc/rss/news.xml");
            int count = cNController.GetCount();
            cNewsN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["academCN"], out int academCN))
        {
            RssController aCController = new("https://academcity.org/rss.xml");
            int count = aCController.GetCount();
            academCN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["elementyN"], out int elementyN))
        {
            RssController elController = new("https://elementy.ru/rss/news/it");
            int count = elController.GetCount();
            elementyN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["ershArch"], out int ershArch))
        {
            ershArch = rand.Next(1, 100); // add abs random
            redirect = true;
        }
        if (redirect)
        {
            return Results.Redirect($"/?vkN={vkN}&tgN={tgN}&oBN={oBN}&cNewsN={cNewsN}&academCN={academCN}&elementyN={elementyN}&ershArch={ershArch}");
        }
        return Results.Content(mainPageContoller.CreateHtml(vkN, tgN, oBN, cNewsN, academCN, elementyN, ershArch), "text/html");
    }
    
    public IResult GetFieldResult(HttpRequest request)
    {
        throw new NotImplementedException();
    }

}