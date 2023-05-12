using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitConsumer.Models;
using RabbitConsumer.Persistance;
using AutoMapper;
using RabbitConsumer.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using System.Linq.Expressions;

namespace RabbitConsumer.Services
{
    public class RabbitConsumerService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitConsumerService> _logger;
        private readonly IMapper _mapper;
        //private readonly ISaveDb _saveDb;
        public RabbitConsumerService(IConfiguration configuration,
            ILogger<RabbitConsumerService> logger, AppDbContext appDbContext, IMapper mapper
            //ISaveDb saveDb
            )
        {
            _appDbContext= appDbContext;
            _configuration = configuration; 
            _mapper = mapper;
            _logger = logger;
            //_saveDb = saveDb;
            try
            {
                _logger.LogInformation("Init rabbit consuner service");
                _configuration = configuration;
                var factory = new ConnectionFactory();
                factory.HostName = _configuration.GetValue<string>("RABBIT_SERVICE_NAME");
                factory.Port = _configuration.GetValue<int>("RABBIT_SERVICE_PORT");
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(
                    exchange: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"),
                    ExchangeType.Topic
                    );
                _channel.QueueDeclare(
                    queue: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"), 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null);
                _channel.QueueBind(
                    queue: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"),
                    exchange: _configuration.GetValue<string>("RABBIT_QUEUE_NAME"),
                    routingKey:  "somekey",
                    arguments: null
                    );
                _channel.BasicQos(
                    prefetchSize: 0,
                    prefetchCount: 1,
                    global: false
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitConsumer error - {ex.Message}");
                throw new Exception("Cannot run - " + ex.Message);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (ch, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogInformation("Get new message - " + message);
                        var result = JsonConvert.DeserializeObject<LogMessageDTO>(message);

                        var logMessage = _mapper.Map<LogMessage>(result);
                        _appDbContext.LogMessage.Add(logMessage);
                        _appDbContext.SaveChanges();
                        //образец кода для синхронизации записи в БД через семафор
                        //await _saveDb.SaveMessage(result!);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Ошибка записи - " + ex.Message);
                    }
                };
                consumer.Shutdown += Consumer_Shutdown;
                consumer.Registered += Consumer_Registered;
                consumer.Unregistered += Consumer_Unregistered;
                consumer.ConsumerCancelled += Consumer_ConsumerCancelled;
                _channel.BasicConsume(
                    _configuration.GetValue<string>("RABBIT_QUEUE_NAME"), 
                    false, 
                    consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitConsumer error - {ex.Message}");
            }
            return Task.CompletedTask;
        }

        private void Consumer_ConsumerCancelled(object? sender, ConsumerEventArgs e)
        {
            _logger.LogError($"ConsumerCancelled - {e.ConsumerTags}");
        }

        private void Consumer_Unregistered(object? sender, ConsumerEventArgs e)
        {
            _logger.LogError($"Unregistered - {e.ConsumerTags}");
        }

        private void Consumer_Registered(object? sender, ConsumerEventArgs e)
        {
            _logger.LogError($"Registered - {e.ConsumerTags}");
        }

        private void Consumer_Shutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogError($"Shutdown - {e.Cause}");
        }

        public override void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
            }
            if (_connection != null)
            {
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
