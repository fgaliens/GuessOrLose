using GuessOrLose.Models;

namespace GuessOrLose.Game
{
    public interface ITeamsBuilder
    {
        IEnumerable<Team> BuildTeams(IEnumerable<Player> players);
    }
}
