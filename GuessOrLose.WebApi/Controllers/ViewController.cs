using Microsoft.AspNetCore.Mvc;

namespace GuessOrLose.Controllers
{
    public class ViewController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetIndexPage()
        {
            return Ok("Index page");
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetGamePage(Guid id)
        {
            return Ok($"_Game page: '{id}'");
        }
    }
}