using System;

namespace O3.SeaBattle.Logic
{
    public class Ship
    {
        public Ship(Cell leftTop, Cell rightBottom)
        {
            if (leftTop.Row > rightBottom.Row)
            {
                throw new ArgumentException($"{nameof(leftTop)} must not be located lower than {nameof(rightBottom)}");
            }

            if (leftTop.Col > rightBottom.Col)
            {
                throw new ArgumentException($"{nameof(leftTop)} must not be located to the right of {nameof(rightBottom)}");
            }

            LeftTop = leftTop;
            RightBottom = rightBottom;

            _size = (RightBottom.Col - LeftTop.Col + 1) * (RightBottom.Row - LeftTop.Row + 1);
        }

        /// <summary>
        /// Tests whether the ship spans over the specified cell
        /// </summary>
        public bool SpansOver(Cell cell)
            => cell.Row >= LeftTop.Row &&
               cell.Row <= RightBottom.Row &&
               cell.Col >= LeftTop.Col &&
               cell.Col <= RightBottom.Col;

        public int AddShot() 
            => _shotCount++;

        public bool Overlaps(Ship that)
        {
            static bool ContainsAnyCorner(Ship a, Ship b)
                => a.SpansOver(b.LeftTop) ||
                   a.SpansOver(b.LeftBottom) ||
                   a.SpansOver(b.RightBottom) ||
                   a.SpansOver(b.RightTop);

            return ContainsAnyCorner(this, that) || ContainsAnyCorner(that, this);
        }

        private readonly int _size = 0;

        private int _shotCount = 0;

        public Cell LeftTop { get; init; }

        public Cell RightBottom { get; init; }

        public Cell LeftBottom => new () { Col = LeftTop.Col, Row = RightBottom.Row };

        public Cell RightTop => new () { Col = RightBottom.Col, Row = LeftTop.Row };

        public bool IsAlive => _size > _shotCount;

        public override string ToString() => $"[{LeftTop} {RightBottom}]";
    }
}
