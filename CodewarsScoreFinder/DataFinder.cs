using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CodewarsScoreFinder
{
    public class DataFinder
    {
        public string[,] ReadCsvFile(string file, string errorMessage, bool replaceNullsWithEmptyStrings)
        {
            string[] lines = readFile(file, errorMessage);

            int maxColumnCount = getMaxColumnCount(lines);

            string[,] data = splitArrayLinesByCommas(lines, maxColumnCount);

            if (replaceNullsWithEmptyStrings)
                this.replaceNullsWithEmptyStrings(data);

            return data;
        }
        private int getMaxColumnCount(string[] lines)
        {
            int maxColumnCount = 0;
            foreach (string line in lines)
            {
                string[] cells = line.Split(',');
                if (cells.Length > maxColumnCount)
                    maxColumnCount = cells.Length;
            }

            return maxColumnCount;
        }
        private string[,] splitArrayLinesByCommas(string[] lines, int maxColumnCount)
        {
            string[,] data = new string[lines.Length, maxColumnCount];
            for (int row = 0; row < lines.Length; row++)
            {
                string[] cells = lines[row].Split(',');
                for (int col = 0; col < cells.Length; col++)
                    data[row, col] = cells[col];
            }

            return data;
        }
        private void replaceNullsWithEmptyStrings(string[,] data)
        {
            for (int row = 0; row < data.GetLength(0); row++)
                for (int col = 0; col < data.GetLength(1); col++)
                    if (data[row, col] == null)
                        data[row, col] = "";
        }

        private string[] readFile(string file, string errorMessage)
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(file);
            }
            catch
            {
                Console.WriteLine("ERROR: " + errorMessage);
                Console.WriteLine("File: " + file);
                Console.ReadLine();
            }

            return lines;
        }

        public void PopulateUserDataFromCodewars(CodewarsUsersGroup users)
        {
            foreach (CodewarsUser user in users.Users)
                populateScore(user);
        }
        private async Task populateScore(CodewarsUser user)
        {
            WebClient client = new WebClient();
            string apiResponse = null;
            try
            {
                apiResponse = await client.DownloadStringTaskAsync("https://www.codewars.com/api/v1/users/" + user.Username);
            }
            catch
            {
                Console.WriteLine("Error getting a user's data from Codewars: " + user.Username);
                user.Score = -1;
                user.TotalCompletedKata = -1;
            }

            if (apiResponse != null)
            {
                JObject response = JObject.Parse(apiResponse);
                int.TryParse(response.GetValue("honor").ToString(), out int score);
                int.TryParse(response.SelectToken("codeChallenges.totalCompleted").ToString(), out int totalCompletedKataCount);
                user.Score = score;
                user.TotalCompletedKata = totalCompletedKataCount;
            }
        }
    }
}
