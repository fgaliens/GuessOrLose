using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace GuessOrLose.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly ConcurrentDictionary<string, Guid> _tokenStore = new();
        private readonly Regex _tokenFormatter = new(@"[^\w\d]");

        public string GenerateForId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id can not be empty", nameof(id));
            }

            var buffer = new byte[32];
            Random.Shared.NextBytes(buffer);
            var token = Convert.ToBase64String(buffer);

            token = _tokenFormatter.Replace(token, string.Empty);

            if (!_tokenStore.TryAdd(token, id))
            {
                throw new InvalidOperationException("Token can not be generated. Try again");
            }

            return token;
        }

        public bool TryFindId(string token, out Guid id)
        {
            if (_tokenStore.TryGetValue(token, out id))
            {
                return true;
            }

            id = Guid.Empty;
            return false;
        }
    }
}
