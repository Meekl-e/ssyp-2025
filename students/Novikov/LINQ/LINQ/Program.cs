using System;
using System.Linq;

namespace Fifth
{
    class Program
    {
        static void Main(string[] args)
        {

            var cnt = Enumerable.Range((Enumerable.Range(1, 5).Select(x => x * 11).Max()), 45).Reverse().Intersect(Enumerable.Range(1, 55));
            //Console.Write(cnt);
            foreach (var el in cnt)
            {
                //Console.Write(el + " ");
            }
            //Console.Write("\n");
            //var res = cnt.Skip(10).Take(7).Average();
            //Console.Write(res);
            (char, int)[] arr = { ('p', 4), ('s', 1), ('s', 2), ('y', 3), ('2', 5), ('5', 6) };
            var res = arr.OrderBy(x => x.Item2).Select(x => (x.Item1).ToString()).Aggregate((cnct, x) => cnct + x);
            Console.Write(res);
        }
    }
}
