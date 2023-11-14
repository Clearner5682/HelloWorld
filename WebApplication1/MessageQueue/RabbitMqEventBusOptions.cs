namespace WebApplication1.MessageQueue
{
    public class RabbitMqEventBusOptions
    {
        public string HostName { get; set; }

        public string ExchangeType { get; set; } = "direct";
        public ExchangeAndQueue Normal { get; set; }
        public ExchangeAndQueue DeadLetter { get; set; }
    }

    public class ExchangeAndQueue
    {
        public string ClientName { get; set; }
        public string ExchangeName { get; set; }
    }
}
