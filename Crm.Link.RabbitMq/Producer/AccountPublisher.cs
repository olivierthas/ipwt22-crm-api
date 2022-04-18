using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Crm.Link.RabbitMq.Producer
{
    public class AccountPublisher : ProducerBase<AttendeeEvent>
    {
        public AccountPublisher(
            ConnectionProvider connectionProvider,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<AttendeeEvent>> producerBaseLogger)
            : base(connectionProvider, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "Accounts";

        protected override string RoutingKeyName => "weet ik niet";

        protected override string AppId => "Crm";
    }
}
