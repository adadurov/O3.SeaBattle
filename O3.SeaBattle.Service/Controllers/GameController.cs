using Microsoft.AspNetCore.Mvc;
using O3.SeaBattle.Service.Dto;
using O3.SeaBattle.Service.Infrastructure;
using O3.SeaBattle.Service.Parsers;
using O3.SeaBattle.Service.Services;

namespace O3.SeaBattle.Service.Controllers
{
    [Route("/")]
    [ApiController]
    [ServiceFilter(typeof(ExceptionFilter))]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILocationParser _locationParser;

        public GameController(
            IGameService gameService, 
            ILocationParser locationParser)
        {
            _gameService = gameService;
            _locationParser = locationParser;
        }

        /// <summary>
        /// Fires a big bullet at the specified cell (e.g. "1A", "13Z") and returns a result
        /// </summary>
        [HttpPost("shot")]
        public ActionResult<ShotResult> Shot([FromBody] Shot newShot)
        {
            var location = _locationParser.Parse(newShot.coord);

            var result = _gameService.Shot(location);

            return Ok(new ShotResult
            {
                destroy = result.Destroyed,
                knock = result.Knocked,
                end = result.GameFinished
            });
        }

        /// <summary>
        /// Retrieves the statistics for the current game.
        /// </summary>
        [HttpGet("state")]
        public ActionResult<GameStats> GetState()
        {
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
}
