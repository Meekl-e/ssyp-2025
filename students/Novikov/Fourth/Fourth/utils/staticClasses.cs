using System.IO;
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
                     <a href='/'>На главную</a>
                     <br>
               {snippet}
               </body>
               </html>
               ";
    }

    public static string GetMainHtml(int vk, int tg, int oB, int cNews, int academC, int elementy)
    {

        return File.ReadAllText(Directory.GetCurrentDirectory()+ @"\wwwroot\htmls\index.html");
    }
}

public class APIResults
{
    public required List<List<string>> values { get; set; } 
}
