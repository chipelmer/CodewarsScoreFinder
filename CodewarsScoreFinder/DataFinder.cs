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
                Console.ReadLine();
            }

            return usernames;
        }

        public void PopulateScores(CodewarsUsersGroup users)
        {
            foreach (var user in users.Users)
            {
                populateScore(user);
            }
        }

        private async Task populateScore(CodewarsUser user)
        {
            var client = new WebClient();
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
                var response = JObject.Parse(str);
                int.TryParse(response.GetValue("honor").ToString(), out int score);
                user.Score = score;
            }
        }
    }
}
