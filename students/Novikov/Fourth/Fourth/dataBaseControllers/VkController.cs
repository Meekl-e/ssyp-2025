using System.Xml.Linq;
using System.Text;

public class VkController
{
    
    readonly XElement xDB;
    const string rdf = "{http://www.w3.org/1999/02/22-rdf-syntax-ns#}";

    public List<int> GetIds()
    {
       
        List<int> ids = xDB.Elements().Where(post =>
        post.Name.LocalName == "post")
        .Select(post => Int32.Parse(post.Attribute($"{rdf}about")?.Value)).ToList();
        return ids;
    }

    public string CreateField(int id)
    {
        XElement xHtml = new("div",
        xDB.Elements().Where(post => post.Name.LocalName == "post")
        .Where(post =>
        {
            if (post.Attribute($"{rdf}about")?.Value == id.ToString())
            {
                return true;
            }
            return false;
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
        return HtmlPage.GetField("", xHtml.ToString());
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
        xDB = XElement.Load("datasets/data.fog");
    }
}