using System.Text.Json;

public class OldBaseController
{
    
    public static async Task<APIResults?> Read()
    {
        string spreadsheetId = "1NmIcu_vEI8ETwqaO1nHbaIykiko6vtB2tQaKTeQ7YiE";
        string range = "Sheet1!A:M";
        string apiKey = "AIzaSyDqDdzU6h-JUqxxLVXDeoL1ei9FZsj8IXA";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using HttpClient client = new();
        string json = await client.GetStringAsync(url);
        APIResults? result = JsonSerializer.Deserialize<APIResults>(json);

        return result;
    }

    public static string CreateField(int rawNum)
    {
        APIResults? results = Read().Result;
        if (results is null) return "";
        int rowsCount = results.values.Count;
        int num = rawNum % (rowsCount - 1);
        List<string> row = results.values[num + 1];
        string html = "";
        if (row.Count > 0)
        {
            html += @$"<div style='width:500px'>
                <i>{row[1]}.{row[3]}</i>
                {row[4]}<br>{row[5]}<br>
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
                        <style class='font-family: Calibri;'>
                            <td style='width:200px'>{row[7]}</th>
                            <td style='width:200px'>{row[8]}</td>
                            <td style='width:200px'>{row[9]}</td>
                        </style>
                        </tr>
                </table>
                </div><br>";
        }
        return html;
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
                html += $@"<div style='width:600px'>
                <li>
                <i>{row[1]}.{row[3]}</i>
                {row[4]}<br>{row[5]}<br>
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
                            <td style='width:200px'>{row[7]}</th>
                            <td style='width:200px'>{row[8]}</td>
                            <td style='width:200px'>{row[9]}</td>
                        </tr>
                </table>
                </li></div><br>";
            }
        }
        html += "</ul>";

        return HtmlPage.GetHtml("Данные старых мастерских", html);
    }
}
