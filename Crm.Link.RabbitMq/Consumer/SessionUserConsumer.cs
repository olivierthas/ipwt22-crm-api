using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.UUID;
using Crm.Link.UUID.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Crm.Link.RabbitMq.Consumer
{
    public class SessionUserConsumer : ConsumerBase<SessionAttendeeEvent>, IHostedService
    {
        private readonly ISessionGateAway _sessionGateAway;
        private readonly IUUIDGateAway _uUIDGateAway;

        public SessionUserConsumer(
            ConnectionProvider connectionProvider,
            ILogger<ConsumerBase<SessionAttendeeEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            ISessionGateAway sessionGateAway,
            IUUIDGateAway uUIDGateAway)
            : base(connectionProvider, consumerLogger, logger)
        {
            _sessionGateAway = sessionGateAway;
            _uUIDGateAway = uUIDGateAway;
        }

        public override string ClientType => "CONSUMER";

        protected override string QueueName => "CrmAttendeeSession";

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
                    _logger.LogCritical(ex, "Error while binding to queue.");
                    SetTimer();
                }
            }
            else
            {
                _logger.LogCritical("Channel was null - starting timer.");
                SetTimer();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected async override Task HandleMessage(SessionAttendeeEvent? messageObject)
        {
            Thread.Sleep(2000);
            try
            {
                _ = messageObject ?? throw new ArgumentNullException(nameof(messageObject));
                
                ResourceDto? session = null;
                ResourceDto? contact = null;
                if (Guid.TryParse(messageObject.SessionUUID, out var sId) && Guid.TryParse(messageObject.AttendeeUUID, out var aId))
                {
                    session = await _uUIDGateAway.GetResource(sId, SourceEnum.CRM.ToString());
                    contact = await _uUIDGateAway.GetResource(aId, SourceEnum.CRM.ToString());
                    if (session == null || contact == null)
                    {
                        _logger.LogError("uuid response was null session: {id1} - contact: {id2}", new[] { sId, aId });
                    }

                    switch (messageObject.Method)
                    {
                        case MethodEnum.CREATE:
                        case MethodEnum.UPDATE:
                            await _sessionGateAway.AddUserToSession("Contacts", contact!.SourceEntityId, session!.SourceEntityId);
                            break;                            
                        case MethodEnum.DELETE:
                            await _sessionGateAway.RemoveUserFromSession("Contact", contact!.SourceEntityId, session!.SourceEntityId);
                            break;
                        default:
                            _logger.LogError("methode notFound: {method}", messageObject.Method);
                            break;
                    }
                }

                _logger.LogError("uuidNumber not falid session: {uuid}, user: {uuid2}", new[] { messageObject.SessionUUID, messageObject.AttendeeUUID });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "post to crm error: ");
                throw;
            }
        }
    }
}
