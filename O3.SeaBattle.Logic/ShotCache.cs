using System;
using System.Collections;

namespace O3.SeaBattle.Logic
{
    public class ShotCache
    {
        private readonly BitArray _bits;
        private readonly short _rows, _cols;

        public ShotCache(short rows, short cols)
        {
            _rows = rows;
            _cols = cols;
            _bits = new BitArray(_rows * _cols);
        }

        public void AddShotMark(Location location)
        {
            var index = GetIndex(location);
            _bits.Set(index, true);
        }

        public bool EverFiredOn(Location location)
        {
            var index = GetIndex(location);
            return _bits[index];
        }

        private int GetIndex(Location location)
        {
            var col = location.Col - 'A';
            var row = location.Row - 1;

            return row * _rows + col;
        }
    }
}
