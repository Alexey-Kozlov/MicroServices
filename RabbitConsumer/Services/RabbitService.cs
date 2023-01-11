using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitConsumer.Models;
using RabbitConsumer.Persistance;
using AutoMapper;
using RabbitConsumer.Domain;

namespace RabbitConsumer.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RabbitService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public void ConsumeMessage()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "admin";
            factory.VirtualHost = "/";
            factory.Port = 5671;
            factory.Ssl.CertPath = @"d:\Certificates\publicCert.pem";
            factory.Ssl.Enabled = true;
            factory.Ssl.ServerName = "192.168.1.10";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "MServices",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = JsonConvert.DeserializeObject<LogMessageDTO>(message);
                    var logMessage = _mapper.Map<LogMessage>(result);
                    _appDbContext.LogMessage.Add(logMessage);
                    await _appDbContext.SaveChangesAsync();
                };
                channel.BasicConsume(queue: "MServices",
                                             autoAck: true,
                                             consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}
