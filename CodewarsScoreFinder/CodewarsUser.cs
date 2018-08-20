using System.Collections.Generic;

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


        public static List<CodewarsUser> ParseUsers(string[] users)
        {
            if (users == null)
                return new List<CodewarsUser>() { new CodewarsUser("N/A") { Name = "None", Score = 0 } };

            List<CodewarsUser> userList = new List<CodewarsUser>();

            foreach (string user in users)
            {
                string[] data = user.Split(',');
                CodewarsUser newUser = new CodewarsUser(data[0]);
                newUser.Name = data[1].Trim();
                userList.Add(newUser);
            }

            return userList;
        }
    }
}
