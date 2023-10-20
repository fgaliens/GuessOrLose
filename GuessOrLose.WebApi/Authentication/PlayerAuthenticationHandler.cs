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

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetToken(out var token))
            {
                return (AuthenticateAsyncResult)AuthenticateResult.NoResult();
            }

            if (!_tokenService.TryFindId(token, out var id))
            {
                return (AuthenticateAsyncResult)AuthenticateResult.Fail("Invalid token");
            }

            try
            {
                var player = _playersStorage.GetPlayer(id);
                var claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaims(new[]
                {
                    new Claim(ClaimTypes.Id, id.ToString()),
                    new Claim(ClaimTypes.Name, player.Name)
                });

                var principal = new ClaimsPrincipal(claimsIdentity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return (AuthenticateAsyncResult)AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return (AuthenticateAsyncResult)AuthenticateResult.Fail(ex);
            }
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

        private readonly struct AuthenticateAsyncResult
        {
            public AuthenticateAsyncResult(AuthenticateResult authenticateResult)
            {
                AuthenticateResult = authenticateResult;
            }

            public AuthenticateResult AuthenticateResult { get; }

            public static implicit operator AuthenticateAsyncResult(AuthenticateResult authenticateResult) =>
                new(authenticateResult);

            public static implicit operator Task<AuthenticateResult>(AuthenticateAsyncResult authenticateAsyncResult) =>
                Task.FromResult(authenticateAsyncResult.AuthenticateResult);
        }
    }

    //public class PlayerAuthenticationHandler : IAuthenticationHandler
    //{
    //    private readonly ITokenService _tokenService;
    //    private readonly IPlayersStorage _playersStorage;
    //    private AuthenticationScheme? _scheme;
    //    private string? _tokenValue;

    //    public PlayerAuthenticationHandler(ITokenService tokenService, IPlayersStorage playersStorage)
    //    {
    //        _tokenService = tokenService;
    //        _playersStorage = playersStorage;
    //    }

    //    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    //    {
    //        _scheme = scheme;

    //        var tokenValues = new StringValues();
    //        if (context.Request.Headers.TryGetValue("Token", out var values))
    //        {
    //            tokenValues = values;
    //        }
    //        else if (context.Request.Query.TryGetValue("token", out values))
    //        {
    //            tokenValues = values;
    //        }

    //        _tokenValue = tokenValues.FirstOrDefault();

    //        return Task.CompletedTask;
    //    }

    //    public Task<AuthenticateResult> AuthenticateAsync()
    //    {
    //        if (_tokenValue is null)
    //        {
    //            return Task.FromResult(AuthenticateResult.NoResult());
    //        }

    //        if (_scheme is null)
    //        {
    //            return Task.FromResult(AuthenticateResult.Fail("Scheme is not defined"));
    //        }

    //        try
    //        {
    //            if (!_tokenService.TryFindId(_tokenValue, out var id))
    //            {
    //                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
    //            }

    //            var player = _playersStorage.GetPlayer(id);
    //            var claimsIdentity = new ClaimsIdentity();
    //            claimsIdentity.AddClaims(new[]
    //            {
    //                new Claim(ClaimNames.Id, id.ToString()),
    //                new Claim(ClaimNames.Name, player.Name)
    //            });
    //            var principal = new ClaimsPrincipal(claimsIdentity);

    //            //var authProperties = new AuthenticationProperties();

    //            var ticket = new AuthenticationTicket(principal, _scheme.Name);

    //            return Task.FromResult(AuthenticateResult.Success(ticket));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Task.FromResult(AuthenticateResult.Fail(ex));
    //        }
    //    }

    //    public Task ChallengeAsync(AuthenticationProperties? properties)
    //    {
    //        //throw new NotImplementedException();
    //        return Task.CompletedTask;
    //    }

    //    public Task ForbidAsync(AuthenticationProperties? properties)
    //    {
    //        //throw new NotImplementedException();
    //        return Task.CompletedTask;
    //    }
    //}
}
