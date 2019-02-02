using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    class KataListBoard
    {
        public CodewarsUsersGroup CodewarsUsersGroup { get; private set; }

        public KataListBoard(CodewarsUsersGroup codewarsUsersGroup)
        {
            populateUserCompletedKataLists(codewarsUsersGroup);

            CodewarsUsersGroup = codewarsUsersGroup;
        }

        private void populateUserCompletedKataLists(CodewarsUsersGroup codewarsUsersGroup)
        {
            DataFinder dataFinder = new DataFinder();

            new System.Threading.Thread(() => dataFinder.PopulateCompletedKata(codewarsUsersGroup)).Start();
            UX.DisplayLoadingWindow(UX.LoadingOptions.CyclingBar,
                () => codewarsUsersGroup.PopulatedKataListCount,
                () => codewarsUsersGroup.TotalCount,
                "Getting data from Codewars...");
        }

        public override string ToString()
        {
            string[,] dataTable = getKataListBoardAsTable();
            return Formatter.GetTableFromArray(dataTable);
        }

        private string[,] getKataListBoardAsTable()
        {
            List<RequiredKata> requiredKataLists = new List<RequiredKata>();
            requiredKataLists.Add(RequiredKata.GetList1());
            requiredKataLists.Add(RequiredKata.GetList2());
            requiredKataLists.Add(RequiredKata.GetList3());

            // plus 1 for header, plus 2 for Username-TotalKata
            string[,] dataTable = new string[CodewarsUsersGroup.TotalCount + 1, requiredKataLists.Count + 2];

            dataTable[0, 0] = "Username";
            // plus 1 to shift right because of Username column
            dataTable[0, requiredKataLists.Count + 1] = "Total Kata";

            for (int user = 0; user < CodewarsUsersGroup.Users.Count; user++)
            {
                dataTable[user + 1, 0] = CodewarsUsersGroup.Users[user].Username;
                // plus 1 for column to shift right because of Username column
                dataTable[user + 1, requiredKataLists.Count + 1] = CodewarsUsersGroup.Users[user].CompletedKata.Count.ToString();

                for (int listCount = 0; listCount < requiredKataLists.Count; listCount++)
                {
                    // plus 1 for column to shift right because of Username column
                    dataTable[0, listCount + 1] = "Kata List " + (listCount + 1);

                    int listCompletionCount = 0;
                    foreach (Kata requiredKata in requiredKataLists[listCount].List)
                    {
                        foreach (Kata completedKata in CodewarsUsersGroup.Users[user].CompletedKata)
                        {
                            if (completedKata.Id == requiredKata.Id)
                            {
                                listCompletionCount += 1;
                                break;
                            }
                        }
                    }

                    // plus 1 in column to shift right because of Username column
                    dataTable[user + 1, listCount + 1] = listCompletionCount + "/" + requiredKataLists[listCount].List.Count;
                }
            }

            return dataTable;
        }
    }
}
