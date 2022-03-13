namespace Crm.Link.RabbitMq.Producer
{
    public class LogIntegrationEvent
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
