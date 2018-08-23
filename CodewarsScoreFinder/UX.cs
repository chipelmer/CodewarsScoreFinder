using System;
using System.Collections.Generic;
using System.Threading;

namespace CodewarsScoreFinder
{
    public static class UX
    {
        public static void PlayDoneSound()
        {
            Console.Beep(440, 200);
            Console.Beep(440, 200);
            Console.Beep(660, 200);
            Console.Beep(660, 200);
            Console.Beep(880, 500);
        }

        public static void SetColors(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Clear();
        }

        public enum LoadingOptions { Bar, Dots }

        public static void DisplayLoadingWindow(DataFinder dataFinder, List<CodewarsUser> codewarsUsers, LoadingOptions loadingOption)
        {
            Console.WriteLine("Getting data from Codewars...");

            var barLocation = 0;
            var maxBarSpace = 30;
            var barLenth = 4;
            var direction = 1;

            var maxDots = 8;
            var dotCount = 0;

            var scoresPopulated = 0;
            Console.CursorVisible = false;
            while (scoresPopulated < codewarsUsers.Count)
            {
                scoresPopulated = CodewarsUser.UsersWithScoresCount(codewarsUsers);

                Console.CursorLeft = 0;
                Console.Write(scoresPopulated + "/" + codewarsUsers.Count);

                if(loadingOption == LoadingOptions.Bar)
                {
                    Thread.Sleep(20);
                    Console.CursorLeft += 2;
                    Console.BackgroundColor = default(ConsoleColor);
                    Console.Write(new string(' ', barLocation));
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write(new string(' ', barLenth));
                    Console.BackgroundColor = default(ConsoleColor);
                    Console.Write(new string(' ', maxBarSpace - barLenth - barLocation));
                    barLocation += direction;
                    if (barLocation + barLenth >= maxBarSpace || (barLocation <= 0)) direction *= -1;
                }
                else if(loadingOption == LoadingOptions.Dots)
                {
                    Thread.Sleep(200);
                    Console.CursorLeft += 2;
                    Console.Write(new string('.', dotCount) + new string(' ', maxDots - dotCount));

                    dotCount = dotCount > maxDots - 1 ? 0 : dotCount + 1;
                }
            }

            Console.CursorLeft = 0;
            Console.Write(scoresPopulated + "/" + codewarsUsers.Count);

            PlayDoneSound();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" ---> DONE" + new string(' ', Math.Max(maxDots, maxBarSpace)));
            Thread.Sleep(1500);
        }

        public static void DisplayResults(List<CodewarsUser> codewarsUsers)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    SetColors(ConsoleColor.DarkCyan, ConsoleColor.White);
                else
                    SetColors(ConsoleColor.Black, ConsoleColor.White);

                Console.WriteLine(Formatter.GetTextFormattedForDisplay(codewarsUsers));
                Thread.Sleep(100);
            }
        }

        public static void DisplayFinalOptions()
        {
            Console.CursorVisible = false;
            Console.WriteLine("(Press R to refresh, any other key to exit)");

            if (Console.ReadKey(true).Key == ConsoleKey.R)
            {
                Console.ResetColor();
                Console.Clear();
                Program.Main(new string[0]);
            }
        }
    }
}
