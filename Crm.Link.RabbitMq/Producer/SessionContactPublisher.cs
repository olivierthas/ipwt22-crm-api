using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.RabbitMq.Producer
{
    public class SessionContactPublisher : ProducerBase<SessionAttendeeEvent>
    {
        public SessionContactPublisher(
            ConnectionProvider connectionProvider,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<SessionAttendeeEvent>> producerBaseLogger)
            : base(connectionProvider, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CrmAttendeeSession";

        protected override string RoutingKeyName => "";

        protected override string AppId => "Crm";
    }
}
