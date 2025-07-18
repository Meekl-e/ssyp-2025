using System.Xml.Linq;
using System;
using System.Text.Json;
using System.Text;


public static class Users
{
    public static string GetUser(int id)
    {
        if (id == 1234)
        {
            return "Ivan";
        }
        else if (id == 1233)
        {
            return "Vasya";
        }
        
        return "";
    }
}

public static class HtmlPage
{
    public static string GetHtml(string title, string snippet)
    {
        return @$"
               <!DOCTYPE html>
                <html lang = 'ru'>
                 <head>
                     <meta charset = 'UTF-8'>
                      <title>{title}</title>
                     </head>
                     <body>
               {snippet}
               </body>
               </html>
               ";
    }
}

public class WikiSearchResult
{
    public Query query { get; set; }  // ��� ����� query. ����� �������� ����� ���������� �����, ��� ��� ������� � �������.
}

public class Query
{
    public List<SearchResult> search { get; set; } // ������ search
}

public class SearchResult
{
    public string title { get; set; } // � �������� �������� 
    public string snippet { get; set; } // �������� snippet 
}

public class Post
{
    public DateTime date { get; set; }
    public int id { get; set; }
    public string text { get; set; } = String.Empty;
    public string owner { get; set; } = String.Empty;
    public List<Media> media { get; set; } = new List<Media>(); // ������ �����-�������, ������������� � ����� �����
}
public class Media
{
    public string id { get; set; } = String.Empty;
    public int post_id { get; set; } // ID �����, � �������� �������� ������ ����� ����
    public int width { get; set; }
    public int height { get; set; }
    public string uri { get; set; } = String.Empty;
    public string url { get; set; } = String.Empty;

}


public class Controller
{
    //const int page = 10;
    // List<Post> posts = new List<Post>();
    // List<Media> medias = new List<Media>();
    XElement xDB;
    public string CreateHtml(int start, int step)
    {
        // if (start < 0) start = 0;
        // if (finish < 10) finish = 10;

        XElement xHtml = new XElement("div",
        new XElement("a", new XAttribute("href", $"/vk?start={start - page}&finish={finish - page}")),
        new XElement("a", new XAttribute("href", $"/vk?start={start + page}&finish={finish + page}")),
            //new XElement("div", new XAttribute("style", "width: 500px"),
            xDB.Elements().Select(post =>
            {
                if (post.Name.LocalName == "post")
                {
                    return new XElement("li",  new XAttribute("style", "width: 500px"),
                    new XElement("i", new XAttribute("style", "color: gray"),
                    new XText(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(post.Elements().Single(x => x.Name.LocalName == "date").Value)).ToShortDateString())),
                    new XElement("br"),
                    new XText(ConvertBase64(post.Elements().Single(x => x.Name.LocalName == "text").Value)),
                    new XElement("br"),
                    xDB.Elements().Select(r =>
                    {
                        if (r.Name.LocalName == "media")
                        {
                            if (r.Elements().Single(x => x.Name.LocalName == "post").Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}resource").Value == post.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value)
                            {
                                return new XElement("img", new XAttribute("src", ConvertBase64(r.Elements().Single(x => x.Name.LocalName == "url").Value)));
                            }
                            return null;
                        }
                        return null;
                    }));
                }
                else
                {
                    return null;
                }
            }));
        return HtmlPage.GetHtml("sss", xHtml.ToString());
            // .Where(x => x.Name.LocalName == "post").Select(post=> new XElement("li", new XAttribute("style", "width: 500px"),
        // new XElement("i", new XAttribute("style", "color: gray"),
        // new XText(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(post.Elements().Single(x => x.Value == "date").Value)).ToShortDateString())),
        // new XElement("br"),
        // new XText(ConvertBase64(post.Elements().Single(x => x.Value == "text").ToString()))),
        // new XElement("br"),
        // xDB.Elements().Where(x => x.Name.LocalName == "media").Select(media => media.Elements().Single(x => x.Name.LocalName == "post").Attribute("rdf:resource").Value == post.Attribute("rdf:about"))
        // )));


