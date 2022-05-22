using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.UUID;
using Crm.Link.UUID.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crm.Link.RabbitMq.Consumer
{
    public class SessionConsumer : ConsumerBase<SessionEvent>, IHostedService
    {
        private readonly ILogger<SessionConsumer> sessionLogger;
        private readonly IUUIDGateAway _uUIDGateAway;
        private readonly ISessionGateAway _sessionGateAway;

        protected override string QueueName => "CrmSession";

        public SessionConsumer(
            ConnectionProvider connectionProvider,
            ILogger<SessionConsumer> sessionLogger,
            ILogger<ConsumerBase<SessionEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            IUUIDGateAway uUIDGateAway,
            ISessionGateAway sessionGateAway) :
            base(connectionProvider, consumerLogger, logger)
        {
            this.sessionLogger = sessionLogger;
            _uUIDGateAway = uUIDGateAway;
            _sessionGateAway = sessionGateAway;
            TimerMethode += async () => await StartAsync(new CancellationToken(false)); 
        }

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
                    sessionLogger.LogCritical(ex, "Error while binding to queue.");
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
            Channel?.Dispose();

            return Task.CompletedTask;
        }

        protected async override Task HandelMessage(SessionEvent messageObject)
        {

            ResourceDto response;

            switch (messageObject.Method)
            {
                case MethodEnum.CREATE:
                    break;
                case MethodEnum.UPDATE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
                    {
                        response = await _uUIDGateAway.GetResource(id);
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - account");
                        }
                        else
                        {
                            /*crmObject.Id = response.SourceEntityId;
                            var result = await _accountGateAway.CreateOrUpdate(crmObject);

                            if (result.IsSuccessStatusCode)
                            {
                                await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Account, messageObject.EntityVersion);
                            }*/
                        }
                    }
                    break;
                case MethodEnum.DELETE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out var mesId))
                    {
                        var del = await _uUIDGateAway.GetResource(mesId);
                        await _sessionGateAway.Delete(del.SourceEntityId);
                    }
                    break;
                default:
                    break;
            }

            
        }
    }
}
