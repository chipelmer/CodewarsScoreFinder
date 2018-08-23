using System;
using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public static class Formatter
    {
        private static int getMaxNameCharacterCount(List<CodewarsUser> codewarsUsers)
            => codewarsUsers.Max(x => x.Name.Length);

        private static int getMaxScoreCharacterCount(List<CodewarsUser> codewarsUsers)
            => codewarsUsers.Max(x => x.Score.ToString().Length);

        public static string GetTextFormattedForDisplay(CodewarsUsersGroup users)
        {
            users.SortUsersByScore();

            var usernameText = "Username:";
            var nameText = "Name:";
            var scoreText = "Score:";

            // using Linq
            var maxUsername = Math.Max(users.Users.Max(x => x.Username.Length), usernameText.Length);

            // using methods
            var maxName = Math.Max(getMaxNameCharacterCount(users.Users), nameText.Length);
            var maxScore = Math.Max(getMaxScoreCharacterCount(users.Users), scoreText.Length);
            var extraSpace = " | ";

            var formattedString = extraSpace + usernameText + new string(' ', maxUsername - usernameText.Length) + extraSpace
                + nameText + new string(' ', maxName - nameText.Length) + extraSpace
                + scoreText + new string(' ', maxScore - scoreText.Length) + extraSpace
                + "\n";
            foreach (var user in users.Users)
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
