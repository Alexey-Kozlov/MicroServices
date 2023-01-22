using RabbitConsumer.Models;

namespace RabbitConsumer.Services
{
    public interface ISaveDb
    {
        Task SaveMessage<T>(T message);
    }
}
