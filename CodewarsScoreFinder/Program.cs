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
            
            new System.Threading.Thread(() => dataFinder.PopulateScores(codewarsUsers)).Start();

            int scoresPopulated = 0;
            int dotCount = 0;
            int maxDots = 8;
            Console.CursorVisible = false;
            while(scoresPopulated < codewarsUsers.Count)
            {
                scoresPopulated = CodewarsUser.UsersWithScoresCount(codewarsUsers);
                Console.CursorLeft = 0;
                Console.Write(scoresPopulated + "/" + codewarsUsers.Count);

                System.Threading.Thread.Sleep(200);
                Console.CursorLeft += 2;
                Console.Write(new string('.', dotCount) + new string(' ', maxDots - dotCount));

                dotCount = dotCount > maxDots - 1 ? 0 : dotCount + 1;
            }

            Console.CursorLeft = 0;
            Console.Write(scoresPopulated + "/" + codewarsUsers.Count);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Beep(440, 250);
            Console.Beep(660, 250);
            Console.Beep(880, 250);
            Console.Write(" ---> DONE" + new string(' ', maxDots));
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(1500);
            Console.CursorVisible = true;

            codewarsUsers = codewarsUsers.OrderByDescending(x => x.Score).ToList();

            Console.Clear();
            Console.WriteLine(Formatter.GetTextFormattedForDisplay(codewarsUsers));
            Console.ReadLine();
        }
    }
}
