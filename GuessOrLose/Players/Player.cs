using GuessOrLose.Helpers.EqualityComparers;

namespace GuessOrLose.Players
{
    public record Player : IIdentifiable
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
