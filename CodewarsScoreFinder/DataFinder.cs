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

        public void PopulateScores(CodewarsUsersGroup users)
        {
            foreach (CodewarsUser user in users.Users)
                populateScore(user);
        }
        private async Task populateScore(CodewarsUser user)
        {
            WebClient client = new WebClient();
            string str = null;
            try
            {
                str = await client.DownloadStringTaskAsync("https://www.codewars.com/api/v1/users/" + user.Username);
            }
            catch
            {
                Console.WriteLine("Error getting a user's data from Codewars: " + user.Username);
            }

            if (str != null)
            {
                JObject response = JObject.Parse(str);
                int.TryParse(response.GetValue("honor").ToString(), out int score);
                user.Score = score;
            }
        }

        public void PopulateCompletedKata(CodewarsUsersGroup codewarsUsersGroup)
        {
            foreach (CodewarsUser user in codewarsUsersGroup.Users)
                setCompletedKataCountAsync(user);
            foreach (CodewarsUser user in codewarsUsersGroup.Users)
                populateCompletedKata(user);
        }
        private async Task populateCompletedKata(CodewarsUser user)
        {
            int totalPages = await getCompletedKataPageCountAsync(user);

            for (int currentPage = 0; currentPage < totalPages; currentPage++)
                await populateCompletedKataByPageAsync(user, currentPage);
        }
        private async Task setCompletedKataCountAsync(CodewarsUser user)
        {
            WebClient client = new WebClient();
            string str = null;

            try
            {
                str = await client.DownloadStringTaskAsync("https://www.codewars.com/api/v1/users/" + user.Username +
                    "/code-challenges/completed?page=1000");
            }
            catch
            {
                Console.WriteLine("Error getting a user's data from Codewars: " + user.Username);
                return;
            }

            if (str != null)
            {
                int.TryParse(JObject.Parse(str)["totalItems"].ToString(), out int totalCompletedKata);
                user.TotalCompletedKata = totalCompletedKata;
            }

            return;
        }
        private async Task<int> getCompletedKataPageCountAsync(CodewarsUser user)
        {
            WebClient client = new WebClient();
            string str = null;

            try
            {
                str = await client.DownloadStringTaskAsync("https://www.codewars.com/api/v1/users/" + user.Username +
                    "/code-challenges/completed?page=1000");
            }
            catch
            {
                Console.WriteLine("Error getting a user's data from Codewars: " + user.Username);
                return 0;
            }

            if (str != null)
            {
                int.TryParse(JObject.Parse(str)["totalPages"].ToString(), out int totalPages);
                return totalPages;
            }

            return 0;
        }
        private async Task populateCompletedKataByPageAsync(CodewarsUser user, int page)
        {
            WebClient client = new WebClient();
            string str = null;

            try
            {
                str = await client.DownloadStringTaskAsync("https://www.codewars.com/api/v1/users/" + user.Username
                    + "/code-challenges/completed?page=" + page);
            }
            catch
            {
                Console.WriteLine("Error getting a user's data from Codewars: " + user.Username);
                return;
            }

            if (str != null)
            {
                JObject response = JObject.Parse(str);

                foreach (JObject item in (JArray)response.GetValue("data"))
                    user.CompletedKata.Add(
                        new Kata(item["id"].ToString(), item["name"].ToString(), item["slug"].ToString())
                    );
            }
        }
    }
}
