using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchWords
{
    class Program
    {
        public static void Main()
        {
            List<int> numbers = new List<int>(new int[] { 35, 44, 200, 84, 3987, 4, 199, 329, 446, 208 });

            IEnumerable<IGrouping<int, int>> query = numbers
                .GroupBy(number => number % 2);

            foreach (var group in query)
            {
                Console.WriteLine(group.Key == 0 ? "\nEven numbers:" : "\nOdd numbers:");
                foreach (int i in group)
                {
                    Console.WriteLine(i);
                }
            }
            Console.WriteLine("Start SearchWords");

            string[] docs = new string[] { "qwe rty", "asd fgh jkl", "qwe aaa uiop" };
            
            DataSourceList dsl = new DataSourceList(docs);
            WordsSearcher<string, int> searcher = new WordsSearcher<string, int>(dsl);
            searcher.Search("sdl uiop qwe".Split(' '));
        }
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

        public Func<string, IEnumerable<string>> GetDWFunc() // преобразователь документа в поток строк
        {
            return (string line) => line.Split(' ').Distinct();
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
}

