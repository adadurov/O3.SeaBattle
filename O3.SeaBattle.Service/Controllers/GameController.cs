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

        [HttpPost("create-matrix")]
        public ActionResult CreateMatrix([FromBody]CreateMatrixDto matrix)
        {
            var maxSize = _gameService.GetMaxSize();
            if (matrix.range < 1)
            {
                return ValidationProblem(
                    $"The requested range ({matrix.range} is too small. Minimum supported: 1");
            }

            if (matrix.range > maxSize)
            {
                return ValidationProblem(
                    $"The requested range ({matrix.range} is too large. Maximum supported: {maxSize}");
            }

            _gameService.SetMatrixSize(matrix.range);
            return Ok();
        }

        [HttpPost("ship")]
        public ActionResult CreateShips([FromBody] CreateShipsDto shipsInfo)
        {
            if (_gameService.IsGameInProgress)
            {
                return ValidationProblem(
                    $"A game is already in progress. Complete or reset the game before creating ships.");
            }

            var shipSpecs = shipsInfo.Coordinates.Split(',', StringSplitOptions.TrimEntries);

            var ships = shipSpecs.Select(spec => _shipFactory.Create(spec));

            _gameService.BeginGame(ships);

            return Ok();
        }


        [HttpPost("shot")]
        public ActionResult<ShotResultDto> Shot([FromBody] NewShotDto newShot)
        {
            var location = _locationParser.Parse(newShot.coord);

            var game = _gameService.GetGame();

            var result = game.Shot(location);

            if (result.Duplicate)
            {
                return ValidationProblem(
                    $"You have already fired at the specified coordinates ({newShot.coord}).");
            }

            return Ok(new ShotResultDto {
                destroy = result.Destroyed,
                knock = result.Knocked,
                end = result.GameFinished
            });
        }
    }
}
