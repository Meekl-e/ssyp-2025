

using Nestor;

public class VkView : DefaultView
{
    VkController vkController = new();
    


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
            return Results.Redirect($"/vk?start=0&step={step}");
        }

        return Results.Content(vkController.CreateHtml(start, step), "text/html");
    }

    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/vk_field?num={num}");
        }

        return Results.Content(vkController.CreateField(num), "text/html");
    }
    
}