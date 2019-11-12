using System;

namespace CodewarsScoreFinder
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Getting users...");
            CodewarsUsersGroup codewarsUsersGroup = new CodewarsUsersGroup($"Usernames{System.IO.Path.DirectorySeparatorChar}Usernames.csv");
            if (codewarsUsersGroup == null || codewarsUsersGroup.TotalCount < 1)
            {
                Console.WriteLine("No users found.");
                Console.ReadLine();
                return;
            }

            new Controller(codewarsUsersGroup).DisplayInitialOptions();
        }
    }
}
