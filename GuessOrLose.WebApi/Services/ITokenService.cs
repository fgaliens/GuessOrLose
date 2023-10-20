namespace GuessOrLose.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateForId(Guid id);
        bool TryFindId(string token, out Guid id);
    }
}
