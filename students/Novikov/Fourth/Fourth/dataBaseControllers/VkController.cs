using System.Xml.Linq;
using System.Text;

public class VkController
{
    readonly XElement xDB;
    const string rdf = "{http://www.w3.org/1999/02/22-rdf-syntax-ns#}";


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

        return Results.Content(CreateHtml(start, step), "text/html");
    }

    public IResult GetFieldResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["num"], out int num))
        {
            num = 0;
            return Results.Redirect($"/vk_field?num={num}");
        }

        return Results.Content(CreateField(num), "text/html");
    }

    public string CreateField(int rawNum)
    {
        int postsCount = xDB.Elements().Where(x => x.Name.LocalName == "post").Count();
        int num = rawNum % postsCount;
        int absPostsCount = 0;
        XElement xHtml = new("div",
        xDB.Elements().Where(post =>
        {
            if (absPostsCount != num)
            {
                absPostsCount += 1;
                return false;
            }
            absPostsCount += 1;
            return true;
        }).Select(post =>
        {
            return new XElement(new XElement("div",
                        new XElement("i", new XAttribute("style", "color: gray"),
                        new XText(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(post.Elements().Single(x => x.Name.LocalName == "date").Value)).ToShortDateString())),
                        new XElement("br"),
                        new XText(ConvertBase64(post.Elements().Single(x => x.Name.LocalName == "text").Value)),
                        new XElement("br"),
                        xDB.Elements().Select(r =>
                        {
                            if (r.Name.LocalName == "media")
                            {
                                if (r.Elements().Single(x => x.Name.LocalName == "post").Attribute($"{rdf}resource")?.Value == post.Attribute($"{rdf}about")?.Value)
                                {
                                    return new XElement("img", new XAttribute("style", "width:465px;"), new XAttribute("src", ConvertBase64(r.Elements().Single(x => x.Name.LocalName == "url").Value)));
                                }
                                return null;
                            }
                            return null;
                        })));
        }));
        return HtmlPage.GetHtml1("", xHtml.ToString());
    }

    public string CreateHtml(int start, int step)
    {

        int absPostsCount = 0;
        int postsCount = 0;
        XElement xHtml = new("div",
        new XElement("a", new XAttribute("href", $"/vk?start={start - step}&step={step}"), "Назад"),
        new XElement("a", new XAttribute("href", $"/vk?start={start + step}&step={step}"), "Вперёд"),
        new XElement("ol",
            xDB.Elements().Select(post =>
            {
                if (absPostsCount <= start)
                {
                    absPostsCount += 1;
                    return null;
                }
                if (postsCount <= step)
                {
                    if (post.Name.LocalName == "post")
                    {
                        postsCount += 1;
                        return new XElement("li", new XAttribute("style", "width: 500px"),
                        new XElement("i", new XAttribute("style", "color: gray"),
                        new XText(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(post.Elements().Single(x => x.Name.LocalName == "date").Value)).ToShortDateString())),
                        new XElement("br"),
                        new XText(ConvertBase64(post.Elements().Single(x => x.Name.LocalName == "text").Value)),
                        new XElement("br"),
                        xDB.Elements().Select(r =>
                        {
                            if (r.Name.LocalName == "media")
                            {
                                if (r.Elements().Single(x => x.Name.LocalName == "post").Attribute($"{rdf}resource")?.Value == post.Attribute($"{rdf}about")?.Value)
                                {
                                    return new XElement("img", new XAttribute("src", ConvertBase64(r.Elements().Single(x => x.Name.LocalName == "url").Value)));
                                }
                                return null;
                            }
                            return null;
                        }));
                    }
                    return null;
                }
                return null;
            })),
        new XElement("a", new XAttribute("href", $"/vk?start={start - step}&step={step}"), "Назад"),
        new XElement("a", new XAttribute("href", $"/vk?start={start + step}&step={step}"), "Вперёд"));
        return HtmlPage.GetHtml("База ВКонтакте", xHtml.ToString());
    }

    public static string ConvertBase64(string? text)
    {
        if (text == null)
        {
            return String.Empty;
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(text));
    }

    public VkController()
    {
        xDB = XElement.Load("data.fog");
    }
}