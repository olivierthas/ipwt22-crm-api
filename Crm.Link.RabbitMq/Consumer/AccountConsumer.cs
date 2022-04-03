using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crm.Link.RabbitMq.Consumer
{
    public class AccountConsumer : ConsumerBase, IHostedService
    {
        protected override string QueueName => "Accounts";
        private readonly ILogger<AccountConsumer> accountLogger;

        public AccountConsumer(
            IConnectionFactory connectionFactory,
            ILogger<AccountConsumer> accountLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, consumerLogger, logger)
        {
            this.accountLogger = accountLogger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<LogCommand>;
                Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                accountLogger.LogCritical(ex, "Error while consuming message");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
