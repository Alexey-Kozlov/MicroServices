using AutoMapper;
using Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OrdersAPI.Services;

namespace MIdentity
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IdentitySettings _identitySettings;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public IdentityService(IHttpClientFactory clientFactory, IConfiguration config,
            IOptions<IdentitySettings> options, IMapper mapper) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _identitySettings = options.Value;
            _config = config;
            _mapper = mapper;
        }
        public async Task<bool> CheckToken(string token)
        {
            return await SendAsync<bool>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Data = new IdentityModel { token = token },
                Url = _config["IdentitySettings:IdentityUrlCheckToken"]!
            }); ;
        }
        public ClaimsPrincipal? GetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenObj = tokenHandler.ReadJwtToken(token);
            if (tokenObj != null)
            {
                var model = _mapper.Map<IdentityModel>(tokenObj);
                //эдесь получаем аутентифицированного пользователя упрощенно, из списка клаймсов
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
                var claimIdentity = new ClaimsIdentity(claims, "SomeCustomKey");
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                return claimsPrincipal;
            }
            return null;
        }
    }
}
