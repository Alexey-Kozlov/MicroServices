using System.Security.Claims;

namespace MainAPI.Services
{
    public interface IIdentityService : IBaseServise
    {
        Task<string> CheckToken(string token);
        Task<ClaimsPrincipal?> GetPrincipal(string token);
    }
}
