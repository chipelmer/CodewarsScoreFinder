using System;
using System.Collections.Generic;
using System.Linq;

namespace CodewarsScoreFinder
{
    public static class Formatter
    {
        public static string GetTextFormattedForDisplay(CodewarsUsersGroup users)
        {
            users.SortUsersByScore();

            string[,] dataTable = new string[users.TotalCount + 1, 3]; // plus 1 for header, 3 for Username-Name-Score

            dataTable[0, 0] = "Username";
            dataTable[0, 1] = "Name";
            dataTable[0, 2] = "Score";

            for (int user = 0; user < users.Users.Count; user++)
            {
                dataTable[user + 1, 0] = users.Users[user].Username;
                dataTable[user + 1, 1] = users.Users[user].Name;
                dataTable[user + 1, 2] = users.Users[user].Score.ToString(); ;
            }

            return getTableFromArray(dataTable);
        }

        private static string getTableFromArray(string[,] arrayForTable)
        {
            var verticalLine = '\u2502';
            var verticalLineRightCrosspiece = '\u251C';
            var verticalLineLeftCrosspiece = '\u2524';
            var horizontalLine = '\u2500';
            var horizontalLineTopCrosspiece = '\u2534';
            var horizontalLineBottomCrosspiece = '\u252c';
            var topLeftCorner = '\u250c';
            var topRightCorner = '\u2510';
            var bottomRightCorner = '\u2518';
            var bottomLeftCorner = '\u2514';
            var doubleCrosspiece = '\u253c';

            standardizeLengthOfAllCellsInEachColumn(ref arrayForTable);

            var table = getHorizontalBorder(arrayForTable, topLeftCorner, horizontalLine, horizontalLineBottomCrosspiece, topRightCorner) + "\n";

            for (int row = 0; row < arrayForTable.GetLength(0); row++)
            {
                if (row == 1)
                {
                    table += getHorizontalBorder(arrayForTable, verticalLineRightCrosspiece, horizontalLine, doubleCrosspiece, verticalLineLeftCrosspiece) + "\n";
                }

                table += getTableRow(arrayForTable, row, verticalLine) + "\n";
            }

            table += getHorizontalBorder(arrayForTable, bottomLeftCorner, horizontalLine, horizontalLineTopCrosspiece, bottomRightCorner) + "\n";

            return table;
        }

        private static void standardizeLengthOfAllCellsInEachColumn(ref string[,] table)
        {
            for (int col = 0; col < table.GetLength(1); col++)
            {
                var maxWidth = 0;
                for (int row = 0; row < table.GetLength(0); row++)
                    if (table[row, col].Length > maxWidth)
                        maxWidth = table[row, col].Length;

                for (int row = 0; row < table.GetLength(0); row++)
                    table[row, col] += new string(' ', maxWidth - table[row, col].Length);
            }
        }

        private static string getTableRow(string[,] table, int row, char cellDivider)
        {
            var rowString = "";
            for (int col = 0; col < table.GetLength(1); col++)
                rowString += cellDivider + table[row, col];
            
            rowString += cellDivider;
            return rowString;
        }

        private static string getHorizontalBorder(string[,] table, char left, char middle, char middleDivider, char right)
        {
            var border = left.ToString();
            for (int col = 0; col < table.GetLength(1); col++)
                border += new string(middle, table[0, col].Length) + middleDivider;
            
            return border.Remove(border.Length - 1) + right;
        }
    }
}
