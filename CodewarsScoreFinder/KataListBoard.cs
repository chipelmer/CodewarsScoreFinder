using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    class KataListBoard
    {
        public CodewarsUsersGroup CodewarsUsersGroup { get; private set; }

        public KataListBoard(CodewarsUsersGroup codewarsUsersGroup)
        {
            CodewarsUsersGroup = codewarsUsersGroup;
        }

        public override string ToString()
        {
            string[,] dataTable = getKataListBoardAsTable();
            return Formatter.GetTableFromArray(dataTable);
        }

        private string[,] getKataListBoardAsTable()
        {
            List<KataList> requiredKataLists = KataList.RequiredKataLists();

            // plus 1 for header, plus 2 for Username-TotalKata
            string[,] dataTable = new string[CodewarsUsersGroup.TotalCount + 1, requiredKataLists.Count + 2];

            dataTable[0, 0] = "Username";
            dataTable[0, 1] = "Total Kata";
            for (int col = 2; col < requiredKataLists.Count + 2; col++)
                dataTable[0, col] = requiredKataLists[col - 2].GroupName;

            int row = 1;
            foreach (CodewarsUser user in CodewarsUsersGroup.Users)
            {
                dataTable[row, 0] = user.Username;
                dataTable[row, 1] = user.CompletedKata.Count.ToString();
                int col = 2;
                foreach (KataList kataList in requiredKataLists)
                {
                    dataTable[row, col] = completionCount(user, kataList).ToString() + "/" + kataList.List.Count;
                    col += 1;
                }

                row += 1;
            }

            return dataTable;
        }
        private int completionCount(CodewarsUser user, KataList kataList)
        {
            int completionCount = 0;
            foreach (Kata requiredKata in kataList.List)
            {
                foreach (Kata completedKata in user.CompletedKata)
                {
                    if (completedKata.Id == requiredKata.Id)
                    {
                        completionCount += 1;
                        break;
                    }
                }
            }

            return completionCount;
        }
    }
}
