using O3.SeaBattle.Logic;
using System;
using System.Collections.Generic;

namespace O3.SeaBattle.Service.Services
{
    public class GameService : IGameService
    {
        private readonly Game _game;

        private int _matrixSize;

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
            throw new NotImplementedException();
        }
    }
}