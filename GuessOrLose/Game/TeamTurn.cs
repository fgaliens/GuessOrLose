using GuessOrLose.Models;

namespace GuessOrLose.Game
{
    public class TeamTurn
    {
        public TeamTurn(Team team)
        {
            Team = team;
        }

        public Team Team { get; }
    }
}
