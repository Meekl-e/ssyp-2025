using System.Text.Json;
using System.Xml.Linq;

public class RssController
{

    XElement xDB;
    List<DefaultObject> docs_to_search;

    private string name_source = "";
    public WordsSearcher<string, int> searcher;
    public RssController(string source, string name_source)
    {
        this.name_source = name_source;
        xDB = XElement.Load(source);
        bool inTag = false;
        int id_cont = 0;
        this.docs_to_search = xDB.Elements()
        .Single(x => x.Name.LocalName == "channel")
        .Elements()
        .Where(x => x.Name.LocalName == "item")
        .Select(el => new DefaultObject(name_source){description=el.Element("description").Value, id=id_cont++}).ToList();
        DataSourceList dsl = new DataSourceList([.. docs_to_search.Select(o => o.description)]);
        this.searcher = new WordsSearcher<string, int>(dsl);
        Console.WriteLine($"RSS {source} Loaded");
    }

     public string Search(string[] query_search)
    {
        var search_result = searcher.SearchForKey(query_search).Select(x => docs_to_search[x.Item1]);

        return JsonSerializer.Serialize(search_result);
    }


    public int GetCount()
    {
        return xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Where(x => x.Name.LocalName == "item").Count();
    }

    public string CreateField(int num, bool onMainPage)
    {
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
            string time = item.Elements().Single(x => x.Name.LocalName == "pubDate").Value.ToString();
            bool inTag = false;
            return new XElement("div",
            new XElement("i", time.Substring(0, time.Length - 6)),
            new XElement("br"),
            new XElement("div", new XAttribute("class", "content12"),
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
        if (onMainPage)
        {
            return xHtml.ToString();
        }
        return HtmlPage.GetHtml("Rss", xHtml.ToString());
    }

    


    public string CreateHtml()
    {
        XElement xHtml = new XElement("div",
        new XElement("ol",
        xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Select(item =>
        {
            if (item.Name.LocalName == "item")
            {
                string time = item.Elements().Single(x => x.Name.LocalName == "pubDate").Value;
                bool inTag = false;
                return new XElement("li",
                new XElement("i", time.Substring(0, time.Length - 6)),
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