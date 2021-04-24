using System;

namespace O3.SeaBattle.Logic
{
    public class Ship
    {
        internal Ship(Location leftTop, Location rightBottom)
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
        public bool SpansOver(Location cell)
            => cell.Row >= LeftTop.Row &&
               cell.Row <= RightBottom.Row &&
               cell.Col >= LeftTop.Col &&
               cell.Col <= RightBottom.Col;

        public int AddShot() => _shotCount++;

        public bool IntersectsWith(Ship that)
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

        public Location LeftTop { get; internal set; }

        public Location RightBottom { get; internal set; }

        public Location LeftBottom => new () { Col = LeftTop.Col, Row = RightBottom.Row };

        public Location RightTop => new () { Col = RightBottom.Col, Row = LeftTop.Row };

        public bool IsAlive => _size > _shotCount;

        public override int GetHashCode() => HashCode.Combine(LeftTop.Row, LeftTop.Col);
    }
}
