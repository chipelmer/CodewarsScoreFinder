using System;
using System.Collections.Generic;
using System.Text;

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
            var dataFinder = new DataFinder();

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
            // plus 1 for header, 3 for Username-List1-List2-List3-TotalKata
            string[,] dataTable = new string[CodewarsUsersGroup.TotalCount + 1, 5];
            RequiredKata list1 = RequiredKata.GetList1();

            dataTable[0, 0] = "Username";
            dataTable[0, 1] = "List 1";
            dataTable[0, 2] = "List 2";
            dataTable[0, 3] = "List 3";
            dataTable[0, 4] = "Total Kata";

            for (int user = 0; user < CodewarsUsersGroup.Users.Count; user++)
            {
                int list1CompletionCount = 0;
                foreach (Kata requiredKata in list1.List)
                {
                    foreach (Kata completedKata in CodewarsUsersGroup.Users[user].CompletedKata)
                    {
                        if (completedKata.Id == requiredKata.Id)
                        {
                            list1CompletionCount += 1;
                            break;
                        }
                    }
                }

                dataTable[user + 1, 0] = CodewarsUsersGroup.Users[user].Username;
                dataTable[user + 1, 1] = list1CompletionCount + "/" + list1.List.Count;
                dataTable[user + 1, 2] = "";
                dataTable[user + 1, 3] = "";
                dataTable[user + 1, 4] = CodewarsUsersGroup.Users[user].CompletedKata.Count.ToString();
            }

            return dataTable;
        }
    }
}
