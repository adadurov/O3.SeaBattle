using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public class GameService : IGameService
    {
        private Game _game;
        private int _matrixSize;

        public bool IsGameStarted => _game != null;

        public bool IsGameFinished => _game != null && _game.Finished;

        public void SetMatrixSize(int size)
        {
            _matrixSize = size;
        }

        public void BeginGame(IEnumerable<Ship> ships)
        {
            _game = new Game(_matrixSize, ships, LocationValidators.Default);
        }

        public ShotResult Shot(Cell targetCell) => _game.Shot(targetCell);

        public int GetMaxSize() => GameConfig.MaxSize;

        public bool CanShoot() => _game is not null;

        public GameStats GetStatistics() => _game.GetStatistics();

        public void Reset() => _game = null;
    }
}
