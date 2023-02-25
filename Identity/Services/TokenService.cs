﻿using Identity.Controllers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor accessor, ILogger<TokenService> logger)
        {
            _config = config;
            _userManager = userManager;
            _accessor = accessor;
            _logger = logger;
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
                Expires = Double.Parse(_config["TokenExpiresHours"]) == 0 ? 
                    DateTime.UtcNow.AddMinutes(Double.Parse(_config["TokenExpiresMinutes"])) :
                    DateTime.UtcNow.AddHours(Double.Parse(_config["TokenExpiresHours"])),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            _logger.LogInformation("ValidateToken - " + token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            var validationParameters =  new TokenValidationParameters()
            {
                ValidateLifetime = true, 
                ValidateAudience = false, 
                ValidateIssuer = false,   
                IssuerSigningKey = key
            };
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            if (validatedToken != null && validatedToken.ValidTo > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }
    }
}
