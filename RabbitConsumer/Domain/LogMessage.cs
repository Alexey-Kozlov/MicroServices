
namespace RabbitConsumer.Domain
{
    public class LogMessage
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string TypeName { get; set; }
        public string TypeData { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
