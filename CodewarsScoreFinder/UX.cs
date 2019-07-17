using System;
using System.Threading;
using System.Collections.Generic;

namespace CodewarsScoreFinder
{
    public static class UX
    {
        public static void SetConsoleColors(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Clear();
        }

        public enum LoadingOptions { CyclingBar, SideToSideBar, Dots }

        public static void SetWindowSizeMinimum(int desiredHeight, int desiredWidth)
        {
            int resultingHeight = Math.Min(Console.LargestWindowHeight, desiredHeight);
            resultingHeight = Math.Max(resultingHeight, Console.WindowHeight);
            Console.WindowHeight = resultingHeight;

            int resultingWidth = Math.Min(Console.LargestWindowWidth, desiredWidth);
            resultingWidth = Math.Max(resultingWidth, Console.WindowWidth);
            Console.WindowWidth = resultingWidth;
        }

        public static void DisplayLoadingWindow(LoadingOptions loadingOption, Func<int> progress, Func<int> total,
            string generalMessage)
        {
            Console.WriteLine(generalMessage);

            int maxBarSpace = 22;
            int barLenth = 4;

            CyclingBar cyclingBar = new CyclingBar(barLenth, maxBarSpace);
            SideToSideBar sideToSideBar = new SideToSideBar(barLenth, maxBarSpace);

            int maxDots = 8;
            int dotCount = 0;

            Console.CursorVisible = false;
            while (progress() < total())
            {
                Console.CursorLeft = 0;
                Console.Write(progress() + "/" + total());
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
            Console.Write(progress() + "/" + total());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" ---> DONE" + new string(' ', Math.Max(maxDots, maxBarSpace)));
            Thread.Sleep(1500);
        }

        public static void ScreenFlashThenDisplay(string text)
        {
            Console.Clear();

            for (int i = 0; i < 2; i++)
            {
                if (i % 2 == 0)
                    SetConsoleColors(ConsoleColor.DarkCyan, ConsoleColor.White);
                else
                    SetConsoleColors(ConsoleColor.Black, ConsoleColor.White);

                Console.WriteLine(text);
                Thread.Sleep(1000);
            }

            Console.Clear();
            Console.WriteLine(text);
        }

        private class CyclingBar
        {
            public CyclingBar(int greenWidth, int totalWidth, int speedDelayInMilliseconds = 20)
            {
                this.speedDelayInMilliseconds = speedDelayInMilliseconds;
                for (int i = 0; i < totalWidth; i++)
                    values.Add(i < greenWidth);
            }

            private List<bool> values = new List<bool>();
            private int speedDelayInMilliseconds;

            public void MoveBar()
            {
                Thread.Sleep(speedDelayInMilliseconds);

                values.Insert(0, values[values.Count - 1]);
                values.RemoveAt(values.Count - 1);

                foreach (bool nextValue in values)
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

                for (int i = 0; i < totalWidth; i++)
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

                foreach (bool value in values)
                {
                    Console.BackgroundColor = value ? ConsoleColor.Green : default(ConsoleColor);
                    Console.Write(' ');
                }

                Console.BackgroundColor = default(ConsoleColor);
            }
        }
    }
}
