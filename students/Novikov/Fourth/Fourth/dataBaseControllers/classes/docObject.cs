using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;


public class DocObject
{
    public string type { get; set; }
    public IEnumerable<string> url_docs = new List<string>();
    public int id { get; set; }

    public string[] row_array;

    public string description { get; set; }
    public string serialized_urls { get; set; }

    public DocObject(IEnumerable<string> row)
    {
        row_array = row.ToArray();
        this.type = row_array[1];
        this.description = row_array[3];
        this.id = int.Parse(row_array[0]);
    }
    public void SerializeUrls()
    {
        this.serialized_urls = JsonSerializer.Serialize(url_docs);
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
