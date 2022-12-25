using System.Security.Claims;
using OrdersAPI.Services;

namespace MIdentity
{
    public interface IIdentityService : IBaseServise
    {
        Task<bool> CheckToken(string token);
        ClaimsPrincipal? GetPrincipal(string token);
    }
}
