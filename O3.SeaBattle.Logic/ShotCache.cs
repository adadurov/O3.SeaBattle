using System.Collections;

namespace O3.SeaBattle.Logic
{
    public class ShotCache
    {
        private readonly BitArray _bits;
        private readonly short _cols;

        public ShotCache(short rows, short cols)
        {
            _cols = cols;
            _bits = new BitArray(rows * cols);
        }

        public void AddShotMark(Cell location)
        {
            var index = GetIndex(location);
            _bits.Set(index, true);
        }

        public bool EverFiredOn(Cell location)
        {
            var index = GetIndex(location);
            return _bits[index];
        }

        private int GetIndex(Cell location)
        {
            return location.Row * _cols + location.Col;
        }
    }
}
