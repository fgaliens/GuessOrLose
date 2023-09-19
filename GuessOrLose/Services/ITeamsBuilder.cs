using GuessOrLose.Models;

namespace GuessOrLose.Services
{
    public interface ITeamsBuilder
    {
        IEnumerable<Team> BuildTeams(IEnumerable<Player> players);
    }
}
