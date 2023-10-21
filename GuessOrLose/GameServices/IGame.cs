using GuessOrLose.Helpers.EqualityComparers;

namespace GuessOrLose.GameServices
{
    public interface IGame : IIdentifiable
    {
        IGameStage ActiveStage { get; }
        Task NotifyStageCompleteAsync(IGameStage stage);
    }
}
