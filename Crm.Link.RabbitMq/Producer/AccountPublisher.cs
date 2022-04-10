using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Crm.Link.RabbitMq.Producer
{
    public class AccountPublisher : ProducerBase<AttendeeEvent>
    {
        public AccountPublisher(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<AttendeeEvent>> producerBaseLogger)
            : base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "Accounts";

        protected override string RoutingKeyName => "weet ik niet";

        protected override string AppId => "Crm";
    }
}
