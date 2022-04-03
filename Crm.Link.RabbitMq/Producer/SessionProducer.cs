using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.RabbitMq.Producer
{
    public class SessionProducer : ProducerBase<SessionEvent>
    {
        public SessionProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<SessionEvent>> producerBaseLogger)
            : base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "Session";

        protected override string RoutingKeyName => "Weet ik niet";

        protected override string AppId => "Crm";
    }
}
