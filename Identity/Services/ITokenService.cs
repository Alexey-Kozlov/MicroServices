using Identity.Models;

namespace Identity.Services
{
    public interface ITokenService
    {
        Task SetRefreshToken(ApplicationUser user);
        Task<string> CreateToken(ApplicationUser user);
    }
}
