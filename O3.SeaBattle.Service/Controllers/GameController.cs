using Microsoft.AspNetCore.Mvc;
using O3.SeaBattle.Service.Dto;
using O3.SeaBattle.Service.Parsers;
using O3.SeaBattle.Service.Services;
using System;
using System.Linq;

namespace O3.SeaBattle.Service.Controllers
{
    [Route("/")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILocationParser _locationParser;
        private readonly ShipFactory _shipFactory;

        public GameController(
            IGameService gameService, 
            ILocationParser locationParser,
            ShipFactory shipFactory)
        {
            _gameService = gameService;
            _locationParser = locationParser;
            _shipFactory = shipFactory;
        }

        /// <summary>
        /// Specifies the size of the matrix for a new game (1..26)
        /// </summary>
        [HttpPost("create-matrix")]
        public ActionResult CreateMatrix([FromBody]CreateMatrix matrix)
        {
            lock (_gameService)
            {
                var maxSize = _gameService.GetMaxSize();
                if (matrix.range < 1)
                {
                    return ValidationProblem(
                        $"The requested range ({matrix.range}) is too small. Minimum supported: 1");
                }

                if (matrix.range > maxSize)
                {
                    return ValidationProblem(
                        $"The requested range ({matrix.range} is too large. Maximum supported: {maxSize}");
                }

                _gameService.SetMatrixSize(matrix.range);
                return Ok();
            }
        }

        /// <summary>
        /// Puts all ships to the matrix and starts a new game. E.g. "1A 1B, 3C 6C" etc. You may only put ships on the matrix once.
        /// </summary>
        [HttpPost("ship")]
        public ActionResult CreateShips([FromBody] CreateShips shipsInfo)
        {
            lock (_gameService)
            {
                if (_gameService.IsGameStarted)
                {
                    return ValidationProblem(
                        $"A game is already in progress. Complete or reset the game before creating ships.");
                }

                var shipSpecs = shipsInfo.Coordinates.Split(',', StringSplitOptions.TrimEntries);

                var ships = shipSpecs.Select(spec => _shipFactory.Create(spec));

                _gameService.BeginGame(ships);

                return Ok();
            }
        }

        /// <summary>
        /// Fires a big bullet at the specified cell (e.g. "1A", "13Z") and returns a result
        /// </summary>
        [HttpPost("shot")]
        public ActionResult<ShotResult> Shot([FromBody] Shot newShot)
        {
            lock (_gameService)
            {
                if (!_gameService.IsGameStarted)
                {
                    return ValidationProblem(
                        "The game has not been started yet. Create a matrix and put ships before shooting.");
                }

                if (_gameService.IsGameFinished)
                {
                    return ValidationProblem(
                        "The game has already been finished. Starte a new game before shooting again.");
                }

                var location = _locationParser.Parse(newShot.coord);

                var result = _gameService.Shot(location);

                if (result.Duplicate)
                {
                    return ValidationProblem(
                        $"You have already fired at the specified cell ({newShot.coord}).");
                }

                if (result.InvalidLocation)
                {
                    return ValidationProblem(
                        $"You have attempted to fire out of range ({newShot.coord}).");
                }

                return Ok(new ShotResult
                {
                    destroy = result.Destroyed,
                    knock = result.Knocked,
                    end = result.GameFinished
                });
            }
        }

        /// <summary>
        /// Retrieves the statistics for the current game.
        /// </summary>
        [HttpGet("state")]
        public ActionResult<GameStats> GetState()
        {
            lock (_gameService)
            {
                if (!_gameService.IsGameStarted)
                {
                    return ValidationProblem(
                        "The game has not been started yet. Create a matrix and put ships before requesting game statistics.");
                }

                var stats = _gameService.GetStatistics();

                return Ok(new GameStats
                {
                    ship_count = stats.ShipCount,
                    knocked = stats.Knocked,
                    destroyed = stats.Destroyed,
                    shot_count = stats.ShotCount
                });
            }
        }

        /// <summary>
        /// Resets the current game and allows to start a new one before destroying all ships.
        /// </summary>
        [HttpPost("clear")]
        public ActionResult RestartGame()
        {
            lock (_gameService)
            {
                _gameService.Reset();
                return Ok();
            }
        }
    }
}
