using Microsoft.AspNetCore.Mvc;
using O3.SeaBattle.Service.Dto;
using O3.SeaBattle.Service.Infrastructure;
using O3.SeaBattle.Service.Parsers;
using O3.SeaBattle.Service.Services;
using System;
using System.Linq;

namespace O3.SeaBattle.Service.Controllers
{
    [Route("/")]
    [ApiController]
    [ServiceFilter(typeof(ExceptionFilter))]
    public class GameStateController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ShipFactory _shipFactory;

        public GameStateController(
            IGameService gameService, 
            ShipFactory shipFactory)
        {
            _gameService = gameService;
            _shipFactory = shipFactory;
        }

        /// <summary>
        /// Specifies the size of the matrix for a new game (1..26)
        /// </summary>
        [HttpPost("create-matrix")]
        public ActionResult CreateMatrix([FromBody]CreateMatrix matrix)
        {
            _gameService.SetMatrixSize(matrix.range);
            return Ok();
        }

        /// <summary>
        /// Puts all ships to the matrix and starts a new game. E.g. "1A 1B, 3C 6C" etc. You may only put ships on the matrix once.
        /// </summary>
        [HttpPost("ship")]
        public ActionResult CreateShips([FromBody] CreateShips shipsInfo)
        {
            var shipSpecs = shipsInfo.Coordinates.Split(',', StringSplitOptions.TrimEntries);

            var ships = shipSpecs.Select(spec => _shipFactory.Create(spec));

            _gameService.BeginGame(ships);

            return Ok();
        }

        /// <summary>
        /// Resets the current game and allows to start a new one before destroying all ships.
        /// </summary>
        [HttpPost("clear")]
        public ActionResult RestartGame()
        {
            _gameService.Reset();
            return Ok();
        }
    }
}
