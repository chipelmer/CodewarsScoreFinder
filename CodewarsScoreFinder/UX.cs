using System;
using System.Threading;
using System.Collections.Generic;

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

        public enum LoadingOptions { CyclingBar, SideToSideBar, Dots }

        public static void DisplayLoadingWindow(DataFinder dataFinder, CodewarsUsersGroup users, LoadingOptions loadingOption)
        {
            Console.WriteLine("Getting data from Codewars...");

            var maxBarSpace = 22;
            var barLenth = 4;

            var cyclingBar = new CyclingBar(barLenth, maxBarSpace);
            var sideToSideBar = new SideToSideBar(barLenth, maxBarSpace);

            var maxDots = 8;
            var dotCount = 0;

            Console.CursorVisible = false;
            while (users.PopulatedScoresCount < users.TotalCount)
            {
                Console.CursorLeft = 0;
                Console.Write(users.PopulatedScoresCount + "/" + users.TotalCount);
                Console.CursorLeft += 2;

                if (loadingOption == LoadingOptions.CyclingBar)
                    cyclingBar.MoveBar();
                else if (loadingOption == LoadingOptions.SideToSideBar)
                    sideToSideBar.MoveBar();
                else if (loadingOption == LoadingOptions.Dots)
                {
                    Thread.Sleep(200);
                    Console.Write(new string('.', dotCount) + new string(' ', maxDots - dotCount));
                    dotCount = dotCount > maxDots - 1 ? 0 : dotCount + 1;
                }
            }

            Console.CursorLeft = 0;
            Console.Write(users.PopulatedScoresCount + "/" + users.TotalCount);

            PlayDoneSound();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" ---> DONE" + new string(' ', Math.Max(maxDots, maxBarSpace)));
            Thread.Sleep(1500);
        }

        public static void DisplayResults(CodewarsUsersGroup users)
        {
            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    SetColors(ConsoleColor.DarkCyan, ConsoleColor.White);
                else
                    SetColors(ConsoleColor.Black, ConsoleColor.White);

                string results = Formatter.GetTextFormattedForDisplay(users);
                Console.WindowHeight = Math.Min(Console.LargestWindowHeight, results.Split("\n").Length + 2);
                Console.WriteLine(results);
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

        private class CyclingBar
        {
            public CyclingBar(int greenWidth, int totalWidth, int speedDelayInMilliseconds = 20)
            {
                this.speedDelayInMilliseconds = speedDelayInMilliseconds;
                for (var i = 0; i < totalWidth; i++)
                    values.Add(i < greenWidth);
            }

            private List<bool> values = new List<bool>();
            private int speedDelayInMilliseconds;

            public void MoveBar()
            {
                Thread.Sleep(speedDelayInMilliseconds);

                values.Insert(0, values[values.Count - 1]);
                values.RemoveAt(values.Count - 1);

                foreach (var nextValue in values)
                {
                    Console.BackgroundColor = nextValue ? ConsoleColor.Green : default(ConsoleColor);
                    Console.Write(' ');
                }

                Console.BackgroundColor = default(ConsoleColor);
            }
        }

        private class SideToSideBar
        {
            public SideToSideBar(int greenWidth, int totalWidth, int speedDelayInMilliseconds = 20)
            {
                this.speedDelayInMilliseconds = speedDelayInMilliseconds;

                for (var i = 0; i < totalWidth; i++)
                    values.Add(i < greenWidth);
            }

            private List<bool> values = new List<bool>();
            private int direction = 1;
            private int speedDelayInMilliseconds;

            public void MoveBar()
            {
                Thread.Sleep(speedDelayInMilliseconds);

                if (direction > 0)
                {
                    values.Insert(0, values[values.Count - 1]);
                    values.RemoveAt(values.Count - 1);
                    if (values[values.Count - 1])
                        direction *= -1;
                }
                else
                {
                    values.Add(values[0]);
                    values.RemoveAt(0);
                    if (values[0])
                        direction *= -1;
                }

                foreach (var value in values)
                {
                    Console.BackgroundColor = value ? ConsoleColor.Green : default(ConsoleColor);
                    Console.Write(' ');
                }

                Console.BackgroundColor = default(ConsoleColor);
            }
        }
    }
}
