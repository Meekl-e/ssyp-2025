using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ErshovArchive
{
    class ErshovArchiveController
    {
        private IEnumerable<DocObject> database;
        public Dictionary<string, DocObject> dict_docs;

        public string[] docs_to_search;
        private Dictionary<int, string> scans = new Dictionary<int, string>();
        public ErshovArchiveController(string path_to_folder)
        {
            string file_content = File.ReadAllText(path_to_folder + @"\docs.csv");
            string file_scans_content = File.ReadAllText(path_to_folder + @"\lists.csv");



            this.scans = file_scans_content.Split("\n").Skip(1).Select(row => new KeyValuePair<int, string>(int.Parse(row.Split("\t")[0]), row.Split("\t")[3])).ToDictionary(x => x.Key, v => v.Value);


            this.database = file_content.Split("\n").Select(x => x.Split("\t")).Skip(1).Select(x => new DocObject(x, this.scans));

            this.docs_to_search = [.. database.Select(o => o.description)];

            this.dict_docs = this.database.DistinctBy(x=>x.description).ToDictionary(x => x.description);

            Console.WriteLine("Loaded");

        }
       


        

        public IEnumerable<DocObject> GetDocs(int amount = 10)
        {
            return this.database.Where(x => x.type == "Фотографии").Take(amount);
        }

        public string GetHtml(int amount = 10)
        {
            return ErshovArchiveController.CreateHtml(GetDocs(amount));
        }

        public static string CreateHtml(IEnumerable<DocObject> posts)
        {
            string html = File.ReadAllText("wwwroot/htmls/index.html");
            string ul_string = "<ul>";
            if (posts.Count() != 0)
            {
                ul_string += posts.Select(x => $"<li><img src='{x.url_docs.FirstOrDefault()}'/> {x.description}</li>").Aggregate((a, o) => a + o);
            }
            else
            {
                ul_string += "<li>НЕ НАЙДЕНО</li>";
            }
            
            ul_string += "</ul>";
            return html.Replace("{{ list_docs }}", ul_string);

        }
           
        
    }
}
