using Identity.Models;
using System.Security.Principal;

namespace Identity.Services
{
    public interface ITokenService
    {
        Task SetRefreshToken(ApplicationUser user);
        Task<string> CreateToken(ApplicationUser user);
        IPrincipal ValidateToken(string token);
    }
}
