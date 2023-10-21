using GuessOrLose.Models;
using GuessOrLose.Players;

namespace GuessOrLose.GameServices
{
    public interface ITeamsBuilder
    {
        IEnumerable<Team> BuildTeams(IEnumerable<Player> players);
    }
}
