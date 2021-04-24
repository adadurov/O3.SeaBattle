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
            throw new NotImplementedException();
            return ValidationProblem();
        }

        [HttpPost("ship")]
        public ActionResult CreateShips([FromBody] CreateShipsDto shipsInfo)
        {
            var shipSpecs = shipsInfo.Coordinates.Split(' ', System.StringSplitOptions.TrimEntries);

            var ships = shipSpecs.Select(spec => _shipFactory.Create(spec));

            //_gameService.CreateGame(ships);
            throw new NotImplementedException();

            return ValidationProblem();
        }


        [HttpPost("shot")]
        public ActionResult<ShotResult> Shot([FromBody] NewShotDto newShot)
        {
            var location = _locationParser.Parse(newShot.coord);

            var game = _gameService.GetGame();

            var result = game.Shot(location);

            if (result.Duplicate)
            {
                return ValidationProblem("You have already fired at the specified coordinates.");
            }

            throw new NotImplementedException();

            return Ok(new ShotResult
            {

            });
        }
    }
}
