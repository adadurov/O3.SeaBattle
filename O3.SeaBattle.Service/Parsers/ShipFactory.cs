using O3.SeaBattle.Logic;
using System;

namespace O3.SeaBattle.Service.Parsers
{
    public class ShipFactory
    {
        private readonly ILocationParser _locationParser;

        public ShipFactory(ILocationParser locationParser)
        {
            _locationParser = locationParser;
        }

        public Ship Create(string shipSpecification)
        {
            if (shipSpecification is null)
                throw new ArgumentNullException(nameof(shipSpecification));

            if (string.IsNullOrWhiteSpace(shipSpecification))
                throw new ArgumentException("Empty ship specification. Expected: '<location> <location>'");

            var corners = shipSpecification.Split(' ', StringSplitOptions.TrimEntries);
            if (corners.Length != 2)
            {
                throw new ArgumentException($"Bad ship specification: '{shipSpecification}'. Expected: '<location> <location>'");
            }

            return new Ship(
                _locationParser.Parse(corners[0]),
                _locationParser.Parse(corners[1])
                );
        }
    }
}
