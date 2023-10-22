namespace GuessOrLose.Words
{
    public record Word
    {
        public required string Value { get; init; }
        public required string Meaning { get; init; }
    }
}
