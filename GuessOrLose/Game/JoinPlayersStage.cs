using GuessOrLose.Data;
using GuessOrLose.Exceptions;
using GuessOrLose.Messages;
using GuessOrLose.Models;
using GuessOrLose.Models.Messages;
using Nito.AsyncEx;

namespace GuessOrLose.Game
{
    public class JoinPlayersStage : IGameStage
    {
        private readonly AsyncLock _lock = new();
        private readonly ISingleValueStorage<Player, IGamePipeline> _playersStorage;
        private readonly IMessageWriter<JoinPlayersStageMessages.StateChanged> _stateChangedMessageWriter;
        private readonly IMessageWriter<JoinPlayersStageMessages.PlayerJoined> _playerJoinedMessageWriter;
        private readonly IMessageWriter<JoinPlayersStageMessages.PlayerReadyToStart> _playerReadyToStartMessageWriter;
        private readonly HashSet<Player> _joinedUsers;
        private readonly HashSet<Player> _readyUsers;

        public JoinPlayersStage(
            ISingleValueStorage<Player, IGamePipeline> playersStorage,
            IEqualityComparer<Player> playersEqualityComparer,
            IMessageWriter<JoinPlayersStageMessages.StateChanged> stateChangedMessageWriter,
            IMessageWriter<JoinPlayersStageMessages.PlayerJoined> playerJoinedMessageWriter,
            IMessageWriter<JoinPlayersStageMessages.PlayerReadyToStart> playerReadyToStartMessageWriter)
        {
            State = StageState.Ready;

            _playersStorage = playersStorage;
            _stateChangedMessageWriter = stateChangedMessageWriter;
            _playerJoinedMessageWriter = playerJoinedMessageWriter;
            _playerReadyToStartMessageWriter = playerReadyToStartMessageWriter;

            _joinedUsers = new HashSet<Player>(playersEqualityComparer);
            _readyUsers = new HashSet<Player>(playersEqualityComparer);
        }

        public IGamePipeline? Pipeline { get; private set; }

        public StageState State { get; private set; }

        public IEnumerable<IGameStage> Substages => Enumerable.Empty<IGameStage>();

        public async Task StartAsync(IGamePipeline gamePipeline)
        {
            using var lockScope = await _lock.LockAsync();

            ThrowIfStateIsNot(StageState.Ready, nameof(StartAsync));

            Pipeline = gamePipeline;
            State = StageState.InAction;

            await ChangeStateAsync(StageState.InAction);
        }

        public async Task JoinAsync(Player player)
        {
            using var lockScope = await _lock.LockAsync();

            ThrowIfStateIsNot(StageState.InAction, nameof(JoinAsync));

            if (await _playersStorage.ContainsKeyAsync(player))
            {
                throw new ActionForbiddenException(ExceptionCode.PlayerNotInThisGame,
                    $"Player is participant of the other game");
            }

            await _playersStorage.AddAsync(player, Pipeline!);
            _joinedUsers.Add(player);
        }

        public async Task PlayerIsReadyToStartAsync(Player player)
        {
            using var lockScope = await _lock.LockAsync();

            ThrowIfStateIsNot(StageState.InAction, nameof(PlayerIsReadyToStartAsync));

            var game = await _playersStorage.GetValueAsync(player);
            if (game.TryGetValue(out var gamePipeline))
            {
                _readyUsers.Add(player);
                if (_joinedUsers.Count == _readyUsers.Count)
                {
                    await ChangeStateAsync(StageState.Finished);
                    return;
                }
            }
            else
            {
                throw new ActionForbiddenException(ExceptionCode.PlayerNotInThisGame,
                    $"Player is not participant of this game");
            }
        }

        private async Task ChangeStateAsync(StageState newState)
        {
            var previousState = State;
            State = newState;

            if (newState == StageState.Finished)
            {
                await Pipeline!.NotifyStageCompleteAsync(this);
            }

            await _stateChangedMessageWriter.WriteAsync(message =>
            {
                message.Source = this;
                message.PreviousState = previousState;
                message.CurrentState = newState;
            });
        }

        private void ThrowIfStateIsNot(StageState expectedState, string caller)
        {
            if (State != expectedState)
            {
                throw new ActionForbiddenException(ExceptionCode.ActionForbiddenDueToState,
                    $"Object has an unexpected newState ('{State}' instead of '{expectedState}' in {caller})");
            }
        }
    }
}
