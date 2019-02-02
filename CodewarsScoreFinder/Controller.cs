using System;
using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    public class Controller
    {
        public CodewarsUsersGroup CodewarsUsersGroup { get; set; }

        public Controller(CodewarsUsersGroup codewarsUsersGroup)
        {
            CodewarsUsersGroup = codewarsUsersGroup;
        }

        public void DisplayInitialOptions()
        {
            int choice = 0;
            while (choice == 0)
            {
                Console.Clear();
                choice = getUserIntegerResponse(
                    "1) Show Leaderboard\n" +
                    "2) Check Completed Kata\n" +
                    "3) Exit\n" +
                    "\nWhat would you like to do?", 1, 3);

                switch (choice)
                {
                    case 1:
                        showCurrentLeaderboard();
                        break;
                    case 2:
                        showKataListsProgress();
                        Console.ReadLine();
                        break;
                    case 3:
                        return;
                }

                choice = 0;
            }
        }

        private void displayGoBackPrompt()
        {
            Console.CursorVisible = false;
            Console.WriteLine("(Press any key to go back)");
            Console.ReadKey(true);
        }

        private int getUserIntegerResponse(string question, int lowerBound, int upperBound)
        {
            Console.WriteLine(question);
            string responseStr = Console.ReadLine();

            int response = lowerBound - 1;
            int wrongCount = 0;
            while (int.TryParse(responseStr, out response) == false || response < lowerBound || response > upperBound)
            {
                wrongCount += 1;
                if (wrongCount >= 3)
                {
                    wrongCount = 0;
                    Console.Clear();
                    Console.WriteLine(question);
                }

                Console.WriteLine();
                Console.WriteLine($"Please choose a number from {lowerBound} to {upperBound}.");
                responseStr = Console.ReadLine();
            }

            return response;
        }

        private void showCurrentLeaderboard()
        {
            string leaderboard = new Leaderboard(CodewarsUsersGroup).ToString();
            Console.WindowHeight = Math.Min(Console.LargestWindowHeight, leaderboard.Split("\n").Length + 3);
            UX.ScreenFlashThenDisplay(leaderboard);
            displayGoBackPrompt();
        }

        private void showKataListsProgress()
        {
            string kataListBoard = new KataListBoard(CodewarsUsersGroup).ToString();
            Console.WindowHeight = Math.Min(Console.LargestWindowHeight, kataListBoard.Split("\n").Length + 3);
            UX.ScreenFlashThenDisplay(kataListBoard);
            displayGoBackPrompt();
        }
    }
}
