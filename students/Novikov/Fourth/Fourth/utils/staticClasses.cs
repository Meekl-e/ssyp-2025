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

    public static string GetMainHtml(string vk, string tg, string oB, string cNews, string academC, string elementy)
    {
        return File.ReadAllText(@"C:\Users\programmer\Documents\Pirogov_Anton\ssyp-2025\students\Novikov\Fourth\Fourth\utils\index.html");
    }
}

public class APIResults
{
    public required List<List<string>> values { get; set; } 
}
