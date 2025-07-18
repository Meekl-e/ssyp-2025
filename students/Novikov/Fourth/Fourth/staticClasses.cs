
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
                     <a href='/'>На главную</a>
                     <br>
               {snippet}
               </body>
               </html>
               ";
    }
}

public class APIResults
{
    public required List<List<string>> values { get; set; } 
}
