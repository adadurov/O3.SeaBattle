using O3.SeaBattle.Logic;

namespace O3.SeaBattle.Service.Parsers
{
    public interface ILocationParser
    {
        int RowOrigin { get; }

        char ColumnOrigin { get; }

        Cell Parse(string location);
    }
}