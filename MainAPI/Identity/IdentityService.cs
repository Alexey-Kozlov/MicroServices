using AutoMapper;
using Models;
using Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MIdentity
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IdentitySettings _identitySettings;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityService> _logger;
        public IdentityService(IHttpClientFactory clientFactory, IConfiguration config,
            IOptions<IdentitySettings> options, IMapper mapper, ILogger<IdentityService> logger) 
            : base(clientFactory, logger)
        {
            _clientFactory = clientFactory;
            _identitySettings = options.Value;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> CheckToken(string token)
        {
            return await SendAsync<bool>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Data = new IdentityModel { token = token },
                Url = _config["IDENTITY_TOKEN"]!,
                Token= token ?? ""
            }); ;
        }
        public ClaimsPrincipal? GetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenObj = tokenHandler.ReadJwtToken(token);
            if (tokenObj != null)
            {
                var model = _mapper.Map<IdentityModel>(tokenObj);
                var tempToken = CreateTempToken(model);
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ТестовыйКлюч"));
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = symmetricKey
                };
                var identity = tokenHandler.ValidateToken(tempToken, validationParameters, out var sec);
                return identity;
            }
            return null;
        }

        private string CreateTempToken(IdentityModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.GivenName, model.DisplayName)
            };
            //назначаем роли
            foreach (var role in model.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ТестовыйКлюч"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
