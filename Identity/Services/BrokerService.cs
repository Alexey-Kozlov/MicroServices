using Models;
using Newtonsoft.Json;
using Services;

namespace Identity.Services
{
    public class BrokerService :  BaseService, IBrokerService
    {
        private readonly IConfiguration _config;
        public BrokerService(IConfiguration config, IHttpClientFactory clientFactory) : 
            base(clientFactory)
        {
            _config = config;
        }

        public async Task SendToLog<T>(T logObject, string action, string token)
        {
            var data = new LogMessageDTO
            {
                typeName = typeof(T).ToString(),
                action = action,
                data = JsonConvert.SerializeObject(logObject!)
            };
            await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["RabbitProducer"]! + "/api/rabbitsend",
                Data = data!,
                Token = token
            });
        }
    }
}
