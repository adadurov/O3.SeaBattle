using O3.SeaBattle.Logic;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public class SynchronizedGameService : IGameService
    {
        private readonly GameService _gameServiceImpl;

        public SynchronizedGameService(GameService gameServiceImpl)
        {
            _gameServiceImpl = gameServiceImpl;
        }

        public void BeginGame(IEnumerable<Ship> ships)
        {
            lock (_gameServiceImpl)
            {
                _gameServiceImpl.BeginGame(ships);
            }
        }

        public int GetMaxSize() => _gameServiceImpl.GetMaxSize();

        public GameStats GetStatistics()
        {
            lock (_gameServiceImpl)
            {
                return _gameServiceImpl.GetStatistics();
            }
        }

        public void Reset()
        {
            lock (_gameServiceImpl)
            {
                _gameServiceImpl.Reset();
            }
        }

        public void SetMatrixSize(int size)
        {
            lock (_gameServiceImpl)
            {
                _gameServiceImpl.SetMatrixSize(size);
            }
        }

        public ShotResult Shot(Cell targetCell)
        {
            lock (_gameServiceImpl)
            {
                return _gameServiceImpl.Shot(targetCell);
            }
        }
    }
}
