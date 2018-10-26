﻿using System;

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
                return;

            var users = new CodewarsUsersGroup(usernames);
            if (users == null || users.TotalCount < 1)
                return;

            new System.Threading.Thread(() => dataFinder.PopulateScores(users)).Start();
            UX.DisplayLoadingWindow(dataFinder, users, UX.LoadingOptions.CyclingBar);

            UX.DisplayResults(users);
            UX.DisplayFinalOptions();
        }
    }
}
