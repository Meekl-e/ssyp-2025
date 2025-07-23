

using System.Text.Json;

class SearchView 
{
    VkView vk;
    TgView tg;
    OldBaseView old;
    RssView cNView;
    RssView aCView;
    RssView elView;
    ErshovArchiveView ershov;
    List<DefaultView> views = new();

    public SearchView(ref VkView vk, ref TgView tg, ref OldBaseView old, ref RssView cNView, ref RssView aCView, ref RssView elView, ref ErshovArchiveView ershov)
    {
        views.Add(vk);
        views.Add(tg);
        views.Add(old);
        views.Add(cNView);
        views.Add(aCView);
        views.Add(elView);
        views.Add(ershov);
        
    }

    public IResult GetFieldResult(HttpRequest request)
    {
        throw new NotImplementedException();
    }

    public IResult GetResult(HttpRequest request)
    {
        var query = request.Query;
        Dictionary<DefaultView, bool> map_search = new();

        map_search[this.ershov] = query.ContainsKey("ershArch");
        map_search[this.vk] = query.ContainsKey("vkN");
        map_search[this.tg] = query.ContainsKey("tgN");
        map_search[this.old] = query.ContainsKey("oBN");
        map_search[this.cNView] = query.ContainsKey("cNewsN");
        map_search[this.aCView] = query.ContainsKey("academCN");
        map_search[this.elView] = query.ContainsKey("elementyN");

        

        if (query.ContainsKey("search") && request.Query["search"] != "")
        {
            string resuls = JsonSerializer.Serialize(this.views.Where(v => map_search[v]).Select(v => v.Search(request)));
            return Results.Content(resuls, "text/json");
        }
        else
        {
            return Results.Redirect("/");
        }
        
    

    }
}