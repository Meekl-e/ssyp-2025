
using System.Xml.Linq;

public class Controller
{

    public static string CreateHtml(string search)
    {
        XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/search/{search}");
        XElement html = new XElement("div",
        xDB.Elements().Where(item => item.Attribute("type").Value == "http://fogid.net/o/person")
        .Select(person =>
        {
            string id = person.Attribute("id").Value;
            string name = person.Elements().Single(x => x.Attribute("prop").Value == "http://fogid.net/o/name").Value;
            return new XElement("a", new XAttribute("href", $"/person?id={id}"), $"{name}",
            new XElement("br"));
        }));

        return HtmlPage.GetField("", html.ToString());
    }

    public static string CreatePersonHtml(string id)
    {
        XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/tree/{id}");
        XElement html = new XElement("div",
        xDB.Elements().Single(x => x.Name.LocalName == "text")
            .Elements().Single(x => x.Name.LocalName == "v").Value,
        xDB.Elements().Single(x => x.Attribute("prop").Value == "participant")
            .Elements().Select(studio =>
        {
            string studioId = studio.Elements().Single(x => x.Attribute("prop").Value == "in-org")
            .Elements().Single(x => x.Attribute("tp").Value == "org-sys")
                .Attribute("id")
                    .Value;

            string studioName = studio.Elements().Single(x => x.Attribute("prop").Value == "in-org")
                .Elements().Single(x => x.Attribute("tp").Value == "org-sys")
                .Elements().Single(x => x.Attribute("prop").Value == "name")
                .Elements().Single(x => x.Name.LocalName == "v")
                    .Value;
            return new XElement("div",
            new XElement("a", new XAttribute("href", $"studio?id={studioId}"), $"{studioName}"), 
            new XElement("br"));
        }),
        xDB.Elements().Select(imgs =>
        {
            if (imgs.Attribute("prop").Value != "reflected")
            {
                return null;
            }
            return new XElement("div", imgs.Elements().Select(photo =>
            {
                string uri = photo.Elements().Single(x => x.Attribute("prop").Value == "in-doc")
                        .Elements().Single(x => x.Attribute("tp").Value == "photo-doc")
                        .Elements().Single(x => x.Attribute("prop").Value == "uri").Value;
                return new XElement("img", new XAttribute("src", $"http://syp.iis.nsk.su/SypWebApi/photo?uri={uri}&s=medium"));
            }));
        }));

        return HtmlPage.GetField("", html.ToString());
    }

    public static string CreateStudioHtml(string id)
    {
        XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/tree/{id}");
        XElement html = new XElement("div",
        xDB.Elements().Single(x => x.Attribute("prop").Value == "name")
            .Elements().Single(x => x.Name.LocalName == "v").Value,
        xDB.Elements().Single(x => x.Attribute("prop").Value == "in-org")
            .Elements().Select(person =>
        {
            string personId = person.Elements().Single(x => x.Attribute("prop").Value == "participant")
            .Elements().Single(x => x.Attribute("tp").Value == "person")
                .Attribute("id")
                    .Value;

            string personName = person.Elements().Single(x => x.Attribute("prop").Value == "participant")
                .Elements().Single(x => x.Attribute("tp").Value == "person")
                .Elements().Single(x => x.Attribute("prop").Value == "name")
                .Elements().Single(x => x.Name.LocalName == "v")
                    .Value;
            return new XElement("div",
            new XElement("a", new XAttribute("href", $"person?id={personId}"), $"{personName}"),
            new XElement("br"));
        }),
        xDB.Elements().Select(imgs =>
        {
            if (imgs.Attribute("prop").Value != "reflected")
            {
                return null;
            }
            return new XElement("div", imgs.Elements().Select(photo =>
            {
                string uri = photo.Elements().Single(x => x.Attribute("prop").Value == "in-doc")
                        .Elements().Single(x => x.Attribute("tp").Value == "photo-doc")
                        .Elements().Single(x => x.Attribute("prop").Value == "uri").Value;
                return new XElement("img", new XAttribute("src", $"http://syp.iis.nsk.su/SypWebApi/photo?uri={uri}&s=medium"));
            }));
        })
        // ,
        // xDB.Elements().Select(studios =>
        // {
        //     if (studios.Attribute("prop").Value == "org-parent")
        //     {
        //         return null;
        //     }
        //     return new XElement("div", studios.Elements().Select(studio =>
        //     {
        //         string studioId = studio.Elements().Single(x => x.Attribute("prop").Value == "org-child")
        //                 .Elements().Single(x => x.Attribute("tp").Value == "org-sys")
        //                 .Attribute("id").Value;
        //         return new XElement("a", new XAttribute("href", $"http://syp.iis.nsk.su/SypWebApi/xml/tree/{id}"),
        //         studio.Elements().Single(x => x.Attribute("prop").Value == "org-child")
        //                 .Elements().Single(x => x.Attribute("tp").Value == "org-sys")
        //                 .Elements().Single(x => x.Attribute("prop").Value == "name")
        //                 .Elements().Single(x => x.Name.LocalName == "v").Value);
        //     }));
        // }
        );

        return HtmlPage.GetField("", html.ToString());
    }
}