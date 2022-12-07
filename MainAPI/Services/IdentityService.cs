using MainAPI.Models;
using Microsoft.Extensions.Options;
using System;

namespace MainAPI.Services
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IdentitySettings _identitySettings;
        public IdentityService(IHttpClientFactory clientFactory,
            IOptions<IdentitySettings> options) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _identitySettings = options.Value;
        }
        public async Task<T> GetIdentity<T>(string token)
        {
            var dd = await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Data = new IdentityModel { token = token},
                Url = "https://192.168.1.10:5010/CheckToken"
            });
            return dd;
        }
    }
}
