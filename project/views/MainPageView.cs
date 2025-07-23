


class MainPageView : DefaultView
{

    private readonly Random rand = new();
    MainPageContoller mainPageContoller = new();
    VkView vk;
    TgView tg;
    OldBaseView old;
    RssView cNView;
    RssView aCView;
    RssView elView;
    
    ErshovArchiveView ershovView;

    public IResult GetResult(HttpRequest request)
    {
        bool redirect = false;
        if (!int.TryParse(request.Query["vkN"], out int vkN))
        {

            List<int> ids = vk.vkController.GetIds();
            vkN = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["tgN"], out int tgN))
        {
            List<int> ids = tg.tgController.GetIds();
            tgN = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["oBN"], out int oBN))
        {
            List<int> ids = old.oldBaseController.GetIds();
            oBN = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (!int.TryParse(request.Query["cNewsN"], out int cNewsN))
        {
            int count = cNView.cNController.GetCount();
            cNewsN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["academCN"], out int academCN))
        {
            int count = aCView.aCController.GetCount();
            academCN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["elementyN"], out int elementyN))
        {

            int count = elView.elController.GetCount();
            elementyN = rand.Next(0, count - 1);
            redirect = true;
        }
        if (!int.TryParse(request.Query["ershArch"], out int ershArch))
        {

            List<int> ids = ershovView.ershov_controller.GetIds();
            ershArch = ids[rand.Next(0, ids.Count - 1)];
            redirect = true;
        }
        if (redirect)
        {
            return Results.Redirect($"/?vkN={vkN}&tgN={tgN}&oBN={oBN}&cNewsN={cNewsN}&academCN={academCN}&elementyN={elementyN}&ershArch={ershArch}");
        }
        return Results.Content(mainPageContoller.CreateHtml(vkN, tgN, oBN, cNewsN, academCN, elementyN, ershArch), "text/html");
    }

    public MainPageView(ref VkView vk, ref TgView tg, ref OldBaseView old, ref RssView cNView, ref RssView aCView, ref RssView elView, ref ErshovArchiveView ershovArchiveView)
    {
        this.vk = vk;
        this.tg = tg;
        this.old = old;
        this.cNView = cNView;
        this.aCView = aCView;
        this.elView = elView;
        this.ershovView = ershovArchiveView;
    }
    
    public IResult GetFieldResult(HttpRequest request)
    {
        throw new NotImplementedException();
    }

}