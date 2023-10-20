namespace GuessOrLose.Players
{
    public record Player
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
