

class OldBaseView : DefaultView
{
    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/old_base_field?num={num}");
        }
        return Results.Content(OldBaseController.CreateField(num), "text/html");
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

        return Results.Content(OldBaseController.CreateHtml(year), "text/html");
    }
}