using GuessOrLose.Exceptions;
using GuessOrLose.GameServices.Stages;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;

namespace GuessOrLose.GameServices
{
    public class Game : IGame
    {
        private readonly AsyncLock _sync = new();
        public int activeStageIndex = 0;
        public readonly List<IGameStage> stages;

        public Game(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid();
            stages = new()
            {
                serviceProvider.GetRequiredService<JoinPlayersStage>()
            };
        }

        public Guid Id { get; }

        public IGameStage ActiveStage => stages[activeStageIndex];

        public async Task InsertStageAsync(IGameStage stage)
        {
            using var scope = await _sync.LockAsync();
            
            if (stage.State != StageState.Ready)
            {
                throw new IncorrectOperationException(ExceptionCode.ActionForbiddenDueToState,
                    $"Can`t insert a stage with state {stage.State}");
            }

            stages.Insert(activeStageIndex, stage);
        }

        public async Task NotifyStageCompleteAsync(IGameStage stage)
        {
            using var scope = await _sync.LockAsync();

            if (stage.State != StageState.Finished)
            {
                throw new IncorrectOperationException(ExceptionCode.ActionForbiddenDueToState, 
                    $"State can not be marked as finished because it`s state is {stage.State}");
            }

            activeStageIndex++;
        }
    }
}
