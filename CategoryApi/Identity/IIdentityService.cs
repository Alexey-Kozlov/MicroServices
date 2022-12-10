using System.Security.Claims;
using Services;

namespace MIdentity
{
    public interface IIdentityService : IBaseServise
    {
        Task<bool> CheckToken(string token);
        ClaimsPrincipal? GetPrincipal(string token);
    }
}
