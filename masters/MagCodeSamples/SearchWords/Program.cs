

using SearchWords;

Console.WriteLine("Start SearchWords");

string[] docs = new string[] { "qwe rty uiop", "asd fgh jkl", "qwe aaa" };
KeysSearcher searcher = new KeysSearcher(docs.Select(s => (object)s).ToArray(), ob => ((string)ob).Split(' '));
