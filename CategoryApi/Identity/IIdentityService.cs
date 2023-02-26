using System.Security.Claims;
using Services;

namespace MIdentity
{
    public interface IIdentityService 
    {
        ClaimsPrincipal? GetPrincipal(string token);
    }
}
