using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Hosting;

namespace Crm.Link.RabbitMq.Producer
{
    public class LogBackGroundTask : BackgroundService
    {
        private readonly IRabbitMqProducer<LogIntegrationEvent> _producer;
        public LogBackGroundTask(IRabbitMqProducer<LogIntegrationEvent> producer) => _producer = producer;
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var @event = new LogIntegrationEvent
                {
                    Id = Guid.NewGuid(),
                    Message = $"Hello! Message generated at {DateTime.UtcNow.ToString("O")}"
                };

                _producer.Publish(@event);
                await Task.Delay(20000, stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
