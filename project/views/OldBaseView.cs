

class OldBaseView : DefaultView
{
    public OldBaseController oldBaseController = new();

    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/old_base_field?num={num}");
        }
        return Results.Content(oldBaseController.CreateField(num, false), "text/html");
    }

    public IResult GetMainFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/old_base_main_field?num={num}");
        }
        return Results.Content(oldBaseController.CreateField(num, true), "text/html");
    }

    public IResult GetResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["year"], out int year))
        {
            year = 2017;
        }
        if (year > 2017)
        {
            return Results.Redirect("/old_base?year=2017");
        }

        return Results.Content(oldBaseController.CreateHtml(year), "text/html");
    }
}