using System;
using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public class CodewarsUsersGroup
    {
        public CodewarsUsersGroup(string[] users)
        {
            Users = parseUsers(users);
        }

        public List<CodewarsUser> Users { get; private set; }
        public int TotalCount
        {
            get => Users.Count;
        }
        public int PopulatedScoresCount
        {
            get => Users.Count(x => x.Score > 0);
        }
        public int PopulatedKataListCount
        {
            get => Users.Count(x => x.CompletedKata.Count > 0);
        }

        public void SortUsersByScore()
        {
            Users = Users.OrderByDescending(x => x.Score).ToList();
        }

        private List<CodewarsUser> parseUsers(string[] users)
        {
            if (users == null)
                return new List<CodewarsUser>() { new CodewarsUser("N/A") { Name = "N/A", Score = 0 } };

            List<CodewarsUser> userList = new List<CodewarsUser>();

            foreach (string user in users)
            {
                string[] data = user.Split(',');
                CodewarsUser newUser = new CodewarsUser(data[0]);

                if (data.Length > 1)
                    newUser.Name = data[1].Trim();
                else
                    newUser.Name = "[No Name]";

                userList.Add(newUser);
            }

            return userList;
        }
    }
}
