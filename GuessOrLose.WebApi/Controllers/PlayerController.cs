using GuessOrLose.WebApi.Contracts;
using GuessOrLose.WebApi.Services;
using GuessOrLose.WebApi.Storages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessOrLose.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/player")]
    public class PlayerController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPlayersStorage _playersStorage;

        public PlayerController(ITokenService tokenService, IPlayersStorage playersStorage)
        {
            _tokenService = tokenService;
            _playersStorage = playersStorage;
        }

        [AllowAnonymous]
        [HttpPost("log-in")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            var id = _playersStorage.CreatePlayer(request.Name);
            try
            {
                var token = _tokenService.GenerateForId(id);

                var response = new LogInResponse
                {
                    Token = token,
                    Id = id
                };

                return Ok(response);
            }
            catch
            {
                _playersStorage.RemovePlayer(id);
                return BadRequest();
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            var id = User.Claims.First(x => x.Type == "id").Value;

            return Ok(new { Status = "OK", Code = 100, Id = id });
        }
    }
}