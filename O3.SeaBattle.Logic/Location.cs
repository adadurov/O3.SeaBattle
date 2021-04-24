namespace O3.SeaBattle.Logic
{
    /// <summary>
    /// Specifies a location on the seabattle matrix
    /// </summary>
    public struct Location
    {
        internal Location(byte row, char col)
        {
            Col = col;
            Row = row;
        }

        public char Col;

        public byte Row;

        public static Location Origin { get; } = new Location(1, 'A');
    }
}