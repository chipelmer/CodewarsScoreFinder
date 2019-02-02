using System;

namespace CodewarsScoreFinder
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Getting users...");
            var usernames = new DataFinder().GetUsernames("Usernames.csv");
            if (usernames == null)
                return;

            var codewarsUsersGroup = new CodewarsUsersGroup(usernames);
            if (codewarsUsersGroup == null || codewarsUsersGroup.TotalCount < 1)
            {
                Console.WriteLine("No users found.");
                Console.ReadLine();
                return;
            }

            new System.Threading.Thread(() => dataFinder.PopulateScores(users)).Start();
            UX.DisplayLoadingWindow(dataFinder, users, UX.LoadingOptions.CyclingBar);

            UX.DisplayResults(users);
            UX.DisplayFinalOptions();
        }
    }
}
