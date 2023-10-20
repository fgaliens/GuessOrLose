using GuessOrLose.Models;
using GuessOrLose.Players;

namespace GuessOrLose.Game
{
    public interface ITeamsBuilder
    {
        IEnumerable<Team> BuildTeams(IEnumerable<Player> players);
    }
}
