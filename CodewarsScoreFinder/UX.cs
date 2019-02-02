﻿using System;
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

        public static void DisplayLoadingWindow(LoadingOptions loadingOption, Func<int> progress, Func<int> total,
            string generalMessage)
        {
            Console.WriteLine(generalMessage);

            var maxBarSpace = 22;
            var barLenth = 4;

            var cyclingBar = new CyclingBar(barLenth, maxBarSpace);
            var sideToSideBar = new SideToSideBar(barLenth, maxBarSpace);

            var maxDots = 8;
            var dotCount = 0;

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

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    SetConsoleColors(ConsoleColor.DarkCyan, ConsoleColor.White);
                else
                    SetConsoleColors(ConsoleColor.Black, ConsoleColor.White);

                Console.WriteLine(text);
                Thread.Sleep(100);
            }

            Console.Clear();
            Console.WriteLine(text);
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
