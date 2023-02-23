using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitProducer.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly IUserAccessor _userAccessor;
        public RabbitService(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public void SendMessage(LogMessageDTO messageText)
        {
            var factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "admin";
            factory.HostName = "localhost";
            //factory.VirtualHost = "/";
            factory.Port = 5672;
            //factory.Ssl.CertPath = @"d:\Certificates\publicCert.pem";
            //factory.Ssl.Enabled = true;
            //factory.Ssl.ServerName = "192.168.1.10";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "MServices",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                messageText.userName = _userAccessor.GetUserName();
                string message = JsonConvert.SerializeObject(messageText);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "",
                                     routingKey: "MServices",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
