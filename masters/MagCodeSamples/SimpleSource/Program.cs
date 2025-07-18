var builder = WebApplication.CreateBuilder(args); var app = builder.Build(); // начальные строчки

PersConroller cont1 = new PersConroller();
Func<HttpRequest, IResult> Transform = (request) => Results.Text($@"
<!DOCTYPE html>
<html>
    <head> <meta charset='utf-8'/> </head>
    <body>
        <h1>Привет!</h1>
        {cont1.persons
            .Select(p => $"<div>{p.name} {p.phone}</div>")
            .Aggregate((sum, s) => sum + s)}
    </body>
</html>
", "text/html");

app.MapGet("/", Transform);

app.Run(); // запуск приложения

public class Pers { public string name, phone; public Pers(string na, string ph) { name = na; phone = ph; } }
public class PersConroller
{
    public Pers[] persons = new Pers[]
    {
        new Pers("Иванов Иван Иванович", "777-7777"),
        new Pers("Петров Петр Петрович", "123-4567"),
        new Pers("Сидоров Сидор Сидорович", "555-1234")
    };
}