using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.RabbitMq.Consumer
{
    public class ContactConsumer : ConsumerBase<AttendeeEvent>, IHostedService
    {
        protected override string QueueName => "Contact";
        private readonly ILogger<AccountConsumer> _accountLogger;

        public ContactConsumer(
            ConnectionProvider connectionProvider,
            ILogger<AccountConsumer> accountLogger,
            ILogger<ConsumerBase<AttendeeEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger)
            : base(connectionProvider, consumerLogger, logger)
        {
            _accountLogger = accountLogger;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (Channel is not null)
            {
                try
                {
                    var consumer = new AsyncEventingBasicConsumer(Channel);
                    consumer.Received += OnEventReceived;
                    //Channel?.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
                }
                catch (Exception ex)
                {
                    _accountLogger.LogCritical(ex, "Error while consuming message");
                }
            }
            else
            {
                SetTimer();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task HandelMessage(AttendeeEvent messageObject)
        {
            throw new NotImplementedException();
        }
    }
}
