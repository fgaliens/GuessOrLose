using Microsoft.AspNetCore.Mvc;

namespace GuessOrLose.Controllers
{
    [ApiController, Route("api/game")]
    public class GameController : ControllerBase
    {      
        public Task<IActionResult> Create(Guid author)
        {

        }
    }
}