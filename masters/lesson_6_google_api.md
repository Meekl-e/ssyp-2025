### Подключение к Google API


Давайте создадим класс `GoogleSheetsReader`, в котором реализуем логику чтения из таблиц. 

Чтобы это осуществить нам необходимы: ключ api google таблиц, id таблицы, а также диапазон интересующих нас значений. 

1. Ключ API можно получить на сайте Google. Для этого:
    1. Перейдите в Google Cloud консоль [https://console.cloud.google.com/](https://console.cloud.google.com/)
    2. Создайте приложение (в левом верхнем углу выберите `Select a project` => `New project` => Заполните имя => `Create`)
    3. "Войдите" в приложение (в левом верхнем углу выберите `Select a project` => имя вашего проекта)
    4. Нажмите на `APIs & Services` => `+ Enable APIs and services` (наверху) => в поиске вбиваем `sheets` => нажимаем на `Google Sheets API` => `Enable` 
    5. Заходим в `Credentials` => `+ Create credentials` (наверху) => `API_KEY` => сохраните ключ
    6. Ограничим доступ по нашему API ключу. В `Credentials` => `API key 1` => `Restrict key` => Выбираем `Google Sheets API` => `OK` => `Save`
2. ID таблицы мы знаем из URL адреса: URL: `https://docs.google.com/spreadsheets/d/12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI/` => ID = `12OhmW7UWUHXsOi1mrSyczMs2UBHHcoPKnJ1pt3sFGAI`
3. Диапазон нам также изввестен: `Лист1!A:F`

Итак, когда все есть, можно создавать класс
```
public class GoogleSheetsReader
{
    public static async Task<string> Read() // Особенности асинхронности, возвращаем Task<string>
    {
        string spreadsheetId = "<ID таблицы>";
        string range = "<Диапазон>";
        string apiKey = "<API ключ>";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}"; // создаем ссылку

       using (HttpClient client = new HttpClient())
        {
            string json = await client.GetStringAsync(url); // получаем строку. Теперь снова необходимо из строки создать класс.
            
            return json;
        
        }
    }
}
```
Давайте добавим вспомогательный класс для десериализации
```
public class GoogleResults
{
    public List<List<string>> values { get; set; } // В этот раз он меньше, при том сохраняем лишь values 
}
```
Закончим функцию `Read()`
```
public class GoogleSheetsReader
{
    public static async Task<GoogleResults?> Read()
    {
        string spreadsheetId = "<ID таблицы>";
        string range = "<Диапазон>";
        string apiKey = "<API ключ>";

        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using (HttpClient client = new HttpClient())
        {
            string json = await client.GetStringAsync(url);

            GoogleResults result = JsonSerializer.Deserialize<GoogleResults>(json);
            
            return result;
        
        }
    }
}
```
Теперь мы получили данные, дальше необходимо их как-то обрабатывать. Давайте напишем функцию, которая из списка чисел сделает нам список html.
```
public static string CreateHtml()
    {
        GoogleResults? googleResults =Read().Result;
        if (googleResults is null) return ""; // Проверям на "пустоту"
        string html = """
                      <!DOCTYPE html>
                      <html lang="ru">
                      <head>
                          <meta charset="UTF-8">
                          <title>Телеграм канал ЛШЮП</title>
                      </head>
                      <body>
                      <ul>
                      """;
        foreach (List<string> row in googleResults.values)
        {
            if (row.Count == 6 && row[0]!="ID") // проверяем, что текст есть (если текста нет, то длина будет равна 5), а также проверям, что это не начало таблицы
            {
                DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1])); // Добавляем секунды от 1970 года
                html += $"<li>{row[5]} <i>{time.Date}</i></li>";
            }
        }

        html += "</ul></body></html>";

        return html;
    }
```
Теперь можно добавить в `Program.cs` 

```
app.MapGet("/sheets/", (HttpRequest request) => Results.Content(GoogleSheetsReader.CreateHtml(), "text/html"));
```

Перейти на страницу и увидеть огромный список новостей. Можно поиграться с оформлением и css, но все равно чего-то не хватает, а именно - картинок. Давайте доавим эту функцию. 

Для этого нам достаточно знать url картинки, давайте создадим функцию, которая будет создавать эту ссылку. При том нам известен id файла.

```
public static string GenerateImage(string file_id)
    {
        if (file_id == "None") return ""; // Проверка на пустоту
        return $"<img src=\"https://drive.google.com/thumbnail?authuser=0&id={file_id}&sz=w1000\" />"; // создаем изображение
    }
```

Давайте обновим нашу функцию

```
public static string CreateHtml()
    {
        GoogleResults? googleResults =Read().Result;
        if (googleResults is null) return "";
        string html = """
                      <!DOCTYPE html>
                      <html lang="ru">
                      <head>
                          <meta charset="UTF-8">
                          <title>Телеграм канал ЛШЮП</title>
                      </head>
                      <body>
                      <ul>
                      """;
        foreach (List<string> row in googleResults.values)
        {
            if (row.Count == 6 && row[0]!="ID")
            {
                DateTime time = new DateTime(1970, 1, 1).AddSeconds(int.Parse(row[1]));
                html += $"<li>{row[5]} <i>{time.Date}</i><br>{GenerateImage(row[3])}</li>"; // Обновление
            }
        }

        html += "</ul></body></html>";

        return html;
    }
```

Теперь можно смотреть фотографии, но чтобы это выглядело красиво, необходимо настроить стили.

Также, сейчас телеграм канал обновляется при каждом обновлении страницы (мы считываем всю информацию заново), а это не совсем оптимально. Можно сделать так, чтобы записи сохранялись один раз, а потом их уже брали из базы данных сайта. 



