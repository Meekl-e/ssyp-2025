### Подключение базы данных 

Здесь мы разберем, как можно подключить xml базу данных. 

Для начала, давайте создадим класс `Controller`, который будет отвечать за подключение и анализ БД. Далее создадим папку `database`, в которую перенесем [данные из Яндекс Диска](https://disk.yandex.ru/d/zO2PfSxPSgpseg). Будем работать с базой данных ВКонтакте. 

Далее необходимо инициализировать загрузку (один раз загрузим, а потом будет использовать). Можно создать список posts и media в классе. Давайте также создадим сам класс поста и класс медиа-записи по данным, которые присутсвуют в базе данных. Создадим классы в отдельном файле, например `Classes.cs`

```
public class Post
{
    public DateTime date; 
    public int id;
    public string text  = String.Empty;
    public string owner  = String.Empty;
    public List<Media> media = new List<Media>(); // список медиа-записей, прикрепленных к этому посту
}
public class Media
{
    public string id = String.Empty;
    public int post_id; // ID поста, к которому привязан данный медиа файл
    public int width;
    public int height;
    public string uri  = String.Empty;
        
}
```


Общая структура контроллера будет выглядеть так:

```
public class Controller
{
    List<Post> posts  = new List<Post>();
    List<Media> medias  = new List<Media>();
}
```
Заметьте, что posts и media - локальные переменные, которые недоступны "извне". Мы будем осуществлять доступ к ним через методы.

Создадим конструктор класса, чтобы загружать базу данных. Это выполняется с помощью `XElement.Load("database/data.fog")`. В параметры передается путь до базы данных. Не забудьте импортировать `using System.Xml.Linq;`

Далее по этому объекту можно итерироваться и получать атрибуты и элементы. Ведь xml разметка - это древовидная разметка.  
Получим посты.
```
public Controller()
{
    
    var xDB = XElement.Load("database/data.fog");  // загружаем базу данных
    foreach (XElement xElement in xDB.Elements()) // итерируемся по объектам
    {
        if (xElement.Name.LocalName == "post") // Если объект <post></post>
        {
            int idFromBD = int.Parse(xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value);  // Берем значение атрибута rdf:about
            string text = xElement.Element("text").Value; // Берем значение внутреннего элемента <text></text>. 
            int date = int.Parse(xElement.Element("date").Value); // Берем значение внутреннего элемента <date></date>. 
            string owner = xElement.Element("owner_id").Value; // Берем значение внутреннего элемента <owner_id></owner_id>. 
            posts.Add(new Post() { date = new DateTime(1970, 1, 1).AddSeconds(date), id = idFromBD, owner = owner, text = text }); // добавляем к постам
        }
        if (xElement.Name.LocalName == "media") // пропускаем медиа
        {
            break;
        }
    }
}
```

Создав класс, давайте создадим экземпляр класса в `Program.cs`, чтобы все загрузить. Добавьте наверху
`Controller controller = new Controller();`

Итак, ничего не произошло. Чтобы проверить, что мы сделали все правильно можно воспользоваться инструментами отладки или создать первую функцию, которая нам красиво вернет список постов. Давайте создадим метод `CreateHtml()` в классе `Controller`

```
 public string CreateHtml()
    {
        string html = """
                      <!DOCTYPE html>
                      <html lang="ru">
                      <head>
                          <meta charset="UTF-8">
                          <title>Посты ВКонтакте</title>
                      </head>
                      <body><ol>
                      """;
        foreach (Post post in posts)
        {
            html += $"<li>{post.text} <i>{post.date}</i></li>";
        }

        html += "</ol></body></html>"; // Создаем HTML код
        return html;
    }
```
И также добавить маршрут в `Program.cs`
```
app.MapGet("/vk/", (HttpRequest request) => Results.Content(controller.CreateHtml(), "text/html"));
```


Перейдем по `http://localhost:5042/vk` (замените `5042` своим портом) и увидим много непонятного текста, поскольку весь текст закодирован в [base64 кодировке](https://ru.wikipedia.org/wiki/Base64). Это сделано для безопасной записи и чтения, чтобы сохранить спецсимволы (перенос строки, табуляция), а также аккуратно все записать. 

Также вы могли увидеть, что дата у нас записана секундами, с начала 1970 года (с начала эпохи), а на странице мы видим нормальную дату. Это происходит, поскольку мы автоматически конвертируем секунды в формат `DateTime` класса `Post`.

Давайте допишем класс `Controller`, чтобы текст отображался нормально. Добавим функцию `ConvertBase64` и не забудем вверху указать `using System.Text;`
```
public static string ConvertBase64(string text)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(text)); // декодируем
    }
```
Изменим добавление постов, теперь будем пропускать текст через нашу функцию.
```
 public Controller()
    {
        var xDB = XElement.Load("database/data.fog");  // загружаем базу данных
        foreach (XElement xElement in xDB.Elements()) // итерируемся по объектам
        {
            if (xElement.Name.LocalName == "post") // Если объект <post></post>
            {
                int idFromBD = int.Parse(xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value);  // Берем значение атрибута rdf:about
                string text = xElement.Element("text").Value; // Берем значение внутреннего элемента <text></text>. 
                int date = int.Parse(xElement.Element("date").Value); // Берем значение внутреннего элемента <date></date>. 
                string owner = xElement.Element("owner_id").Value; // Берем значение внутреннего элемента <owner_id></owner_id>. 
                
                posts.Add(new Post() { date = new DateTime(1970, 1, 1).AddSeconds(date), id = idFromBD, owner = owner, text = ConvertBase64(text) }); // обновление
            }
            if (xElement.Name.LocalName == "media") // пропускаем медиа
            {
                break;
            }
        }
    }
```
Проверим, что все работает, перейдя на страницу. Как можно заметить, подгрузилось 1535 постов, но нам не хватает картинок. Давайте их добавим. В первую очередь положите папку `photos` из базы данных в папку `wwwroot`. Так мы сможем подгрузить фотографии на страницу.
```
public Controller()
    {
        var xDB = XElement.Load("database/data.fog"); // загружаем базу данных
        foreach (XElement xElement in xDB.Elements()) // итерируемся по объектам
        {
            if (xElement.Name.LocalName == "post") // Если объект <post></post>
            {
                int idFromBD =
                    int.Parse(xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about")
                        .Value); // Берем значение атрибута rdf:about
                string text = xElement.Element("text").Value; // Берем значение внутреннего элемента <text></text>. 
                int date = int.Parse(xElement.Element("date")
                    .Value); // Берем значение внутреннего элемента <date></date>. 
                string
                    owner = xElement.Element("owner_id")
                        .Value; // Берем значение внутреннего элемента <owner_id></owner_id>. 

                posts.Add(new Post()
                {
                    date = new DateTime(1970, 1, 1).AddSeconds(date), id = idFromBD, owner = owner,
                    text = ConvertBase64(text)
                }); // добавляем к постам
            }

            if (xElement.Name.LocalName == "media")
            {
                string idFromBD = (xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value);
                int post_id = int.Parse(idFromBD.Split('-')[0]); // берем ID поста
                string uri = xElement.Element("uri").Value; // берем путь файла
                int width = int.Parse(xElement.Element("width").Value); // ширина картинки
                int height = int.Parse(xElement.Element("height").Value); // высота картинки
                Media media = new Media()
                    { uri = uri, width = width, height = height, post_id = post_id, id = idFromBD }; // создаем объект
                foreach (Post p in posts) // ищем пост, к которому привязан данный файл
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
```
Фотографии добавлены, осталось загрузить на страницу. Давайте модернизируем `CreateHtml()`

```
public string CreateHtml()
    {
        string html = """
                      <!DOCTYPE html>
                      <html lang="ru">
                      <head>
                          <meta charset="UTF-8">
                          <title>Посты ВКонтакте</title>
                      </head>
                      <body><ol>
                      """;
        foreach (Post post in posts)
        {
            html += $"<li>{post.text} <i>{post.date}</i><br> ";
            foreach (Media media in post.media) // добавляем фотографии
            {
                html += $"<img src=\"{media.uri}\" />";
            }

            html += "</li>";
        }

        html += "</ol></body></html>";
        return html;
    }

```
После этого обновите страницу и у вас появятся фотографии. 

