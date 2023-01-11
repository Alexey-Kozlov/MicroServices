namespace RabbitConsumer.Models
{
    public class LogMessageDTO
    {
        public LogMessageDTO() { }

        public string typeName { get; set; }
        public string action { get; set; }
        public string data { get; set; }
        public string userName { get; set; }
    }
}
