namespace O3.SeaBattle.Logic
{
    public struct Cell
    {
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row;

        public int Col;

        public override string ToString() => $"{Row}{(char)('A' + Col)}";
    }
}