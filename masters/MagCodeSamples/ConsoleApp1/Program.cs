
// See https://aka.ms/new-console-template for more information
using Factograph.Data.r;

Console.WriteLine("Hello, World!");

string wwwpath = "../../../../data/"; // Это для запуска через dotnet
Factograph.Data.IFDataService db = new Factograph.Data.FDataService(wwwpath, wwwpath + "Ontology_iis-v14.xml", null);
db.Reload();
var xx = db.SearchByName("но");
foreach (var x in xx)
{
    //Console.WriteLine(x.ToString());
}

var y = db.GetItemByIdBasic("Syp2022_mag111_637946167303122294_1105", true);
Console.WriteLine(y.ToString());

var z = db.GetRRecord("Syp2022_mag111_637946167303122294_1105", true);
Console.WriteLine(z.GetName());

var shablon0 = new Rec(null, "http://fogid.net/o/person");
var t = Rec.Build(z, shablon0, null, null);
Console.WriteLine(t.ToString());

var shablon = Rec.GetUniShablon(z.Tp, 2, null, db.ontology);
// Потом раскладываем ресширенную запись в соответствии с шаблоном
var tree = Rec.Build(z, shablon, db.ontology, idd => db.GetRRecord(idd, false));

Console.WriteLine(tree.ToString());
