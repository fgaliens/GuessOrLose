namespace GuessOrLose.WebApi.Contracts
{
    public record LogInResponse
    {
        public required string Token { get; init; }
        public required Guid Id { get; init; }
    }
}
