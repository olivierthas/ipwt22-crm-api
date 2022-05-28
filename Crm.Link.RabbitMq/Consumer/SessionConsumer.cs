using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
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
        private readonly SessionPublisher _sessionPublisher;

        protected override string QueueName => "CrmSession";

        public SessionConsumer(
            ConnectionProvider connectionProvider,
            ILogger<SessionConsumer> sessionLogger,
            ILogger<ConsumerBase<SessionEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            IUUIDGateAway uUIDGateAway,
            ISessionGateAway sessionGateAway,
            SessionPublisher sessionPublisher) :
            base(connectionProvider, consumerLogger, logger)
        {
            this.sessionLogger = sessionLogger;
            _uUIDGateAway = uUIDGateAway;
            _sessionGateAway = sessionGateAway;
            _sessionPublisher = sessionPublisher;
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

        protected async override Task HandleMessage(SessionEvent messageObject)
        {

            ResourceDto? response;

            var crmObject = new MeetingModel
            {
                Name = messageObject.Title,
                EndDate = messageObject.EndDateUTC,
                StartDate = messageObject.StartDateUTC
            };
            switch (messageObject.Method)
            {
                case MethodEnum.CREATE:

                    var resp = await _sessionGateAway.CreateOrUpdate(crmObject);
                    
                    var test = await resp.Content.ReadAsStringAsync(); //wat are you
                    _logger.LogInformation(test);

                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogError("Response from suiteCrm was not Ok : {tostring}", crmObject.ToString());
                        return;
                    }

                    _ = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), EntityTypeEnum.Account, "", 1);                    

                    break;
                case MethodEnum.UPDATE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
                    {
                        response = await _uUIDGateAway.GetResource(id, SourceEnum.CRM.ToString());
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - session - guid: {id}", id);
                            return;
                        }
                        else
                        {
                            crmObject.Id = response.SourceEntityId;
                            var result = await _sessionGateAway.CreateOrUpdate(crmObject);

                            if (result.IsSuccessStatusCode)
                            {
                                await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Account, messageObject.EntityVersion);
                            }
                        }

                        return;
                    }

                    _logger.LogError("uuiDNumber not falid: {uuid}", messageObject.UUID_Nr);
                    break;
                case MethodEnum.DELETE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out var mesId))
                    {
                        var del = await _uUIDGateAway.GetResource(mesId, SourceEnum.CRM.ToString());
                        if (del == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - account - guid: {id}", mesId);
                            return;
                        }
                        await _sessionGateAway.Delete(del!.SourceEntityId);
                        return;
                    }

                    _logger.LogError("uuidNumber not falid: {uuid}", messageObject.UUID_Nr);
                    break;
                default:
                    _logger.LogError("methode notFound: {method}", messageObject.Method);
                    break;
            }

            
        }
    }
}
