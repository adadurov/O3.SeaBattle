using System;

namespace O3.SeaBattle.Logic
{
    public static class ShipFactory
    {
        public static Ship Create(string shipSpecification)
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
                LocationFactory.Create(corners[0]), 
                LocationFactory.Create(corners[1])
                );
        }
    }
}
