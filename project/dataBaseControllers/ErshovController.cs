using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Nestor;
using System.Text.Json;


public class ErshovArchiveController : DefaultController
{
    private List<DocObject> database;

    private Dictionary<int, string> scans = new Dictionary<int, string>();

    public WordsSearcher<string, int> searcher;

    public ErshovArchiveController(string path_to_folder)
    {

        string file_content = File.ReadAllText(path_to_folder + @"\docs.csv");
        string file_scans_content = File.ReadAllText(path_to_folder + @"\lists.csv");


        this.scans = file_scans_content.Split("\n").Skip(1).Select(row => new KeyValuePair<int, string>(int.Parse(row.Split("\t")[0]), row.Split("\t")[3])).ToDictionary(x => x.Key, v => v.Value);
        file_scans_content = "";


        this.database = file_content.Split("\n").Skip(1).Select(x => x.Split("\t")).Select(x => new DocObject(x)).Select(o =>
        {
            if (!o.row_array[6].Contains("N"))
            {

                o.url_docs = o.row_array[6].Split(",").Select(x => int.Parse(x)).Distinct().Where(id => scans.ContainsKey(id)).SelectMany(id => scans[id].Split(","));
                o.SerializeUrls();
            }
            return o;
        }).ToList();

        file_content = "";



        DataSourceList dsl = new DataSourceList([.. database.Select(o => o.description)]);
        this.searcher = new WordsSearcher<string, int>(dsl);

        Console.WriteLine("Ershov loaded");

    }
    public List<int> GetIds()
    {

        List<int> ids = this.database.Select(x => x.id).ToList();
        return ids;
    }


    public IEnumerable<DocObject> GetDocs(int amount = 10)
    {
        return this.database.Where(x => x.type == "Фотографии").Take(amount);
    }

    public string CreateHtml(int start, int step)
    {
        string html = File.ReadAllText("wwwroot/htmls/databasePage.html");
        string ul_string = "<ul>";

        ul_string += this.database.Skip(start).Take(step).Select(x => $"<li><img src='{x.url_docs.FirstOrDefault()}'/> {x.description}</li>").Aggregate((a, o) => a + o);

        ul_string += "</ul>";
        return HtmlPage.GetHtml("Архив Ершова", ul_string);
    }

    public string Search(string[] query_search)
    {
        IEnumerable<DocObject> search_result = searcher.SearchForKey(query_search).Select(x => this.database[x.Item1]);

        return JsonSerializer.Serialize(search_result);
    }

    public string CreateField(int num, bool onMainPage)
    {
        var o = this.database.SingleOrDefault(x => x.id == num);
        if (o == null)
        {
            return "";
        }
        if (onMainPage)
        {
            return o.description + "<br>" + o.url_docs.Select(u => $"<img src={u} width=\"200px\"></img>").Aggregate((acc, w) => acc + w);
        }
        return HtmlPage.GetField("Архив Ершова", o.description + "<br>" + o.url_docs.Select(u => $"<img src={u} width=\"200px\"></img>").Aggregate((acc, w) => acc + w));
    }
    
       public string CreateSearchView(int num)
    {
        var o = this.database.SingleOrDefault(x => x.id == num);
        if (o == null)
        {
            return "";
        }
        return HtmlPage.GetHtml("Архив Ершова", o.description +"<br>"+ o.url_docs.Select(u => $"<img src={u} width=\"200px\"></img>").Aggregate((acc, w)=>acc+w));
    }

}

