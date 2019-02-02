
namespace CodewarsScoreFinder
{
    public static class Formatter
    {
        public static string GetTableFromArray(string[,] arrayForTable)
        {
            char verticalLine = '\u2502';
            char verticalLineRightCrosspiece = '\u251C';
            char verticalLineLeftCrosspiece = '\u2524';
            char horizontalLine = '\u2500';
            char horizontalLineTopCrosspiece = '\u2534';
            char horizontalLineBottomCrosspiece = '\u252c';
            char topLeftCorner = '\u250c';
            char topRightCorner = '\u2510';
            char bottomRightCorner = '\u2518';
            char bottomLeftCorner = '\u2514';
            char doubleCrosspiece = '\u253c';

            standardizeLengthOfAllCellsInEachColumn(ref arrayForTable);

            string table = getHorizontalBorder(arrayForTable, topLeftCorner, horizontalLine, horizontalLineBottomCrosspiece, topRightCorner) + "\n";

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
                int maxWidth = 0;
                for (int row = 0; row < table.GetLength(0); row++)
                    if (table[row, col].Length > maxWidth)
                        maxWidth = table[row, col].Length;

                for (int row = 0; row < table.GetLength(0); row++)
                    table[row, col] += new string(' ', maxWidth - table[row, col].Length);
            }
        }
        private static string getTableRow(string[,] table, int row, char cellDivider)
        {
            string rowString = "";
            for (int col = 0; col < table.GetLength(1); col++)
                rowString += cellDivider + table[row, col];
            
            rowString += cellDivider;
            return rowString;
        }
        private static string getHorizontalBorder(string[,] table, char left, char middle, char middleDivider, char right)
        {
            string border = left.ToString();
            for (int col = 0; col < table.GetLength(1); col++)
                border += new string(middle, table[0, col].Length) + middleDivider;
            
            return border.Remove(border.Length - 1) + right;
        }
    }
}
