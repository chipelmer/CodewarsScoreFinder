
namespace CodewarsScoreFinder
{
    public class Leaderboard
    {
        public CodewarsUsersGroup CodewarsUsersGroup { get; private set; }

        public Leaderboard(CodewarsUsersGroup codewarsUsersGroup)
        {
            populateUserScores(codewarsUsersGroup);

            CodewarsUsersGroup = codewarsUsersGroup;
        }

        private void populateUserScores(CodewarsUsersGroup codewarsUsersGroup)
        {
            var dataFinder = new DataFinder();

            new System.Threading.Thread(() => dataFinder.PopulateScores(codewarsUsersGroup)).Start();
            UX.DisplayLoadingWindow(UX.LoadingOptions.CyclingBar,
                () => codewarsUsersGroup.PopulatedScoresCount,
                () => codewarsUsersGroup.TotalCount,
                "Getting data from Codewars...");
        }

        public override string ToString()
        {
            string[,] dataTable = getLeaderboardAsTable();
            return Formatter.GetTableFromArray(dataTable);
        }

        private string[,] getLeaderboardAsTable()
        {
            CodewarsUsersGroup.SortUsersByScore();

            // plus 1 for header, 3 for Username-Name-Score
            string[,] dataTable = new string[CodewarsUsersGroup.TotalCount + 1, 3];

            dataTable[0, 0] = "Username";
            dataTable[0, 1] = "Name";
            dataTable[0, 2] = "Score";

            for (int user = 0; user < CodewarsUsersGroup.Users.Count; user++)
            {
                dataTable[user + 1, 0] = CodewarsUsersGroup.Users[user].Username;
                dataTable[user + 1, 1] = CodewarsUsersGroup.Users[user].Name;
                dataTable[user + 1, 2] = CodewarsUsersGroup.Users[user].Score.ToString();
            }

            return dataTable;
        }
    }
}
