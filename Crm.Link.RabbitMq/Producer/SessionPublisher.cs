﻿using Crm.Link.RabbitMq.Common;
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

        protected override string ExchangeName => "Session";

        protected override string RoutingKeyName => "Weet ik niet";

        protected override string AppId => "Crm";
    }
}
