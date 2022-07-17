using System.Security.Claims;

namespace API.Extensions
{
    public static class UserClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal claims)
        {
            return  claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}