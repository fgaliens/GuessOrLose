using GuessOrLose.Players;

namespace GuessOrLose.WebApi.Contracts
{
    public interface IRemoteClientActions
    {
        Task Test(string param1, int param2);
        Task SendPlayer(Player player);
    }
}
