namespace GuessOrLose.WebApi.Models
{
    public record Token
    {
        public required string Value { get; init; }
        public required DateTime CreatedAt { get; init; }
    }
}
