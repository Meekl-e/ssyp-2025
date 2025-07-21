using System.Xml.Linq;

public class RssController
{

    XElement xDB;
    public RssController(string source)
    {
        xDB = XElement.Load(source);
    }

    public string CreateField(int rawNum)
    {
        int itemsCount = xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Where(x => x.Name.LocalName == "item").Count();
        int num = rawNum % itemsCount;
        int absItemsCount = 0;
        XElement xHtml = new("div",
        xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Where(x => x.Name.LocalName == "item").Where(item =>
        {
            if (absItemsCount != num)
            {
                absItemsCount += 1;
                return false;
            }
            absItemsCount += 1;
            return true;
        }).Select(item =>
        {
            bool inTag = false;
            return new XElement("div",
            new XElement("i", item.Elements().Single(x => x.Name.LocalName == "pubDate").Value),
            new XElement("br"),
            new XElement("div", new XAttribute("style", "width:700px"),
            new XText(item.Elements().Single(x => x.Name.LocalName == "title").Value),
            new XElement("br"),
            new XElement("a", new XAttribute("href", item.Elements().Single(x => x.Name.LocalName == "link").Value), item.Elements().Single(x => x.Name.LocalName == "description").Value.Where(xChar =>
            {
                if (xChar == '<')
                {
                    inTag = true;
                    return false;
                }
                else if (xChar == '>')
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
        }));

        return xHtml.ToString();
    }


    public string CreateHtml()
    {
        XElement xHtml = new XElement("div",
        new XElement("ol",
        xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Select(item =>
        {
            if (item.Name.LocalName == "item")
            {
                bool inTag = false;
                return new XElement("li",
                new XElement("i", item.Elements().Single(x => x.Name.LocalName == "pubDate").Value),
                new XElement("br"),
                new XElement("div", new XAttribute("style", "width:700px"),
                new XText(item.Elements().Single(x => x.Name.LocalName == "title").Value),
                new XElement("br"),
                new XElement("a", new XAttribute("href", item.Elements().Single(x => x.Name.LocalName == "link").Value), item.Elements().Single(x => x.Name.LocalName == "description").Value.Where(XChar =>
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