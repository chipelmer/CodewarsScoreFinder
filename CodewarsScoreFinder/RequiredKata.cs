using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    public class RequiredKata
    {
        public RequiredKata() { }
        public RequiredKata(params Kata[] kataNames) => List.AddRange(kataNames);

        public string GroupName { get; set; }
        public List<Kata> List { get; set; } = new List<Kata>();

        public static RequiredKata GetList1()
        {
            return new RequiredKata(new DataFinder().GetKataList("RequiredKataList1.csv"));
        }

        public static RequiredKata GetList2()
        {
            return new RequiredKata(new DataFinder().GetKataList("RequiredKataList2.csv"));
        }

        public static RequiredKata GetList3()
        {
            return new RequiredKata(new DataFinder().GetKataList("RequiredKataList3.csv"));
        }
    }
}
