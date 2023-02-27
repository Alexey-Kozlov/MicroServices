﻿using AutoMapper;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MIdentity
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        public IdentityService(IHttpClientFactory clientFactory, IMapper mapper) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
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
                var claimIdentity = new ClaimsIdentity(claims,"SomeCustomKey");
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                return claimsPrincipal;
            }
            return null;
        }
    }
}
