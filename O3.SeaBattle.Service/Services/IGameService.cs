using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public interface IGameService
    {
        int GetMaxSize();

        void SetMatrixSize(int size);

        void BeginGame(IEnumerable<Ship> ships);

        ShotResult Shot(Cell targetCell);

        GameStats GetStatistics();

        void Reset();
    }
}
