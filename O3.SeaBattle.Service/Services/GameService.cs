using O3.SeaBattle.Logic;
using System;
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
            const int minSize = 1;
            var maxSize = GetMaxSize();
            if (size < minSize)
            {
                throw new ArgumentException(
                    $"The requested range ({size}) is too small. Minimum supported: {minSize}");
            }

            if (size > maxSize)
            {
                throw new ArgumentException(
                    $"The requested range ({size} is too large. Maximum supported: {maxSize}");
            }

            _matrixSize = size;
        }

        public void BeginGame(IEnumerable<Ship> ships)
        {
            if (IsGameStarted)
            {
                throw new InvalidOperationException(
                    $"A game is already in progress. Complete or reset the game before creating ships.");
            }

            _game = new Game(_matrixSize, ships, LocationValidators.Default);
        }

        public ShotResult Shot(Cell targetCell)
        {
            if (!IsGameStarted)
            {
                throw new InvalidOperationException(
                    "The game has not been started yet. Create a matrix and put ships before shooting.");
            }

            if (IsGameFinished)
            {
                throw new InvalidOperationException(
                    "The game has already been finished. Start a new game before shooting again.");
            }

            var result = _game.Shot(targetCell);

            if (result.Duplicate)
            {
                throw new InvalidOperationException(
                    $"You have already fired at the specified cell ({targetCell}).");
            }

            if (result.InvalidLocation)
            {
                throw new ArgumentOutOfRangeException(
                    $"You have attempted to fire out of range ({targetCell}).");
            }

            return result;
        }

        public int GetMaxSize() => GameConfig.MaxSize;

        public GameStats GetStatistics()
        {
            if (!IsGameStarted)
            {
                throw new InvalidOperationException(
                    "The game has not been started yet. Create a matrix and put ships before requesting game statistics.");
            }
            return _game.GetStatistics();
        }

        public void Reset() => _game = null;
    }
}
