using System.Text.Json;

public class OldBaseReader
{
    public static IResult GetResult(HttpRequest request)
    {
        if (!int.TryParse(request.Query["year"], out int year))
        {
            year = 2017;
        }
        if (year > 2017)
        {
            return Results.Redirect("/old_base?year=2017");
        }

        return Results.Content(CreateHtml(year), "text/html");
    }

    public static async Task<APIResults?> Read()
    {
        string spreadsheetId = "1NmIcu_vEI8ETwqaO1nHbaIykiko6vtB2tQaKTeQ7YiE";
        string range = "Sheet1!A:M";
        string apiKey = "AIzaSyDqDdzU6h-JUqxxLVXDeoL1ei9FZsj8IXA";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using HttpClient client = new HttpClient();
        string json = await client.GetStringAsync(url);

        APIResults? result = JsonSerializer.Deserialize<APIResults>(json);

        return result;
    }
    public static string CreateHtml(int year)
    {
        APIResults? results = Read().Result;
        if (results is null) return "";
        string html = $@"<a href='/old_base?year={year - 1}'>Назад</a>
        <a href='/old_base?year={year + 1}'>Вперёд</a>
        <ul>";
        foreach (List<string> row in results.values)
        {
            for (int i = 1; i < 13; i++)
            {
                if (row[i] == "NoneValue")
                {
                    row[i] = "";
                }
            }
            if (row.Count > 0 && row[1] != "год" && row[1] != "" && row[1].Split(".")[0] == year.ToString())
            {
                html += $@"<div style='width: 500px'><li><i>{row[1]}.{row[3]}</i> {row[4]}<br>{row[5]}<br>

<table>
<thead>
    <tr>
      <th scope='col'></th>
      <th scope = 'col'></th>
           <th scope = 'col'></th>
          </tr>
        </thead>
<tbody>
    <tr>
      <td>{row[7]}</th>
      <td>{row[8]}</td>
         <td>{row[9]}</td>
       </tr>
         </table>
</div></li><br>";
            }
        }

        html += "</ul>";

        return HtmlPage.GetHtml("Данные старых мастерских", html);
    }
}