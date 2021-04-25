using O3.SeaBattle.Logic;
using System;

namespace O3.SeaBattle.Service.Parsers
{
    public class LocationParser : ILocationParser
    {
        private readonly int _rowOrigin;
        private readonly char _columnOrigin;
        private readonly int _maxSize;

        public LocationParser(int rowOrigin, char columnOrigin, int maxSize)
        {
            _rowOrigin = rowOrigin;
            _columnOrigin = columnOrigin;
            _maxSize = maxSize;
        }

        public int RowOrigin => _rowOrigin;

        public char ColumnOrigin => _columnOrigin;

        /// <summary>
        /// Creates a location from a string of the following format:  <decimal_number><char>
        /// Where char denotes a column and the number denotes a row.
        /// The char must not exceed Game.ColumnOrigin + Game.MaxSize
        /// The number must start at Game.RowOrigin and not exceed Game.MaxSize
        /// </summary>
        public Cell Parse(string locationSpec)
        {
            if (string.IsNullOrWhiteSpace(locationSpec))
            {
                throw new ArgumentNullException(nameof(locationSpec));
            }

            if (locationSpec.Length < 2)
            {
                throw new ArgumentException(
                    $"Bad location specified: {locationSpec}. Expected: <decimal_number><letter> -- e.g. '{_rowOrigin}{_columnOrigin+1}'");
            }

            var col = locationSpec[^1];

            var colOrigin = _columnOrigin;

            if (col < colOrigin)
            {
                throw new ArgumentException($"Column (letter) must not be less than '{colOrigin}'");
            }

            if (col > _columnOrigin + _maxSize)
            {
                throw new ArgumentException($"Column (letter) must not exceed '{colOrigin}'+{_maxSize}");
            }

            var row = short.Parse(locationSpec[..^1]);
            if (row < _rowOrigin || row > (_rowOrigin + _maxSize))
            {
                throw new ArgumentOutOfRangeException(
                    $"The row index must not be less than {_rowOrigin} and must not exceed ({_rowOrigin+_maxSize})"
                );
            }

            return new Cell(row - _rowOrigin, (short)(col - _columnOrigin));
        }
    }
}
