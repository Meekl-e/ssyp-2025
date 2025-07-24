

using System.Text.Json;

class TgView : DefaultView
{
    public TgController tgController = new();

    public string Search(HttpRequest request)
    {
        if (request.Query.ContainsKey("search"))
        {
            string query_search = request.Query["search"];
            if (query_search != null && query_search != "")
            {
                return tgController.Search(query_search.Split(" "));
            }
        }
        return JsonSerializer.Serialize("");
    }

    public IResult GetResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["start"], out int start))
        {
            start = 0;
        }
        if (!int.TryParse(request.Query["step"], out int step))
        {
            step = 10;
        }
        if (start < 0)
        {
            return Results.Redirect($"/tg?start=0&step={step}");
        }

        return Results.Content(tgController.CreateHtml(start, step), "text/html");
    }

    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/tg_field?num={num}");
        }
        return Results.Content(tgController.CreateField(num, false), "text/html");
    }
    
    public IResult GetMainFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/tg_field?num={num}");
        }
        return Results.Content(tgController.CreateField(num, true), "text/html");
    }
    
}