
namespace CodewarsScoreFinder
{
    public class Leaderboard
    {
        public CodewarsUsersGroup CodewarsUsersGroup { get; private set; }

        public Leaderboard(CodewarsUsersGroup codewarsUsersGroup)
        {
            CodewarsUsersGroup = codewarsUsersGroup;
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

            int row = 1;
            foreach (CodewarsUser user in CodewarsUsersGroup.Users)
            {
                dataTable[row, 0] = user.Username;
                dataTable[row, 1] = user.Name;
                dataTable[row, 2] = user.Score.ToString();
                row += 1;
            }

            return dataTable;
        }
    }
}
