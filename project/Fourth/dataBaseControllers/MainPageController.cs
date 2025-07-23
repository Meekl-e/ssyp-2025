

public class MainPageContoller
{

    public string CreateHtml(int vkN, int tgN, int oBN, int cNN, int aCN, int elN, int ershArchN)
    {
        return HtmlPage.GetMainHtml(vkN, tgN, oBN, cNN, aCN, elN, ershArchN);
    }

}