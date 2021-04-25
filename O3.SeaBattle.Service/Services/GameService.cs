using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public class GameService : IGameService
    {
        private Game _game;
        private int _matrixSize;

        public bool IsGameInProgress => _game != null && !_game.Finished;

        public Game GetGame()
        {
            return _game;
        }

        public void SetMatrixSize(int size)
        {
            _matrixSize = size;
        }

        public void BeginGame(IEnumerable<Ship> ships)
        {
            _game = new Game(_matrixSize, ships);
        }

        public int GetMaxSize() => GameConfig.MaxSize;
    }
}
