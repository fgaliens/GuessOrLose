using GuessOrLose.Exceptions;
using GuessOrLose.Players;
using Nito.AsyncEx;

namespace GuessOrLose.GameServices.Stages
{
    public class JoinPlayersStage : IGameStage
    {
        private readonly AsyncLock _sync = new();
        private readonly IPlayerService _playersService;
        private readonly HashSet<Player> _joinedUsers;
        private readonly HashSet<Player> _readyUsers;

        public JoinPlayersStage(
            IPlayerService playersService,
            IEqualityComparer<Player> playersEqualityComparer)
        {
            State = StageState.Ready;

            _playersService = playersService;
            _joinedUsers = new HashSet<Player>(playersEqualityComparer);
            _readyUsers = new HashSet<Player>(playersEqualityComparer);
        }

        public IGame? Game { get; private set; }

        public StageState State { get; private set; }

        public async Task StartAsync(IGame gamePipeline)
        {
            using var lockScope = await _sync.LockAsync();

            ThrowIfStateIsNot(StageState.Ready, nameof(StartAsync));

            Game = gamePipeline;
            State = StageState.InAction;

            await ChangeStateAsync(StageState.InAction);
        }

        public async Task JoinAsync(Player player)
        {
            using var lockScope = await _sync.LockAsync();

            ThrowIfStateIsNot(StageState.InAction, nameof(JoinAsync));

            await _playersService.AddToGameAsync(player, Game!);
            _joinedUsers.Add(player);
        }

        public async Task PlayerIsReadyToStartAsync(Player player)
        {
            using var lockScope = await _sync.LockAsync();

            ThrowIfStateIsNot(StageState.InAction, nameof(PlayerIsReadyToStartAsync));

            if (await _playersService.IsPlayerInGameAsync(player, Game!))
            {
                _readyUsers.Add(player);
                var count = await _playersService.CountPlayersInGameAsync(Game!);
                if (count == _joinedUsers.Count)
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
                await Game!.NotifyStageCompleteAsync(this);
            }

            //await _stateChangedMessageWriter.WriteAsync(message =>
            //{
            //    message.Source = this;
            //    message.PreviousState = previousState;
            //    message.CurrentState = newState;
            //});
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
