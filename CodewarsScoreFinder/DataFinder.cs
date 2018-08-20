using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace CodewarsScoreFinder
{
    class DataFinder
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

        public void PopulateScores(List<CodewarsUser> users)
        {
            WebClient client = new WebClient();

            foreach (var user in users)
            {
                string str = null;
                try
                {
                    str = client.DownloadString("https://www.codewars.com/api/v1/users/" + user.Username);
                }
                catch
                {
                    Console.WriteLine("Error getting a user's data from Codewars.");
                    Thread.Sleep(250);
                }

                if (str != null)
                {
                    JObject response = JObject.Parse(str);
                    int.TryParse(response.GetValue("honor").ToString(), out int score);
                    user.Score = score;
                }
            }
        }
    }
}
