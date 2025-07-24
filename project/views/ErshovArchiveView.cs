using System.Text.Json;

class ErshovArchiveView : DefaultView
{
    public ErshovArchiveController ershov_controller;

    public ErshovArchiveView()
    {
        this.ershov_controller = new ErshovArchiveController("datasets/ErshArchData");

    }

    public string Search(HttpRequest request)
    {
        if (request.Query.ContainsKey("search"))
        {
            if (request.Query["search"].ToString() != null && request.Query["search"].ToString() != "")
            {
                return ershov_controller.Search(request.Query["search"].ToString().Split(" "));
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
            return Results.Redirect($"/ershov_archive?start=0&step={step}");
        }
        return Results.Content(ershov_controller.CreateHtml(start, step), "text/html");
    }


    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/ershov_archive_field?num={num}");
        }

        return Results.Content(ershov_controller.CreateField(num, false), "text/html");
    }

    public IResult GetMainFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/ershov_archive_main_field?num={num}");
        }

        return Results.Content(ershov_controller.CreateField(num, true), "text/html");
    }
    
    public IResult GetSearchResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/ershov_archive_field?num={num}");
        }
        return Results.Content(ershov_controller.CreateSearchView(num), "text/html");
    }
}



