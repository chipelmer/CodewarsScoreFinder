using System;
using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public static class Formatter
    {
        private static int getMaxNameCharacterCount(List<CodewarsUser> codewarsUsers)
        {
            var max = 0;
            foreach(var user in codewarsUsers)
            {
                max = user.Name.Length > max ? user.Name.Length : max;
            }

            return max;
        }

        private static int getMaxScoreCharacterCount(List<CodewarsUser> codewarsUsers)
        {
            var max = 0;
            foreach (var user in codewarsUsers)
            {
                max = user.Score.ToString().Length > max ? user.Score.ToString().Length : max;
            }

            return max;
        }

        public static string GetTextFormattedForDisplay(List<CodewarsUser> codewarsUsers)
        {
            var usernameText = "Username:";
            var nameText = "Name:";
            var scoreText = "Score:";

            // using Linq
            var maxUsername = Math.Max(codewarsUsers.Max(x => x.Username.Length), usernameText.Length);

            // using methods
            var maxName = Math.Max(getMaxNameCharacterCount(codewarsUsers), nameText.Length);
            var maxScore = Math.Max(getMaxScoreCharacterCount(codewarsUsers), scoreText.Length);
            var extraSpace = " | ";

            var formattedString = extraSpace + usernameText + new string(' ', maxUsername - usernameText.Length) + extraSpace 
                + nameText + new string(' ', maxName - nameText.Length) + extraSpace
                + scoreText + new string(' ', maxScore - scoreText.Length) + extraSpace
                + "\n";
            foreach(var user in codewarsUsers)
            {
                formattedString += extraSpace;
                formattedString += user.Username + new string(' ', maxUsername - user.Username.Length) + extraSpace;
                formattedString += user.Name + new string(' ', maxName - user.Name.Length) + extraSpace;
                formattedString += user.Score.ToString() + new string(' ', maxScore - user.Score.ToString().Length) + extraSpace;
                formattedString += "\n";
            }

            return formattedString;
        }
    }
}
