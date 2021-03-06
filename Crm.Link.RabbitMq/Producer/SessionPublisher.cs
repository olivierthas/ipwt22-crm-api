using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;

namespace Crm.Link.RabbitMq.Producer
{
    public class SessionPublisher : ProducerBase<SessionEvent>
    {
        public SessionPublisher(
            ConnectionProvider connectionProvider,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<SessionEvent>> producerBaseLogger)
            : base(connectionProvider, logger, producerBaseLogger)
        {
        }

        public override string ClientType => "PUBLISHER";

        protected override string ExchangeName => "CrmSession";

        protected override string RoutingKeyName => "";

        protected override string AppId => "Crm";
    }
}
