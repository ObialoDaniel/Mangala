using System.Security.Claims;
namespace Mangala.API.Extensions
{
    public static class ClaimsPrinicpalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst("sub")?.Value;
            if (!int.TryParse(value, out var id))
                throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
               return id;
        }
    }
}
