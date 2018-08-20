using System;
using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFinder = new DataFinder();

            Console.WriteLine("Getting users...");
            var usernames = dataFinder.GetUsernames("Usernames.csv");
            if (usernames == null)
                Environment.Exit(-1);

            var codewarsUsers = CodewarsUser.ParseUsers(usernames);
            if (codewarsUsers == null || codewarsUsers.Count < 1)
                Environment.Exit(-1);

            Console.WriteLine("Getting data from Codewars...");
            dataFinder.PopulateScores(codewarsUsers);

            codewarsUsers = codewarsUsers.OrderByDescending(x => x.Score).ToList();

            Console.Clear();
            Console.WriteLine(Formatter.GetTextFormattedForDisplay(codewarsUsers));
            Console.ReadLine();
        }
    }
}
