using System;

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
            ConsoleKey choice = ConsoleKey.NoName;
            while (choice == ConsoleKey.NoName)
            {
                Console.Clear();
                choice = getUserKeyPress(
                    "1) Show Leaderboard\n" +
                    "2) Check Completed Kata\n" +
                    "3) Exit\n" +
                    "\nWhat would you like to do?");

                switch (choice)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        showCurrentLeaderboard();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        showKataListsProgress();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return;
                }

                choice = ConsoleKey.NoName;
            }
        }

        private int refreshOrGoBackPrompt()
        {
            Console.CursorVisible = false;
            Console.WriteLine("(Press R to refresh, any other key to go back)");
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.R)
                return 1;

            return 0;
        }

        private ConsoleKey getUserKeyPress(string question)
        {
            Console.WriteLine(question);
            return Console.ReadKey(true).Key;
        }

        private void showCurrentLeaderboard()
        {
            CodewarsUsersGroup.PopulateUserScores();
            string leaderboard = new Leaderboard(CodewarsUsersGroup).ToString();
            UX.SetWindowHeightMinimum(leaderboard.Split("\n").Length + 3);
            UX.ScreenFlashThenDisplay(leaderboard);
            
            if (refreshOrGoBackPrompt() == 1)
            {
                CodewarsUsersGroup.RefreshUserScores();
                showCurrentLeaderboard();
            }
        }

        private void showKataListsProgress()
        {
            CodewarsUsersGroup.PopulateUserCompletedKataLists();
            string kataListBoard = new KataListBoard(CodewarsUsersGroup).ToString();
            UX.SetWindowHeightMinimum(kataListBoard.Split("\n").Length + 3);
            UX.ScreenFlashThenDisplay(kataListBoard);

            if (refreshOrGoBackPrompt() == 1)
            {
                CodewarsUsersGroup.RefreshUserCompletedKataLists();
                showKataListsProgress();
            }
        }
    }
}
