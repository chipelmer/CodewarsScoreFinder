using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    public class KataList
    {
        public KataList(string groupName, Kata[] kataNames)
        {
            GroupName = groupName;
            List.AddRange(kataNames);
        }

        public string GroupName { get; set; }
        public List<Kata> List { get; set; } = new List<Kata>();

        public static List<KataList> RequiredKataLists()
        {
            List<KataList> kataLists = new List<KataList>();
            kataLists.Add(new KataList("List 1", getKata("Kata/RequiredKataList1.csv")));
            kataLists.Add(new KataList("List 2", getKata("Kata/RequiredKataList2.csv")));
            kataLists.Add(new KataList("List 3", getKata("Kata/RequiredKataList3.csv")));
            return kataLists;
        }
        private static Kata[] getKata(string file)
        {
            string[,] data = new DataFinder().ReadCsvFile(file, "Unable to get kata list.", true);
            Kata[] kata = new Kata[data.GetLength(0)];

            for (int row = 0; row < data.GetLength(0); row++)
                kata[row] = new Kata(data[row, 0], data[row, 1], data[row, 2], data[row, 3]);

            return kata;
        }
    }
}
