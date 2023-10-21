using GuessOrLose.GameServices.Stages;
using Microsoft.Extensions.DependencyInjection;

namespace GuessOrLose.GameServices
{
    public class Game : IGame
    {
        public int activeStageIndex = 0;
        public readonly IGameStage[] stages;

        public Game(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid();
            stages = new[]
            {
                serviceProvider.GetRequiredService<JoinPlayersStage>()
            };
        }

        public Guid Id { get; }

        public IGameStage ActiveStage => stages[activeStageIndex];

        public Task NotifyStageCompleteAsync(IGameStage stage)
        {
            activeStageIndex++;
            return Task.CompletedTask;
        }
    }
}
