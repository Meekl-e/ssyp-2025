### Wikipedia APi
В прошлый раз мы создали собственный API.  

Давайте в этот раз попробуем считать API Википедии. 

Воспользуемся нашим кодом, полученным на прошлом уроке и слегка его модернизируем

```
app.MapGet("/wikipedia/", async (HttpRequest request) =>
{
    string? search = request.Query["search"];
    
    using (HttpClient client = new HttpClient()) // безопасно используем клиент
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaSearchApp/1.0 (contact@example.com)"); // Заголовки запроса (необходимы для авторизации)

        string url = $"https://ru.wikipedia.org/w/api.php?" + // адрес API
                     $"action=query&" + // действие - запрос
                     $"list=search&" + // тип - поиск
                     $"srsearch={Uri.EscapeDataString(search)}&" + // запрос в безопасном формате
                     $"utf8=1&" + // кодировка ответа
                     $"format=json&" + // формат ответа
                     $"srlimit=1"; // лимит поиска
        
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync(); // считываем результат
        Console.WriteLine(json);
        return Results.Content(json, "text/json");
    }
});
```
Основное отличие в том, что мы по-другому используем `HttpClient client = new HttpClient()`, а также используем параметры запроса API Википедии.   
Если все сделано правильно, то на странице `http://localhost:5042/wikipedia/?search=Арбуз` (замените `5042` на свой порт) появится json текст об арбузах

```
{
  "batchcomplete": "",
  "continue": {
    "sroffset": 1,
    "continue": "-||"
  },
  "query": {
    "searchinfo": {
      "totalhits": 1958
    },
    "search": [
      {
        "ns": 0,
        "title": "Арбуз",
        "pageid": 95542,
        "size": 51470,
        "wordcount": 2957,
        "snippet": "\u003Cspan class=\"searchmatch\"\u003EАрбу́з\u003C/span\u003E обыкнове́нный (лат. Citrúllus lanátus), или \u003Cspan class=\"searchmatch\"\u003Eарбу́з\u003C/span\u003E шерсти́стый, или \u003Cspan class=\"searchmatch\"\u003Eарбу́з\u003C/span\u003E столо́вый — однолетнее травянистое растение, вид рода \u003Cspan class=\"searchmatch\"\u003EАрбуз\u003C/span\u003E (Citrullus)",
        "timestamp": "2025-06-20T22:27:04Z"
      }
    ]
  }
}
```
Что же делать дальше?   

Можно получить `snippet`, преобразовав ответ в формат словаря. Для этого необходимо создать несколько классов. Как можно заметить, желаемый сниппет находится в списке результатов (`search`), а список резлуьтатов находится в `Query`. Значит, чтобы получить его значение нам надо получить `query`, из `query` получить список `search`, выбрать объект и получить `snippet`. Теперь давайте реализуем это с помощью классов. Почему с помощью классов? Именно с помощью классов C# понимает, как мы должны представлять объект. Создадим это в другом файле, не в Program.cs
```
public class WikiSearchResult
{
    public Query query { get; set; }  // Тот самый query. Важно называть имена переменных также, как они указаны в словаре.
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
``` 
У вас может возникунть вопрос, зачем добавлять методы `get; set`? Метод `get` позволяет брать значение из переменной, а метод `set` - устанавливать его.  
Теперь мы можем обратиться к Query.search[0].snippet. Допишем нашу программу, чтобы мы могли работать с ответом. 

```
app.MapGet("/wikipedia/", async (HttpRequest request) =>
{
    
    string? search = request.Query["search"];
    
    using (HttpClient client = new HttpClient()) // безопасно используем клиент
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaSearchApp/1.0 (contact@example.com)"); // Заголовки запроса (необходимы для авторизации)

        string url = $"https://ru.wikipedia.org/w/api.php?" + // адрес API
                     $"action=query&" + // действие - запрос
                     $"list=search&" + // тип - поиск
                     $"srsearch={Uri.EscapeDataString(search)}&" + // запрос в безопасном формате
                     $"utf8=1&" + // кодировка ответа
                     $"format=json&" + // формат ответа
                     $"srlimit=1"; // лимит поиска
        
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        WikiSearchResult result = JsonSerializer.Deserialize<WikiSearchResult>(json); // Получаем объект
        return Results.Content(result.query.search[0].snippet, "text/html"); // Отправляем ответ
    }
});
```
Но теперь у нас непонятные символы вместо нашей информации об арбузах. Для исправления этого добавим html разметку. Создадим функцию, которая будет создавать небольшую страничку. Также не в Program.cs
```
public static class HtmlPage
{
    public static string GetHtml(string title, string snippet)
    {
        return $"""
               <!DOCTYPE html>
               <html lang="en">
               <head>
                   <meta charset="UTF-8">
                   <title>{title}</title>
               </head>
               <body>
               {snippet}
               </body>
               </html>
               """;
    }
}
```
Теперь финальный код нашей страницы выглядит так:
```
app.MapGet("/wikipedia/", async (HttpRequest request) =>
{
    
    string? search = request.Query["search"];
    
    using (HttpClient client = new HttpClient()) // безопасно используем клиент
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaSearchApp/1.0 (contact@example.com)"); // Заголовки запроса (необходимы для авторизации)

        string url = $"https://ru.wikipedia.org/w/api.php?" + // адрес API
                     $"action=query&" + // действие - запрос
                     $"list=search&" + // тип - поиск
                     $"srsearch={Uri.EscapeDataString(search)}&" + // запрос в безопасном формате
                     $"utf8=1&" + // кодировка ответа
                     $"format=json&" + // формат ответа
                     $"srlimit=1"; // лимит поиска
        
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        WikiSearchResult result = JsonSerializer.Deserialize<WikiSearchResult>(json);
        string title = result.query.search[0].title; // получаем название
        string snippet = result.query.search[0].snippet; // получаем сниппет
        return Results.Content( HtmlPage.GetHtml(title, snippet), "text/html"); // красиво выводим
    }
});
```


