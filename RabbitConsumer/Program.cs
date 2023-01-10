using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
    };
    channel.BasicConsume(queue: "MServices",
                                 autoAck: true,
                                 consumer: consumer);
    Console.ReadLine();

}
