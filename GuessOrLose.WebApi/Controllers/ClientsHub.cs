using GuessOrLose.WebApi.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace GuessOrLose.Controllers
{
    public class ClientsHub : Hub<IRemoteClientActions>
    {

    }
}
