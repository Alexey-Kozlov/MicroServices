using System.Security.Claims;
using Services;

namespace MIdentity
{
    public interface IIdentityService : IBaseServise
    {
        ClaimsPrincipal? GetPrincipal(string token);
    }
}
