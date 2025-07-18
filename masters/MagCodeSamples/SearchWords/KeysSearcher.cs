using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchWords
{
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
        Func<D, IEnumerable<string>> GetDWFunc();
    }
    public class WordsSearcher<D, K>
    {
        private IDataSource<D, K> dsource;
        private Func<D, IEnumerable<string>> dwFunc;
        private Dictionary<string, K[]> wordToKeys;
        public WordsSearcher(IDataSource<D, K> dsource)
        {
            this.dsource = dsource;
            this.dwFunc = dsource.GetDWFunc();

            wordToKeys = dsource.ElementsDK()
                .SelectMany(dk => dwFunc(dk.Item1)
                    .Select(w => { var k = dk.Item2; return (w, k); }))
                .GroupBy(wk => wk.w, wk => wk.k)
                .ToDictionary(igr => igr.Key, igr => igr.ToArray());
        }
        public void Search(string[] words)
        {
            var query = words.SelectMany(w => { if (wordToKeys.TryGetValue(w, out K[] karr)) return karr; else return new K[0]; })
                .GroupBy(k => k)
                .Select(igr => (igr.Key, igr.Count()))
                .OrderByDescending(k => k.Item2)
                .ToArray()
                ;
            foreach (var pair in query)
            {
                Console.WriteLine($"group: {dsource.GetElement(pair.Key)} {pair.Item2}");
                //foreach (int i in group)
                //{
                //    Console.WriteLine(i);
                //}
            }
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

}
