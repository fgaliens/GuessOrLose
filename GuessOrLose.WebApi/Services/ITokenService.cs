namespace GuessOrLose.WebApi.Services
{
    public interface ITokenService
    {
        Task<string> GenerateForIdAsync(Guid id);
        AwaitableValueContainer<Guid> FindIdAsync(string token);
    }
}
