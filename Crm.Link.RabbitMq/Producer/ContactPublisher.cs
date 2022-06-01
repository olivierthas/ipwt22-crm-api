using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;

namespace Crm.Link.RabbitMq.Producer
{
    public class ContactPublisher : ProducerBase<AttendeeEvent>
    {
        public ContactPublisher(ConnectionProvider connectionProvider, ILogger<RabbitMqClientBase> logger, ILogger<ProducerBase<AttendeeEvent>> producerBaseLogger) : base(connectionProvider, logger, producerBaseLogger)
        {
        }

        public override string ClientType => "PUBLISHER";

        protected override string ExchangeName => "CrmAttendee";

        protected override string RoutingKeyName => "";

        protected override string AppId => "Crm";
    }
}
