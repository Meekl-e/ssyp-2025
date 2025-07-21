using System;
using System.Collections.Generic;
using System.Linq;


public class DocObject
{
    public string type = "";
    public IEnumerable<int> ids_doc = new List<int>();
    public IEnumerable<string> url_docs = new List<string>();
    public int id;

    public string description = "";

    public DocObject(IEnumerable<string> row, Dictionary<int, string> scans)
    {
        string[] row_array = row.ToArray();
        this.type = row_array[1];
        this.description = row_array[3];
        this.id = int.Parse(row_array[0]);
        if (!row_array[6].Contains("N"))
        {

            this.ids_doc = row_array[6].Split(",").Select(x => int.Parse(x)).Distinct().Where(id => scans.ContainsKey(id));
            this.url_docs = ids_doc.Select(id => scans[id]);
        }
    }

    public bool Print()
    {
        Console.Write($"{id} {type} urls: ");
        foreach (var v in this.url_docs)
        {
            Console.Write($"{v}, ");
        }
        Console.Write("\n");
        return true;
    }
    
}
