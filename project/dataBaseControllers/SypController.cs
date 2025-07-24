using System.Xml.Linq;

public class Controller
{

    public static string CreateHtml(string search)
    {
        try
        {
            XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/search/{search}");
            XElement html = new XElement("div", new XAttribute("class", "ForController"),
            xDB.Elements().Select(item =>
            {
                if (item.Attribute("type").Value == "http://fogid.net/o/person")
                {
                    string id = item.Attribute("id").Value;
                    string name = item.Elements().Single(x => x.Attribute("prop").Value == "http://fogid.net/o/name").Value;
                    return new XElement("a", new XAttribute("href", $"/person?id={id}"), $"{name}",
                    new XElement("br"));
                }
                if (item.Attribute("type").Value == "http://fogid.net/o/org-sys")
                {
                    string id = item.Attribute("id").Value;
                    string name = item.Elements().Single(x => x.Attribute("prop").Value == "http://fogid.net/o/name").Value;
                    return new XElement("a", new XAttribute("href", $"/studio?id={id}"), $"{name}",
                    new XElement("br"));
                }
                return null;
            })
            );

            return HtmlPage.GetHtml("Поиск", html.ToString());
        }
        catch
        {
            return HtmlPage.GetHtml("Ошибка", "Ошибка поиска");
        }
    }

    public static string CreatePersonHtml(string id)
    {
        try
        {
            XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/tree/{id}");
            XElement html = new XElement("div", new XAttribute("class", "ForController"),
            xDB.Elements().Single(x => x.Attribute("prop").Value == "name")
                .Elements().Single(x => x.Name.LocalName == "v").Value,
            xDB.Elements().Select(x =>
            {
                if (x.Attribute("prop").Value == "participant")
                {
                    return x.Elements().Select(studio =>
                    {
                        XElement org = studio.Elements().Single(x => x.Attribute("prop").Value == "in-org");
                        XElement sys = org.Elements().Single(x => x.Attribute("tp").Value == "org-sys");
                        string studioId = sys.Attribute("id").Value;

                        XElement orgName = sys.Elements().Single(x => x.Attribute("prop").Value == "name");
                        string studioName = orgName.Elements().Single(x => x.Name.LocalName == "v").Value;

                        return new XElement("div",
                        new XElement("a", new XAttribute("href", $"studio?id={studioId}"), $"{studioName}"),
                        new XElement("br"));
                    });
                }
                return null;
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

            return HtmlPage.GetHtml("Пользователь", html.ToString());
        }
        catch
        {
            return HtmlPage.GetHtml("Ошибка", "Человек не найден");
        }
    }

    public static string CreateStudioHtml(string id)
    {
        try
        {
            XElement xDB = XElement.Load($"http://syp.iis.nsk.su/SypWebApi/xml/tree/{id}");
            XElement html = new XElement("div", new XAttribute("class", "ForController"),
            xDB.Elements().Single(x => x.Attribute("prop").Value == "name")
                .Elements().Single(x => x.Name.LocalName == "v").Value,
            xDB.Elements().Single(x => x.Attribute("prop").Value == "in-org")
                .Elements().Select(person =>
            {
                if (person.Elements().Single(x => x.Attribute("prop").Value == "participant")
                        .Elements().Where(x => x.Attribute("tp").Value == "person").Any())
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
                }
                return null;
            }),
            xDB.Elements().Select(imgs =>
            {
                if (imgs.Attribute("prop").Value != "reflected")
                {
                    return null;
                }
                return new XElement("div", imgs.Elements().Select(photo =>
                {
                    if (photo.Elements().Single(x => x.Attribute("prop").Value == "in-doc")
                                .Elements().Where(x => x.Attribute("tp").Value == "photo-doc").Any())
                    {
                        string uri = photo.Elements().Single(x => x.Attribute("prop").Value == "in-doc")
                                .Elements().Single(x => x.Attribute("tp").Value == "photo-doc")
                                .Elements().Single(x => x.Attribute("prop").Value == "uri").Value;
                        return new XElement("img", new XAttribute("src", $"http://syp.iis.nsk.su/SypWebApi/photo?uri={uri}&s=medium"));
                    }
                    return null;
                }));
            }));

            return HtmlPage.GetHtml("Организация", html.ToString());
        }
        catch
        {
            return HtmlPage.GetHtml("Ошибка", "Организация не найдена");
        }
    }
}