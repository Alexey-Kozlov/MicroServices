using Identity.Controllers;
using Models;
using Newtonsoft.Json;
using Services;

namespace Identity.Services
{
    public class BrokerService :  BaseService, IBrokerService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BrokerService> _logger;
        public BrokerService(IConfiguration config, IHttpClientFactory clientFactory,
            ILogger<BrokerService> logger) : 
            base(clientFactory)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendToLog<T>(T logObject, string action, string token)
        {
            _logger.LogInformation("Рассылка - " + action + ", URL - " + _config["RABBIT_PRODUCER"]);
            try
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
                    Url = _config["RABBIT_PRODUCER"],
                    Data = data!,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Ошибка рассылки - " + ex.Message);
            }
        }
    }
}
