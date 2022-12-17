using Identity.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace Identity.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
        bool ValidateToken(string token);
    }
}
