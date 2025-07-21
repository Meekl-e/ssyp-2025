
using Nestor;

namespace ErshovArchive
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();


            var morph = new NestorMorph();
            ErshovArchiveView ershovViews = new ErshovArchiveView(morph);
    
            app.MapGet("/", (HttpRequest request ) => ershovViews.GetResponse(request));
            

            app.Run();
        }
    }
}