using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CodewarsScoreFinder
{
    public class DataFinder
    {
        public string[] GetUsernames(string file)
        {
            string[] usernames = null;
            try
            {
                usernames = File.ReadAllLines(file);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to get usernames from file.");
                Console.WriteLine("File: " + file);
                Console.ReadLine();
            }

            return usernames;
        }

        public Kata[] GetKataList(string file)
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(file);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to get kata list from file.");
                Console.WriteLine("File: " + file);
                Console.ReadLine();
            }

            Kata[] kata = new Kata[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] kataCells = lines[i].Split(',');
                kata[i] = new Kata(kataCells[0], kataCells[1], kataCells[2], kataCells[3]);
            }

            return kata;
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
                populateCompletedKata(user);
        }
        private async Task populateCompletedKata(CodewarsUser user)
        {
            int totalPages = getCompletedKataPageCount(user);
            int currentPage = 0;

            for (int i = 0; i < totalPages; i++)
                await populateCompletedKataByPage(user, currentPage);
        }
        private int getCompletedKataPageCount(CodewarsUser user)
        {
            WebClient client = new WebClient();
            string str = null;

            try
            {
                str = client.DownloadString("https://www.codewars.com/api/v1/users/" + user.Username +
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
        private async Task populateCompletedKataByPage(CodewarsUser user, int page)
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
