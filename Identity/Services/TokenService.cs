using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _accessor;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager, IHttpContextAccessor accessor)
        {
            _config = config;
            _userManager = userManager;
            _accessor = accessor;
        }

        public async Task<string> CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };
            //назначаем роли
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Double.Parse(_config["TokenExpiresMinutes"])),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber)
            };
        }

        public async Task SetRefreshToken(ApplicationUser user)
        {
            var _user = await _userManager.Users
                .Include(p => p.RefreshTokens)
                .FirstOrDefaultAsync(p => p.UserName == user.UserName);
            var refreshToken = GenerateRefreshToken();
            if (_user!.RefreshTokens.Any())
            {
                var oldToken = _user.RefreshTokens.FirstOrDefault();
                oldToken!.Token = refreshToken.Token;
                oldToken.Expires = refreshToken.Expires;
            }
            else
            {
                user.RefreshTokens.Add(refreshToken);
            }
            await _userManager.UpdateAsync(user);
            var coockieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7)
            };
            _accessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken.Token, coockieOptions);
        }
    }
}
