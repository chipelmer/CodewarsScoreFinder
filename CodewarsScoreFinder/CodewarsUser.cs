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

        public List<Kata> CompletedKata { get; set; } = new List<Kata>();
    }
}
