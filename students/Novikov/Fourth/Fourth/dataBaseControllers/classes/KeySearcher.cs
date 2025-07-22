using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nestor;


/// <summary>
/// Интерфейс источника данных
/// </summary>
/// <typeparam name="D">тип элемента данных (документа)</typeparam>
/// <typeparam name="K">ключ по которому дается доступ к документу</typeparam>
public interface IDataSource<D, K>
{
    // Обычный конструктор имеет параметром поток элементов (документов)
    IEnumerable<D> Elements();
    IEnumerable<Tuple<D, K>> ElementsDK(); // Документ и его ключ
    D GetElement(K key);
    IEnumerable<K> WordToKeys(string word);
    Func<Tuple<D, NestorMorph>, IEnumerable<string>> GetDWFunc();
}
public class WordsSearcher<D, K>
{
    private IDataSource<D, K> dsource;
    private Func<Tuple<D, NestorMorph>, IEnumerable<string>> dwFunc;
    private Dictionary<string, K[]> wordToKeys;

    private NestorMorph morph;

    public WordsSearcher(IDataSource<D, K> dsource, NestorMorph morph)
    {
        this.dsource = dsource;
        this.dwFunc = dsource.GetDWFunc();
        this.morph = morph;

        wordToKeys = dsource.ElementsDK()
            .SelectMany(dk => dwFunc(new Tuple<D, NestorMorph>(dk.Item1, morph))
                .Select(w => { var k = dk.Item2; return (w, k); }))
            .GroupBy(wk => wk.w, wk => wk.k)
            .ToDictionary(igr => igr.Key, igr => igr.ToArray());
    }
    public (D, int)[] Search(string[] words)
    {

        // .Select(w => morph.Lemmatize(w)[0])
        var query = words.SelectMany(w => { if (wordToKeys.TryGetValue(w, out K[] karr)) return karr; else return new K[0]; })
            .GroupBy(k => k)
            .Select(igr => (igr.Key, igr.Count()))
            .OrderByDescending(k => k.Item2)
            .ToArray()
            ;
        return query.Select(x => (dsource.GetElement(x.Key), x.Item2)).ToArray();
        //foreach (var pair in query)
        // {
        //    Console.WriteLine($"group: {dsource.GetElement(pair.Key)} {pair.Item2}");
            //foreach (int i in group)
            //{
            //    Console.WriteLine(i);
            //}
        //  }
    }
}

/// <summary>
/// Источник данных. Сохраняет какое-то множество данных типа T с использованием ключа типа K
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="K"></typeparam>
public class DataSource<T, K>
{
    private T[] docs;
    public DataSource(IEnumerable<T> documents) { }
}

class DataSourceList : IDataSource<string, int>
{
    private string[] list;
    public DataSourceList(IEnumerable<string> flow)
    {
        list = flow.ToArray();
    }
    public IEnumerable<string> Elements()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Tuple<string, int>> ElementsDK() // Выдает поток документов с их ключами
    {
        return list.Select((d, i) => new Tuple<string, int>(d, i));
    }

    public Func<Tuple<string, NestorMorph>, IEnumerable<string>> GetDWFunc() // преобразователь документа в поток строк
    {
        return (Tuple<string, NestorMorph> tuple) => tuple.Item1.Split(" ").Distinct();//.Select(w => tuple.Item2.Lemmatize(w).FirstOrDefault()).Where(x=>x!=null);
    }

    public string GetElement(int key)
    {
        return list[key];
    }

    public IEnumerable<int> WordToKeys(string word)
    {
        throw new NotImplementedException();
    }
}

