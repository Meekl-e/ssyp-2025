using System.Text;
using System.Xml.Linq;

namespace Less71
{
    public class Post
    {
        public DateTime date;
        public int id;
        public string text = String.Empty;
        public string owner = String.Empty;
        public List<Media> media = new List<Media>(); // список медиа-записей, прикрепленных к этому посту
    }
    public class Media
    {
        public string id = String.Empty;
        public int post_id; // ID поста, к которому привязан данный медиа файл
        public int width;
        public int height;
        public string uri = String.Empty;

    }
    public class Controller
    {
        List<Post> posts = new List<Post>();
        List<Media> medias = new List<Media>();
        public Controller()
        {

            var xDB = XElement.Load("wwwroot/data.fog");  // загружаем базу данных
            foreach (XElement xElement in xDB.Elements()) // итерируемся по объектам
            {
                if (xElement.Name.LocalName == "post") // Если объект <post></post>
                {
                    int idFromBD = int.Parse(xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value);  // Берем значение атрибута rdf:about
                    string text = xElement.Element("text").Value; // Берем значение внутреннего элемента <text></text>. 
                    int date = int.Parse(xElement.Element("date").Value); // Берем значение внутреннего элемента <date></date>. 
                    string owner = xElement.Element("owner_id").Value; // Берем значение внутреннего элемента <owner_id></owner_id>. 
                    posts.Add(new Post() { date = new DateTime(1970, 1, 1).AddSeconds(date), id = idFromBD, owner = owner, text = ConvertBase64(text) }); // добавляем к постам
                }
                if (xElement.Name.LocalName == "media") // пропускаем медиа
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
        public static string ConvertBase64(string text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(text)); // декодируем
        }
    }
}
