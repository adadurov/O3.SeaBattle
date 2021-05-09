using System.Collections;

namespace O3.SeaBattle.Logic
{
    public class ShotMarksMatrix
    {
        private readonly BitArray _bits;
        private readonly int _cols;
        private int _marksCount = 0;

        public ShotMarksMatrix(int rows, int cols)
        {
            _cols = cols;
            _bits = new BitArray(rows * cols);
        }

        public int MarksCount => _marksCount;

        public void AddShotMark(Cell location)
        {
            var index = GetIndex(location);
            _bits.Set(index, true);
            _marksCount++;
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
