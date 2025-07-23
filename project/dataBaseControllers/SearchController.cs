using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

public class SearchController
{
    public static string ConvertJson(string json)
    {
        var doc1 = JsonConvert.DeserializeXmlNode(json, "root");
        XDocument xDoc = XDocument.Load(new XmlNodeReader(doc1));
        XElement xml = XElement.Parse(xDoc.ToString());
        XElement html = new XElement("ul",
        xml.Elements().Select(database =>
        {
            if (database.Elements().Any())
            {
                return database.Elements().Select(post =>
                {
                    if (!post.Elements().Any())
                    {
                        return new XElement("li",
                        post.Value, new XElement("br"));
                    }
                    return new XElement("li", new XElement("a", new XAttribute("href", $"/{post.Elements().Single(x => x.Name.LocalName == "name_db").Value}_field?num={post.Elements().Single(x => x.Name.LocalName == "id").Value}"),
                    new XElement("div", post.Elements().Single(x => x.Name.LocalName == "description"), new XElement("br")))
                    );
                });
            }
            return null;
        }));
        return HtmlPage.GetHtml("", html.ToString());
        // XElement html = xml;
        // return html.ToString();
    }
}