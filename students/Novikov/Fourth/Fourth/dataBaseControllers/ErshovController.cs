using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Nestor;


public class ErshovArchiveController
{
    private IEnumerable<DocObject> database;
    public Dictionary<string, DocObject> dict_docs;

    private string[] docs_to_search;
    private Dictionary<int, string> scans = new Dictionary<int, string>();

    public WordsSearcher<string, int> searcher;

    public ErshovArchiveController(string path_to_folder, NestorMorph nestorMorph)
    {

        string file_content = File.ReadAllText(path_to_folder + @"\docs.csv");
        string file_scans_content = File.ReadAllText(path_to_folder + @"\lists.csv");


        this.scans = file_scans_content.Split("\n").Skip(1).Select(row => new KeyValuePair<int, string>(int.Parse(row.Split("\t")[0]), row.Split("\t")[3])).ToDictionary(x => x.Key, v => v.Value);


        this.database = file_content.Split("\n").Select(x => x.Split("\t")).Skip(1).Select(x => new DocObject(x, this.scans));

        this.docs_to_search = [.. database.Select(o => o.description)];

        this.dict_docs = this.database.DistinctBy(x => x.description).ToDictionary(x => x.description);

        DataSourceList dsl = new DataSourceList(docs_to_search);
        this.searcher = new WordsSearcher<string, int>(dsl, nestorMorph);

        Console.WriteLine("Loaded");

    }





    public IEnumerable<DocObject> GetDocs(int amount = 10)
    {
        return this.database.Where(x => x.type == "Фотографии").Take(amount);
    }

    public string CreateHtml(int start, int step)
    {
        string html = File.ReadAllText("wwwroot/htmls/2.html");
        string ul_string = "<ul>";

        ul_string += this.database.Skip(start).Take(step).Select(x => $"<li><img src='{x.url_docs.FirstOrDefault()}'/> {x.description}</li>").Aggregate((a, o) => a + o);

        ul_string += "</ul>";
        return html.Replace("{{ snippet }}", ul_string).Replace("{{ title }}", "Архив Ершова");
    }

    public string Search(string query_search)
    {
        (string, int)[] search_result = searcher.Search(query_search.Split(" "));


        string html = File.ReadAllText("wwwroot/htmls/index.html");
        string ul_string = "<ul>";

        ul_string += search_result.Select(x => dict_docs[x.Item1]).Select(x => $"<li><img src='{x.url_docs.FirstOrDefault()}'/> {x.description}</li>").Aggregate((a, o) => a + o);

        ul_string += "</ul>";
        return html.Replace("{{ list_docs }}", ul_string);
    }
        
    
}

