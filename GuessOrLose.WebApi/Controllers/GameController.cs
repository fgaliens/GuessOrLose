using GuessOrLose.Exceptions;
using GuessOrLose.GameServices;
using GuessOrLose.GameServices.Stages;
using GuessOrLose.Players;
using GuessOrLose.WebApi.Contracts;
using GuessOrLose.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessOrLose.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;

        public GameController(IPlayerProvider playerProvider, IGameService gameService, IPlayerService playerService)
        {
            _playerProvider = playerProvider;
            _gameService = gameService;
            _playerService = playerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGame()
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();

            var isPlayerInOtherGame = await _playerService.IsPlayerInGameAsync(player);
            if (isPlayerInOtherGame)
            {
                throw new ActionForbiddenException(ExceptionCode.PlayerIsInOtherGame, "Player is in other game");
            }

            var game = await _gameService.CreateGameAsync();
            if (game.ActiveStage is JoinPlayersStage joinStage)
            {
                await joinStage.JoinAsync(player);

                var response = new CreateGameResponse
                {
                    Id = game.Id
                };

                return Ok(response);
            }
            else
            {
                throw new IncorrectOperationException(ExceptionCode.UnexpectedGameState, $"Unexpected stage of the game");
            }
            
        }
    }
}