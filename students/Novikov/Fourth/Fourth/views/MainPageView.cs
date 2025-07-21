


class MainPageView : DefaultView
{

    private readonly Random rand = new();
    MainPageContoller mainPageContoller = new();

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
        return Results.Content(mainPageContoller.CreateHtml(vkN, tgN, oBN, cNewsN, academCN, elementyN), "text/html");
    }
    
    public IResult GetFieldResult(HttpRequest request)
    {
        throw new NotImplementedException();
    }

}