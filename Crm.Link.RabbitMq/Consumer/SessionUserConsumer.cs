using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.GateAway;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crm.Link.RabbitMq.Consumer
{
    public class SessionUserConsumer : ConsumerBase<SessionAttendeeEvent>, IHostedService
    {
        private readonly ISessionGateAway _sessionGateAway;

        public SessionUserConsumer(
            ConnectionProvider connectionProvider,
            ILogger<ConsumerBase<SessionAttendeeEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            ISessionGateAway sessionGateAway)
            : base(connectionProvider, consumerLogger, logger)
        {
            _sessionGateAway = sessionGateAway;
        }

        protected override string QueueName => "CrmSessionAttendee";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (Channel is not null)
            {
                try
                {
                    var consumer = new AsyncEventingBasicConsumer(Channel);
                    consumer.Received += OnEventReceived;
                    Channel?.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Error while consuming message");
                    SetTimer();
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

        protected async override Task HandelMessage(SessionAttendeeEvent? messageObject)
        {
            try
            {
                _ = messageObject ?? throw new ArgumentNullException(nameof(messageObject));

                switch (messageObject.EntityType)
                {
                    case "Contact":
                        await _sessionGateAway.AddUserToSession("Contacts", messageObject.AttendeeUUID, messageObject.SessionUUID);
                        break;
                    case "Account":
                        await _sessionGateAway.AddUserToSession("Accounts", messageObject.AttendeeUUID, messageObject.SessionUUID);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "post to crm error: ");
                throw;
            }
        }
    }
}
