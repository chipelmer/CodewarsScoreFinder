using System;

namespace CodewarsScoreFinder
{
    class Program
    {
        public static void Main(string[] args)
        {
            var dataFinder = new DataFinder();

            Console.WriteLine("Getting users...");
            var usernames = dataFinder.GetUsernames("Usernames.csv");
            if (usernames == null)
                Environment.Exit(-1);

            var codewarsUsers = CodewarsUser.ParseUsers(usernames);
            if (codewarsUsers == null || codewarsUsers.Count < 1)
                Environment.Exit(-1);

            new System.Threading.Thread(() => dataFinder.PopulateScores(codewarsUsers)).Start();
            UX.DisplayLoadingWindow(dataFinder, codewarsUsers, UX.LoadingOptions.Bar);

            UX.DisplayResults(codewarsUsers);
            UX.DisplayFinalOptions();
        }
    }
}
