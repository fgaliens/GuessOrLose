using GuessOrLose.Data;
using GuessOrLose.Exceptions;
using GuessOrLose.Models;
using GuessOrLose.Services;
using Nito.AsyncEx;


namespace GuessOrLose.Game
{
    

    public class PlayersConfirmation
    {
        private readonly HashSet<Player> _players = new();

        public int ConfirmsCount => _players.Count;

        public void Confirm(Player player)
        {
            _players.Add(player);
        }
    }

    public class PlayersVoting
    {
        private readonly HashSet<Player> _allPlayers;
        private readonly Dictionary<Player, VotingStatus> _statuses = new();

        public PlayersVoting(IEnumerable<Player> players)
        {
            _allPlayers = players
                .ToHashSet();

            if (_allPlayers.Count == 0)
            {
                throw new ArgumentException("There is no players to vote", nameof(players));
            }
        }

        public bool Complete => _statuses.Count >= _allPlayers.Count;

        public double CompletionRate => (double)_statuses.Count / _allPlayers.Count;

        public VotingStatus Status => _statuses.Count(x => x.Value == VotingStatus.VotedFor) > _statuses.Count / 2 ? VotingStatus.VotedFor : VotingStatus.VotedAgainst;

        public void VoteFor(Player player, bool allowToRevote = false)
        {
            ThrowIfPlayerIsNotValid(player);
            if (!allowToRevote)
            {
                ThrowIfPlayerVotedAlready(player);
            }

            _statuses.Add(player, VotingStatus.VotedFor);
        }

        public void VoteAgainst(Player player, bool allowToRevote = false)
        {
            ThrowIfPlayerIsNotValid(player);
            if (!allowToRevote)
            {
                ThrowIfPlayerVotedAlready(player);
            }

            _statuses.Add(player, VotingStatus.VotedAgainst);
        }

        private void ThrowIfPlayerIsNotValid(Player player)
        {
            if (!_allPlayers.Contains(player))
            {
                throw IncorrectOperationException.Create("This user is not participant of this vote");
            }
        }

        private void ThrowIfPlayerVotedAlready(Player player)
        {
            if (_statuses.ContainsKey(player))
            {
                throw IncorrectOperationException.Create("Player has voted already");
            }
        }
    }

    public enum VotingStatus
    {
        VotedFor,
        VotedAgainst
    }

    public class GameSuperClass
    {
        private PlayersConfirmation _startGameConfirmation = new();
        private LoopEnumerator<Team> _teams;
        private readonly List<Word> _words = new();
        private readonly List<Player> _players = new();
        private readonly Dictionary<Player, List<Word>> _wordsToExclude = new();
        private readonly Dictionary<Player, List<Word>> _excludedWords = new();
        private readonly ITeamsBuilder _teamsBuilder;

        public GameSuperClass(
            IEnumerable<Word> words,
            ITeamsBuilder teamsBuilder)
        {
            State = GameState.Prepearing;
            _words.AddRange(words);
            _teamsBuilder = teamsBuilder;
        }

        public GameState State { get; private set; }

        public void Join(Player player)
        {
            if (State != GameState.Prepearing)
            {
                throw IncorrectOperationException.Create($"Unable to join the game in state '{State}'");
            }

            _players.Add(player);
        }

        public void ConfirmThatPlayerIsReady(Player player)
        {
            if (State != GameState.Prepearing)
            {
                throw IncorrectOperationException.Create($"Unable to do such action in state '{State}'");
            }

            if (!_players.Contains(player))
            {
                throw IncorrectOperationException.Create($"The player is not a participant of this game");
            }

            _startGameConfirmation.Confirm(player);
        }

        public void StartGame()
        {
            if (State != GameState.Prepearing)
            {
                throw IncorrectOperationException.Create($"Unable to start the game in state '{State}'");
            }

            if (_players.Count < _startGameConfirmation.ConfirmsCount)
            {
                throw IncorrectOperationException.Create($"Unable to start the game because some players is not ready");
            }

            State = GameState.InAction;
            _teams = new LoopEnumerator<Team>(_teamsBuilder.BuildTeams(_players));

            var wordsPerPlayer = _words.Count / _players.Count;
            var wordsCounter = 0;
            foreach (var player in _players)
            {
                _wordsToExclude.Add(player, new());
                for (var i = 0; i < wordsPerPlayer; i++)
                {
                    _wordsToExclude[player].Add(_words[wordsCounter++]);
                }
            }
        }

        private const int wordsToExclude = 2;

        public void ExcludeWord(Player player, Word word)
        {
            if (State != GameState.InAction)
            {
                throw IncorrectOperationException.Create($"Unable to do such action in state '{State}'");
            }

            if (_wordsToExclude.TryGetValue(player, out var words))
            {
                if (words.Contains(word))
                {
                    _excludedWords.TryAdd(player, new());
                    if (_excludedWords[player].Count < wordsToExclude)
                    {
                        _excludedWords[player].Add(word);
                        _wordsToExclude[player].Remove(word);
                    }
                }

                throw IncorrectOperationException.Create($"Unable to exclude this word");
            }

            throw IncorrectOperationException.Create($"Unable to exclude the word by this player");
        }

        public void CompleteWordsExcluding()
        {
            if (_excludedWords.Keys.Count != _players.Count)
            {
                throw IncorrectOperationException.Create($"Not all of the players excluded their words");
            }

            foreach (var item in _excludedWords)
            {
                if (item.Value.Count != wordsToExclude)
                {
                    throw IncorrectOperationException.Create($"Not all of the players excluded their words");
                }
            }

            var excludedWords = _wordsToExclude.Values.SelectMany(x => x);
            _words.Clear();
            _words.AddRange(excludedWords);
        }
    }

    public enum GameState
    {
        Prepearing,
        InAction,
        Finished
    }

    public class _Game
    {
        private const int roundsCount = 3;

        private readonly Stack<Round> rounds = new();
        private readonly IEnumerable<Player> players;
        private readonly LoopEnumerator<Team> teams;
        private readonly IEnumerable<Word> words;
        private readonly IOrderRandomizer orderRandomizer;

        public _Game(IEnumerable<Player> players, ITeamsBuilder teamsBuilder, IEnumerable<Word> words, IOrderRandomizer orderRandomizer)
        {
            this.players = players;
            teams = new LoopEnumerator<Team>(teamsBuilder.BuildTeams(players));

            this.words = words;
            this.orderRandomizer = orderRandomizer;

            //System.Reactive.NotificationKind
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
                throw new IncorrectOperationException("_Game is over. Can't start new round");
            }

            var round = new Round(this);
            rounds.Push(round);

            return round;
        }

        public Round GetCurrentRound()
        {
            if (rounds.TryPeek(out var round))
            {
                return round;
            }

            throw new IncorrectOperationException("_Game is not started");
        }
    }
}
