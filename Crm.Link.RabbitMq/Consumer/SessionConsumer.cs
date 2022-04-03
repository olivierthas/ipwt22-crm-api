﻿using Crm.Link.RabbitMq.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crm.Link.RabbitMq.Consumer
{
    public class SessionConsumer : ConsumerBase, IHostedService
    {
        private readonly ILogger<SessionConsumer> sessionLogger;

        protected override string QueueName => "Session";

        public SessionConsumer(
            IConnectionFactory connectionFactory,
            ILogger<SessionConsumer> sessionLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, consumerLogger, logger)
        {
            this.sessionLogger = sessionLogger;
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
                sessionLogger.LogCritical(ex, "Error while consuming message");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Channel.Dispose();

            return Task.CompletedTask;
        }
    }
}
