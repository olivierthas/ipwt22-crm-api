using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;

namespace Crm.Link.RabbitMq.Producer
{
    public class AccountPublisher : ProducerBase<AccountEvent>
    {
        public AccountPublisher(
            ConnectionProvider connectionProvider,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<AccountEvent>> producerBaseLogger)
            : base(connectionProvider, logger, producerBaseLogger)
        {
        }

        public override string ClientType => "PUBLISHER";

        protected override string ExchangeName => "CrmAccount";

        protected override string RoutingKeyName => "";

        protected override string AppId => "Crm";
    }
}
