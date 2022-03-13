using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Crm.Link.RabbitMq.Producer
{
    public class LogProducer : ProducerBase<LogIntegrationEvent>
    {
        public LogProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<LogIntegrationEvent>> producerBaseLogger) :
            base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CUSTOM_HOST.LoggerExchange";
        protected override string RoutingKeyName => "log.message";
        protected override string AppId => "LogProducer";
    }
}
