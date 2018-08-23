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

            var users = new CodewarsUsersGroup(usernames);
            if (users == null || users.TotalCount < 1)
                Environment.Exit(-1);

            new System.Threading.Thread(() => dataFinder.PopulateScores(users)).Start();
            UX.DisplayLoadingWindow(dataFinder, users, UX.LoadingOptions.Bar);

            UX.DisplayResults(users);
            UX.DisplayFinalOptions();
        }
    }
}
