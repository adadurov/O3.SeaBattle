using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public interface IGameService
    {
        bool IsGameInProgress { get; }

        Game GetGame();

        void SetMatrixSize(int size);

        void BeginGame(IEnumerable<Ship> ships);
        int GetMaxSize();
    }
}