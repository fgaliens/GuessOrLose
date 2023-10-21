using GuessOrLose.WebApi.Services;
using GuessOrLose.WebApi.Storages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GuessOrLose.WebApi.Authentication
{
    public class PlayerAuthenticationHandler : AuthenticationHandler<AuthnticationOption>
    {
        private readonly ITokenService _tokenService;
        private readonly IPlayersStorage _playersStorage;

        public PlayerAuthenticationHandler(
            ITokenService tokenService,
            IPlayersStorage playersStorage,
            IOptionsMonitor<AuthnticationOption> optionsMonitor,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock)
            : base(optionsMonitor, loggerFactory, urlEncoder, systemClock)
        {
            _tokenService = tokenService;
            _playersStorage = playersStorage;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetToken(out var token))
            {
                return AuthenticateResult.NoResult();
            }

            var id = await _tokenService.FindIdAsync(token);
            if (!id.HasValue)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var player = await _playersStorage.GetPlayerAsync(id);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(new[]
            {
                new Claim(ClaimTypes.Id, id.ToString()),
                new Claim(ClaimTypes.Name, player.Name)
            });

            var principal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);

        }

        private bool TryGetToken(out string token)
        {
            var tokenValues = new StringValues();
            if (Request.Headers.TryGetValue("Token", out var values))
            {
                tokenValues = values;
            }
            else if (Request.Query.TryGetValue("token", out values))
            {
                tokenValues = values;
            }

            var tokenValue = tokenValues.FirstOrDefault();
            if (tokenValue is not null)
            {
                token = tokenValue;
                return true;
            }

            token = string.Empty;
            return false;
        }
    }
}
