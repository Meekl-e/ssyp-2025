using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Nestor;
using System.Text.Json;


public class ErshovArchiveController
{
    private List<DocObject> database;

    private Dictionary<int, string> scans = new Dictionary<int, string>();

    public WordsSearcher<string, int> searcher;

    public ErshovArchiveController(string path_to_folder, NestorMorph nestorMorph)
    {

        string file_content = File.ReadAllText(path_to_folder + @"\docs.csv");
        string file_scans_content = File.ReadAllText(path_to_folder + @"\lists.csv");


        this.scans = file_scans_content.Split("\n").Skip(1).Select(row => new KeyValuePair<int, string>(int.Parse(row.Split("\t")[0]), row.Split("\t")[3])).ToDictionary(x => x.Key, v => v.Value);
        file_scans_content = "";


        this.database = file_content.Split("\n").Skip(1).Select(x => x.Split("\t")).Select(x => new DocObject(x)).Select(o =>
        {
            if (!o.row_array[6].Contains("N"))
            {

                o.url_docs = o.row_array[6].Split(",").Select(x => int.Parse(x)).Distinct().Where(id => scans.ContainsKey(id)).Select(id => scans[id]);
                o.SerializeUrls();
            }
            return o;
        }).ToList();
        
        file_content = "";

        // this.dict_docs = this.database.ToDictionary(x => x.id);

        DataSourceList dsl = new DataSourceList([.. database.Select(o => o.description)]);
        this.searcher = new WordsSearcher<string, int>(dsl, nestorMorph);

        Console.WriteLine("Loaded");

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
        return html.Replace("{{ snippet }}", ul_string).Replace("{{ title }}", "Архив Ершова");
    }

    public string Search(string query_search)
    {
        IEnumerable<DocObject> search_result = searcher.SearchForKey(query_search.Split(" ")).Select(x => this.database[x.Item1]);
        Console.WriteLine(search_result.Count());

        return JsonSerializer.Serialize(search_result);
    }

       public string GetOnePost(int num)
    {
        var o = this.database.Skip(num - 1).First();
        
        return $"<li>{o.description}</li>";
    }

        
    
}

