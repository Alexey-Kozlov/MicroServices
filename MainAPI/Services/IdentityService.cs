using MainAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MainAPI.Services
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IdentitySettings _identitySettings;
        private readonly IConfiguration _config;
        public IdentityService(IHttpClientFactory clientFactory, IConfiguration config,
            IOptions<IdentitySettings> options) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _identitySettings = options.Value;
            _config = config;
        }
        public async Task<string> CheckToken(string token)
        {
            return await this.SendAsync<string>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Data = new IdentityModel { token = token},
                Url = _config["IdentitySettings:IdentityUrlCheckToken"]!
            });;
        }
        public async Task<ClaimsPrincipal?> GetPrincipal(string token)
        {
            var key = await CheckToken(token);
            if (!string.IsNullOrEmpty(key))
            {
                //токен валидный
                var tokenHandler = new JwtSecurityTokenHandler();
                var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String(key));
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = symmetricKey
                };
                return tokenHandler.ValidateToken(token!, validationParameters, out var claims);
            }
            return null;
        }
    }
}
