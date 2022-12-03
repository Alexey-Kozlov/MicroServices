using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;

namespace Identity.Helpers
{
    public class UsersHelper
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersHelper(ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }
        public async Task<UserDTO> CreateUserDTO(ApplicationUser user)
        {
            await _tokenService.SetRefreshToken(user);
            return await Task.Run(() => new UserDTO
            {
                DisplayName = user.DisplayName ?? "",
                Login = user.UserName,
                Token = _tokenService.CreateToken(user).Result,
                IsAdmin = _userManager.IsInRoleAsync(user, "Admin").Result
            });
        }
    }
}
