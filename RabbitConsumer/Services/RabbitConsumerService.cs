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
    public class RabbitConsumerService : IRabbitConsumerService
    {
        private readonly ISaveDb _saveDb;
        public RabbitConsumerService(ISaveDb saveDb)
        {
            _saveDb = saveDb;
        }

        public void ConsumeMessage()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "admin";
            factory.HostName = "localhost";
            //factory.VirtualHost = "/";
            factory.Port = 5672;
            //factory.Ssl.CertPath = @"d:\Certificates\publicCert.pem";
            //factory.Ssl.Enabled = true;
            //factory.Ssl.ServerName = "localhost";
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
                    await _saveDb.SaveMessage(result!);
                };
                channel.BasicConsume(queue: "MServices",
                                             autoAck: true,
                                             consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}
