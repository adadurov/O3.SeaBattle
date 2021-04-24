using System;

namespace O3.SeaBattle.Logic
{
    public static class LocationFactory
    {
        /// <summary>
        /// creates a location from a string of the following format:  <char><decimal_number>
        /// Where char denotes a column and the number denotes a row.
        /// The char must not exceed A + 255
        /// The number must not exceed 255
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static Location Create(string loc)
        {
            if (string.IsNullOrWhiteSpace(loc))
                throw new ArgumentException("Empty location specified. Expected: <char><number>");

            if (loc.Length < 2)
            {
                throw new ArgumentException("Bad location specified. Expected: <char><number>");
            }

            var col = loc[^1];

            if (col < 'A')
            {
                throw new ArgumentException("Column must start from 'A'");
            }

            if (col > 'A' + 255)
            {
                throw new ArgumentException("Column must not exceed 'A'+255");
            }

            return new Location(byte.Parse(loc[..^1]), col);
        }

    }
}
