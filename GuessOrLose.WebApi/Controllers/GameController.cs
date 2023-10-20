using GuessOrLose.Players;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessOrLose.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly Player _player;

        public GameController(Player player)
        {
            _player = player;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { Status = "OK", Player = _player });
        }
    }
}