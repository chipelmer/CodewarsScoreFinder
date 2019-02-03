﻿using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public class CodewarsUsersGroup
    {
        public CodewarsUsersGroup(string file)
        {
            Users = new List<CodewarsUser>();

            string[,] data = new DataFinder().ReadCsvFile(file, "Unable to read usernames file.", true);

            if (data == null)
                return;

            for (int row = 0; row < data.GetLength(0); row++)
            {
                CodewarsUser newUser = new CodewarsUser(data[row, 0]);
                newUser.Name = data[row, 1].Length > 0 ? data[row, 1] : "[No Name]";
                Users.Add(newUser);
            }
        }

        public List<CodewarsUser> Users { get; private set; }
        public int TotalCount { get => Users.Count; }
        public int PopulatedScoresCount { get => Users.Count(x => x.Score > 0); }
        public int PopulatedKataListCount { get => Users.Count(x => x.CompletedKata.Count > 0); }

        public void SortUsersByScore() => Users = Users.OrderByDescending(x => x.Score).ToList();

        public void PopulateUserScores()
        {
            new System.Threading.Thread(() => new DataFinder().PopulateScores(this)).Start();
            UX.DisplayLoadingWindow(UX.LoadingOptions.CyclingBar,
                () => PopulatedScoresCount,
                () => TotalCount,
                "Getting data from Codewars...");
        }

        public void PopulateUserCompletedKataLists()
        {
            new System.Threading.Thread(() => new DataFinder().PopulateCompletedKata(this)).Start();
            UX.DisplayLoadingWindow(UX.LoadingOptions.CyclingBar,
                () => PopulatedKataListCount,
                () => TotalCount,
                "Getting data from Codewars...");
        }
    }
}
