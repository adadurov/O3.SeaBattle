using System.Collections.Generic;

namespace O3.SeaBattle.Logic
{
    public interface IMatrixLocationValidator
    {
        bool IsWithinMatrix(int size, Cell loc);

        void ValidateShipLocations(int matrixSize, IEnumerable<Ship> liveShips);
    }
}
