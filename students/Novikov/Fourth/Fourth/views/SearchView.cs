

using System.Text.Json;

class SearchView 
{
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
        if (query.Count() == 0)
        {
            return Results.Redirect("/");
        }

        map_search[this.views[6]] = query.ContainsKey("ershArch");
        map_search[this.views[0]] = query.ContainsKey("vkN");
        map_search[this.views[1]] = query.ContainsKey("tgN");
        map_search[this.views[2]] = query.ContainsKey("oBN");
        map_search[this.views[3]] = query.ContainsKey("cNewsN");
        map_search[this.views[4]] = query.ContainsKey("academCN");
        map_search[this.views[5]] = query.ContainsKey("elementyN");

        

        if (query.ContainsKey("search") && request.Query["search"] != "")
        {
            var r = this.views.Where(v => map_search[v]).Select(v => v.Search(request));
            
            Console.WriteLine(r.Count());
            string resuls = "["+r.Aggregate((a, x)=>a+","+x)+"]";
            return Results.Content(resuls, "text/json");
        }
        else
        {
            return Results.Redirect("/");
        }
        
    

    }
}