using Models;

namespace RabbitProducer.Services
{
    public interface IRabbitService
    {
        void SendMessage(LogMessageDTO messageText);
    }
}
