using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Nestor;
using System.Text.Json;

class ErshovArchiveView : DefaultView
{
    ErshovArchiveController ershov_controller;

    public ErshovArchiveView()
    {
        this.ershov_controller = new ErshovArchiveController("datasets/ErshArchData");

    }

    public string Search(HttpRequest request){
        if (request.Query.ContainsKey("search")){
            string query_search = request.Query["search"];
            if (query_search != null && query_search != "")
            {
                return ershov_controller.Search(query_search.Split(" "));
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

        return Results.Content(ershov_controller.CreateField(num), "text/html");
    }
}



