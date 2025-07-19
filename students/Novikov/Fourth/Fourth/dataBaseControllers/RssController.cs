using System.Xml.Linq;

public class RssController
{
    XElement xDB;
    public RssController(string source)
    {
        xDB = XElement.Load(source);
    }
    public string CreateHtml()
    {
        XElement xHtml = new XElement("div",
        new XElement("ol",
        xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Select(post =>
        {
            if (post.Name.LocalName == "item")
            {
                bool inTag = false;
                return new XElement("li",
                new XElement("i", post.Elements().Single(x => x.Name.LocalName == "pubDate").Value),
                new XElement("br"),
                new XElement("div", new XAttribute("style", "width:700px"),
                new XText(post.Elements().Single(x => x.Name.LocalName == "title").Value),
                new XElement("br"),
                new XElement("a", new XAttribute("href", post.Elements().Single(x => x.Name.LocalName == "link").Value), post.Elements().Single(x => x.Name.LocalName == "description").Value.Where(XChar =>
                {
                    if (XChar == '<')
                    {
                        inTag = true;
                        return false;
                    }
                    else if (XChar == '>')
                    {
                        inTag = false;
                        return false;
                    }
                    else if (inTag)
                    {
                        return false;
                    }
                    return true;
                }))));
            }
            return null;
        })));
        return HtmlPage.GetHtml("cnews", xHtml.ToString());
    }
}