using Identity.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace Identity.Services
{
    public interface ITokenService
    {
        Task SetRefreshToken(ApplicationUser user);
        Task<string> CreateToken(ApplicationUser user);
        bool ValidateToken(string token);
    }
}
