
public class View
{
    public IResult GetResult(HttpRequest request)
    {
        if (request.Query["search"] == "")
        {
            Results.Content("Введите запрос для поиска", "text/plain");
        }

        return Results.Content(Controller.CreateHtml(request.Query["search"]), "text/html");
    }
    public IResult GetPersonResult(HttpRequest request)
    {
        if (request.Query["id"] == "")
        {
            Results.Content("Invalid id", "text/plain");
        }

        return Results.Content(Controller.CreatePersonHtml(request.Query["id"]), "text/html");
    }

    public IResult GetStudioResult(HttpRequest request)
    {
        if (request.Query["id"] == "")
        {
            Results.Content("Invalid id", "text/plain");
        }

        return Results.Content(Controller.CreateStudioHtml(request.Query["id"]), "text/html");
    }
}