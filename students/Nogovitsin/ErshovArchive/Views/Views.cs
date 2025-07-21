using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Nestor;

namespace ErshovArchive
{
    class ErshovArchiveView : DefaultViews
    {
        ErshovArchiveController ershov_controller;
        public WordsSearcher<string, int> searcher;
        public ErshovArchiveView(NestorMorph morph)
        {
            this.ershov_controller = new ErshovArchiveController(@"C:\Users\programmer\Documents\Mikhail\ErshovArchive\ErshArchData");



            DataSourceList dsl = new DataSourceList(ershov_controller.docs_to_search);
            this.searcher = new WordsSearcher<string, int>(dsl, morph);

        }

        public IResult GetResponse(HttpRequest request)
        {
            if (request.Query.ContainsKey("search"))
            {
                string query_search = request.Query["search"];
                if (query_search == "" || query_search == null)
                {
                    Results.Content(ershov_controller.GetHtml(), "text/html");
                }

                (string, int)[] search_result = searcher.Search(query_search.Split(" "));

                string all_text = ErshovArchiveController.CreateHtml(search_result.Select(x => ershov_controller.dict_docs[x.Item1]));

                return Results.Content(all_text, "text/html");
            }
            return Results.Content(ershov_controller.GetHtml(), "text/html");
        }
    }

    public interface DefaultViews
    {
        IResult GetResponse(HttpRequest request);
    }
}
