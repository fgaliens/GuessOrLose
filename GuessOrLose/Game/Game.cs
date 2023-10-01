using GuessOrLose.Data;
using GuessOrLose.Exceptions;
using GuessOrLose.Models;
using GuessOrLose.Services;
using Nito.AsyncEx;

namespace GuessOrLose.Game
{
    public class Game
    {
        private const int roundsCount = 3;

        private readonly Stack<Round> rounds = new();
        private readonly IEnumerable<Player> players;
        private readonly LoopEnumerator<Team> teams;
        private readonly IEnumerable<Word> words;
        private readonly IOrderRandomizer orderRandomizer;

        public Game(IEnumerable<Player> players, ITeamsBuilder teamsBuilder, IEnumerable<Word> words, IOrderRandomizer orderRandomizer)
        {
            this.players = players;
            teams = new LoopEnumerator<Team>(teamsBuilder.BuildTeams(players));

            this.words = words;
            this.orderRandomizer = orderRandomizer;
        }

        public LoopEnumerator<Team> Teams => teams;

        public IEnumerable<Word> Words => words;

        public IOrderRandomizer OrderRandomizer => orderRandomizer;

        public int CurrentRound => rounds.Count;

        public int TotalRoundsCount => roundsCount;

        public Round StartRound()
        {
            if (rounds.TryPeek(out var lastRound))
            {
                if (!lastRound.Finished)
                {
                    throw new IncorrectOperationException("There is an active round. Can't start new one");
                }
            }

            if (rounds.Count >= roundsCount)
            {
                throw new IncorrectOperationException("Game is over. Can't start new round");
            }

            return new(this);
        }

        public Round GetCurrentRound()
        {
            if (rounds.TryPeek(out var round))
            {
                return round;
            }

            throw new IncorrectOperationException("Game is not started");
        }
    }
}
