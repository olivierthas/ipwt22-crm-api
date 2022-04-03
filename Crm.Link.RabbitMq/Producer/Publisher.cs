using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Hosting;

namespace Crm.Link.RabbitMq.Producer
{
    public class Publisher
    {
        private readonly IRabbitMqProducer<IntegrationEvent> _producer;
        public Publisher(IRabbitMqProducer<IntegrationEvent> producer) => _producer = producer;
        protected async Task Publish(IntegrationEvent message, CancellationToken stoppingToken)
        {

            _producer.Publish(message);            

            await Task.CompletedTask;
        }
    }
}
