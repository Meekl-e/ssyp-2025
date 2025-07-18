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
        return $"""
               < !DOCTYPE html >
 
                < html lang = "en" >
  
                 < head >
  
                     < meta charset = "UTF-8" >
   
                      < title >{ title}</ title >
      
                     </ head >
      
                     < body >
               { snippet}
               </ body >
               </ html >
               """;
    }
}

public class WikiSearchResult
{
    public Query query { get; set; }  // “от самый query. ¬ажно называть имена переменных также, как они указаны в словаре.
}

public class Query
{
    public List<SearchResult> search { get; set; } // список search
}

public class SearchResult
{
    public string title { get; set; } // и название захватим 
    public string snippet { get; set; } // желанный snippet 
}