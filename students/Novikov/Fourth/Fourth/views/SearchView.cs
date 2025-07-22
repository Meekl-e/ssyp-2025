

using System.Text.Json;

public class SearchView : DefaultView
{
    public IResult GetFieldResult(HttpRequest request)
    {
        throw new NotImplementedException();
    }

    public IResult GetResult(HttpRequest request)
    {
        var query = request.Query;
        var result = "";
        return Results.NotFound();

    }
}