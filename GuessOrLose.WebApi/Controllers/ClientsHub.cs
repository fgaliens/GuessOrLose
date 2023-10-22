using GuessOrLose.Players;
using GuessOrLose.WebApi.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GuessOrLose.Controllers
{
    [Authorize]
    public class ClientsHub : Hub<IRemoteClientActions>
    {
        private readonly IPlayerProvider _playerProvider;

        public ClientsHub(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public override async Task OnConnectedAsync()
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();
            
            //Context.ConnectionId
            //Groups.AddToGroupAsync()

            await Clients.Caller.SendPlayer(player);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
