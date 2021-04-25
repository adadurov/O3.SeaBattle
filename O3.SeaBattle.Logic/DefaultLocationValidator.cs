using System;
using System.Collections.Generic;
using System.Linq;

namespace O3.SeaBattle.Logic
{
    /// <summary>
    /// This validator checks the following ship location rules:
    /// * ships must not overlap
    /// * ships must be within the range of the specified size
    /// </summary>
    public class DefaultLocationValidator : IMatrixLocationValidator
    {
        public bool IsWithinMatrix(int size, Cell loc) =>
            loc.Row >= 0 &&
            loc.Row < size &&
            loc.Col >= 0 &&
            loc.Col < size;

        public void ValidateShipLocations(int matrixSize, IEnumerable<Ship> liveShips)
        {
            var validShips = new List<Ship>(liveShips.Count());

            foreach (var candidate in liveShips)
            {
                var isOutOfRange = !IsWithinMatrix(matrixSize, candidate.LeftTop) || !IsWithinMatrix(matrixSize, candidate.RightBottom);

                if (isOutOfRange)
                {
                    throw new ArgumentException(
                        $"At least one ship ({candidate}) is not entirely within the matrix of {matrixSize}x{matrixSize}.",
                        nameof(liveShips));
                }

                if (validShips.Any(s => s != candidate && candidate.Overlaps(s)))
                {
                    throw new ArgumentException(
                        $"At least one ship overlaps {candidate}.",
                        nameof(liveShips));
                }

                validShips.Add(candidate);
            }
        }
    }
}
