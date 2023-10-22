using System.Security.Claims;

namespace GuessOrLose.WebApi
{
    public static class ClaimsPrincipalExtensions 
    {
        public static Guid GetPlayerId(this ClaimsPrincipal claimsPrincipal)
        {
            var idValue = claimsPrincipal.FindFirstValue(ClaimTypes.Id);
            if (Guid.TryParse(idValue, out var id))
            {
                return id;
            }

            throw new InvalidOperationException($"Can`t get claim of type '{ClaimTypes.Id}'");
        }
    }
}
