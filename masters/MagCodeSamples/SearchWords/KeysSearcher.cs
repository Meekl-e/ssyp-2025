using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchWords
{
    public class DataSource<T>
    {
        private T[] docs;
        public DataSource(IEnumerable<T> documents) { }
    }
    public class KeysSearcher
    {
        private object[] docs;
        private Dictionary<string, int[]> wordToIndexes;
        public KeysSearcher(object[] docs, Func<object, IEnumerable<string>> ToWordsFunc) 
        { 
            this.docs = docs;
            wordToIndexes = docs.SelectMany((d, i) => ToWordsFunc(d).Select(w => (w, i)))
                .GroupBy(wi => wi.w, wi => wi.i)
                .ToDictionary(igr => igr.Key, igr => igr.ToArray());
        }
        public object Search
    }
}