        // new XElement("i", new XAttribute("style", "color: gray"),
        // .Select(x => x.Elements().Where(x => x.Value == "date").Select(x => new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(x.ToString()))))),
        // new XElement("br"),
        // xDB.Elements().Where(x => x.Value == "post").Select(x => x.Elements().Where(x => x.Value =="text").Select(x => ConvertBase64(x.ToString()))),
        // new XElement("br"),
        // xDB.Elements().Where(x => x.Value == "media").Where(x => x.Elements().Where(x => x.Value == "post").Where(x => x.Attribute))));


//         string html = @$"
//                      <a href='/vk?start={start - 10}&finish={finish - 10}'>�����</a>
// <a href='/vk?start={start + 10}&finish={finish + 10}'>�����</a><ol>
//                             ";
//         return "";
        /*
        for (int i = start; i < finish; i++)
        //foreach (Post post in posts)
        {
            Post post = posts[i];
            html += $"<li><div style='width: 500px'><i style='color: gray'>{post.date}</i><br>{post.text} </div><br> ";
            foreach (Media media in post.media) // ��������� ����������
            {
                html += $"<img src=\"{media.url}\" />";
            }

            html += "</li>";
        }

        html += @$"</ol><a href='/vk?start={start - 10}&finish={finish - 10}'>�����</a>
<a href='/vk?start={start + 10}&finish={finish + 10}'>�����</a>";
        return HtmlPage.GetHtml("����� ���������", html);
        */
    }
    

    public static string ConvertBase64(string? text)
    {
        if (text == null)
        {
            return String.Empty;
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(text)); // ����������
    }
    public Controller()
    {
        xDB = XElement.Load("data.fog"); // ��������� ���� ������
        foreach (XElement xElement in xDB.Elements()) // ����������� �� ��������
        {
            if (xElement.Name.LocalName == "post") // ���� ������ <post></post>
            {
                int idFromBD =
                    int.Parse(xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about")
                        .Value); // ����� �������� �������� rdf:about
                string text = xElement.Element("text").Value; // ����� �������� ����������� �������� <text></text>. 
                int date = int.Parse(xElement.Element("date")
                    .Value); // ����� �������� ����������� �������� <date></date>. 
                string
                    owner = xElement.Element("owner_id")
                        .Value; // ����� �������� ����������� �������� <owner_id></owner_id>. 

                posts.Add(new Post()
                {
                    date = new DateTime(1970, 1, 1).AddSeconds(date),
                    id = idFromBD,
                    owner = owner,
                    text = ConvertBase64(text)
                }); // ��������� � ������
            }

            if (xElement.Name.LocalName == "media")
            {
                string idFromBD = (xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value);
                string url = xElement.Element("url").Value;
                int post_id = int.Parse(idFromBD.Split('-')[0]); // ����� ID �����
                string uri = xElement.Element("uri").Value; // ����� ���� �����
                int width = int.Parse(xElement.Element("width").Value); // ������ ��������
                int height = int.Parse(xElement.Element("height").Value); // ������ ��������
                Media media = new Media()
                { uri = uri, width = width, height = height, post_id = post_id, id = idFromBD, url = ConvertBase64(url) }; // ������� ������
                foreach (Post p in posts) // ���� ����, � �������� �������� ������ ����
                {
                    if (p.id == post_id)
                    {
                        p.media.Add(media);
                        break;
                    }
                }

                medias.Add(media);
            }
        }
    }
}

public class GoogleSheetsReader
{
    public static string GenerateImage(string file_id)
    {
        if (file_id == "None") return ""; // �������� �� �������
        return $"<img src=\"https://drive.google.com/thumbnail?authuser=0&id={file_id}&sz=w1000\" />"; // ������� �����������
    }
    public static async Task<APIResults?> Read()
    {
        string spreadsheetId = "12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI";
        string range = "����1!A:F";
        string apiKey = "AIzaSyDqDdzU6h-JUqxxLVXDeoL1ei9FZsj8IXA";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using (HttpClient client = new HttpClient())
        {
            string json = await client.GetStringAsync(url);

            APIResults result = JsonSerializer.Deserialize<APIResults>(json);

            return result;

        }
    }
    public static string CreateHtml()
    {
        APIResults? googleResults = Read().Result;
        if (googleResults is null) return "";
        string html = "<ul";
        foreach (List<string> row in googleResults.values)
        {
            if (row.Count == 6 && row[0] != "ID")
            {
                DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1]));
                html += $"<li>{row[5]} <i>{time.Date}</i><br>{GenerateImage(row[3])}</li>"; // ����������
            }
        }

        html += "</ul>";

        return HtmlPage.GetHtml("�������� ����� ����", html);
    }
}
public class APIResults
{
    public List<List<string>> values { get; set; } // � ���� ��� �� ������, ��� ��� ��������� ���� values 
}

public class OldBaseReader
{
    public static async Task<APIResults?> Read()
    {
        string spreadsheetId = "1NmIcu_vEI8ETwqaO1nHbaIykiko6vtB2tQaKTeQ7YiE";
        string range = "Sheet1!A:M";
        string apiKey = "AIzaSyDqDdzU6h-JUqxxLVXDeoL1ei9FZsj8IXA";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using (HttpClient client = new HttpClient())
        {
            string json = await client.GetStringAsync(url);

            APIResults result = JsonSerializer.Deserialize<APIResults>(json);

            return result;

        }
    }
    public static string CreateHtml()
    {
        APIResults? results = Read().Result;
        if (results is null) return "";
        string html = "<ul>";
        foreach (List<string> row in results.values)
        {
            for (int i = 1; i < 13; i++)
            {
                if (row[i] == "NoneValue")
                {
                    row[i] = "";
                }
            }
            if (row.Count > 0 && row[1] != "���" && row[1] != "")
            {
                html += $@"<div style='width: 500px'><li><i>{row[1]}.{row[3]}</i> {row[4]}<br>{row[5]}<br>

<table>
<thead>
    <tr>
      <th scope='col'>����������</th>
      <th scope = 'col'>��������������</th>
           <th scope = 'col'>������</th>
          </tr>
        </thead>
<tbody>
    <tr>
      <td>{row[7]}</th>
      <td>{row[8]}</td>
         <td>{row[9]}</td>
       </tr>
         </table>
</div></li>"; // ����������
            }
        }

        html += "</ul>";

        return HtmlPage.GetHtml("������ ���� ������ ����", html);
    }
}
