using Microsoft.AspNetCore.Authentication;

namespace GuessOrLose.WebApi.Authentication
{
    public class AuthnticationOption : AuthenticationSchemeOptions
    { }

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
