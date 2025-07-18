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
                     <meta charset = 'UTF-16'>
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
public class CNController
{
    XElement xDB;
    public CNController()
    {
        xDB = XElement.Load("https://www.cnews.ru/inc/rss/news.xml");
    }
    public string CreateHtml()
    {
        XElement xHtml = new XElement("div",
        new XElement("ol",
        xDB.Elements().Single(x => x.Name.LocalName == "channel").Elements().Select(post =>
        {
            if (post.Name.LocalName == "item")
            {
                return new XElement("li",
                new XElement("i", post.Elements().Single(x => x.Name.LocalName == "pubDate").Value),
                new XElement("br"),
                new XElement("div", new XAttribute("style", "width:700px"),
                new XText(post.Elements().Single(x => x.Name.LocalName == "title").Value)),
                new XElement("br"));
            }
            return null;
        })));
        return HtmlPage.GetHtml("Cnews", xHtml.ToString());
    }

}

public class VKController
{
    XElement xDB;
    const string rdf = "{http://www.w3.org/1999/02/22-rdf-syntax-ns#}";
    public string CreateHtml(int start, int step)
    {
        XElement xHtml = new XElement("div",
        new XElement("a", new XAttribute("href", $"/vk?start={start - step}&step={step}"), "Назад"),
        new XElement("a", new XAttribute("href", $"/vk?start={start + step}&step={step}"), "Вперёд"),
        new XElement("ol",
            xDB.Elements().Select(post =>
            {
                if (post.Name.LocalName == "post")
                {
                    if (Int32.Parse(post.Attribute($"{rdf}about").Value) >= start && Int32.Parse(post.Attribute($"{rdf}about").Value) < start + step)
                    {
                        bool emptyPost = post.Elements().Single(x => x.Name.LocalName == "text").Value == "";
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
                                if (r.Elements().Single(x => x.Name.LocalName == "post").Attribute($"{rdf}resource").Value == post.Attribute($"{rdf}about").Value)
                                {
                                    return new XElement("img", new XAttribute("src", ConvertBase64(r.Elements().Single(x => x.Name.LocalName == "url").Value)));
                                }
                                return null;
                            }
                            return null;
                        }));
                    }
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
        return Encoding.UTF8.GetString(Convert.FromBase64String(text)); // ����������
    }

    public VKController()
    {
        xDB = XElement.Load("data.fog");
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
