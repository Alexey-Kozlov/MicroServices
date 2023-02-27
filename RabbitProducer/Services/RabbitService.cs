using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitProducer.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<RabbitService> _logger;
        public RabbitService(IConfiguration configuration, IUserAccessor userAccessor, 
            ILogger<RabbitService> logger)
        {
            _userAccessor = userAccessor;
            _configuration = configuration;
            _logger = logger;   
        }

        public void SendMessage(LogMessageDTO messageText)
        {
            var factory = new ConnectionFactory();
            factory.Port = _configuration.GetValue<int>("RABBIT_SERVICE_PORT");
            factory.HostName = _configuration.GetValue<string>("RABBIT_SERVICE_NAME");
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"),
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                    messageText.userName = _userAccessor.GetUserName();
                    _logger.LogInformation("messageText.userName - " + messageText.userName);
                    string message = JsonConvert.SerializeObject(messageText);
                    _logger.LogInformation("userName serialized -  " + message);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "",
                                         routingKey: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"),
                                         basicProperties: null,
                                         body: body);

                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Ошибка передачи сообщения - " + ex.Message);
            }
        }
    }
}
