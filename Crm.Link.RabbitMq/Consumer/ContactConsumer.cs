using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.RabbitMq.Consumer
{
    public class ContactConsumer : ConsumerBase<AttendeeEvent>, IHostedService
    {
        public ContactConsumer(ConnectionProvider connectionProvider, ILogger<ConsumerBase<AttendeeEvent>> consumerLogger, ILogger<RabbitMqClientBase> logger) : base(connectionProvider, consumerLogger, logger)
        {
        }

        protected override string QueueName => "Contact";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void HandelMessage(AttendeeEvent messageObject)
        {
            throw new NotImplementedException();
        }
    }
}
