using System.Security.Claims;

namespace MIdentity
{
    public interface IIdentityService
    {
        ClaimsPrincipal? GetPrincipal(string token);
    }
}
