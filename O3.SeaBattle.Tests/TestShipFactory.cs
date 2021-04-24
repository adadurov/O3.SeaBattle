using O3.SeaBattle.Logic;
using O3.SeaBattle.Service.Parsers;

namespace O3.SeaBattle.Tests
{
    public static class TestShipFactory
    {
        public static Ship A1 { get; } = new Ship(new Cell(0, 0), new Cell(0, 0));
        
        public static Ship B2 { get; } = new Ship(new Cell(1, 1), new Cell(1, 1));

        public static Ship[] DummyShipList { get; } = new[] { new Ship(new Cell(0, 0), new Cell(0, 1)) };


        public static Ship Create(string spec)
        {
            var locParser = new LocationParser(1, 'A', Game.MaxSize);
            return new ShipFactory(locParser).Create(spec);
        }
    }
}
