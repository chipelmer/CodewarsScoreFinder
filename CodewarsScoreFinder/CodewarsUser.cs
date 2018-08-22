using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public class CodewarsUser
    {
        public CodewarsUser(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public static int UsersWithScoresCount(List<CodewarsUser> codewarsUsers) => codewarsUsers.Count(x => x.Score > 0);

        public static List<CodewarsUser> ParseUsers(string[] users)
        {
            if (users == null)
                return new List<CodewarsUser>() { new CodewarsUser("N/A") { Name = "None", Score = 0 } };

            var userList = new List<CodewarsUser>();

            foreach (var user in users)
            {
                var data = user.Split(',');
                var newUser = new CodewarsUser(data[0]);
                newUser.Name = data[1].Trim();
                userList.Add(newUser);
            }

            return userList;
        }
    }
}
