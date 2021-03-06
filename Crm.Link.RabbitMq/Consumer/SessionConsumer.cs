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

        public override string ClientType => "CONSUMER";

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
            Thread.Sleep(1000);
            if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
            {
                ResourceDto? contact = null;

                ResourceDto? response = await _uUIDGateAway.GetResource(id, SourceEnum.CRM.ToString());
                if (!string.IsNullOrWhiteSpace(messageObject.OrganiserUUID) && Guid.TryParse(messageObject.OrganiserUUID, out Guid contactId))
                {
                    contact = await _uUIDGateAway.GetResource(id, SourceEnum.CRM.ToString());
                }
                var crmObject = new MeetingModel
                {
                    Name = messageObject.Title,
                    StartDate = messageObject.StartDateUTC.ToLocalTime(),
                    DurationHours = (int)(messageObject.EndDateUTC - messageObject.StartDateUTC).TotalHours,
                    DurationMinutes = Math.Abs(messageObject.EndDateUTC.Minute - messageObject.StartDateUTC.Minute),
                    ParentType = "Contacts",
                    ParentId = contact == null ? "a379f70c-faea-36b4-be02-62420b0c7046" : contact.SourceEntityId,
                    Status = null,
                };

                var sendObject = new MeetingBaseObject
                {
                    Data = new MeetingData
                    {
                        Type = "Meetings",
                        Attributes = crmObject
                    }
                };

                switch (messageObject.Method)
                {
                    case MethodEnum.CREATE:
                        if (response != null)
                        {
                            _logger.LogError("entity already exist can't create: {entity}", sendObject.ToString());
                            return;
                        }

                        var resp = await _sessionGateAway.CreateOrUpdate(sendObject);                       
                        if (resp == null)
                        {
                            _logger.LogError("suiteCrm did not create entity: {entity}", sendObject.ToString());
                            throw new FieldAccessException();
                        }
                        _ = await _uUIDGateAway.PublishEntity(id, SourceEnum.CRM.ToString(), EntityTypeEnum.SESSION, resp.Data.Id, 1);                    

                        break;
                    case MethodEnum.UPDATE:                        
                        
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - session - guid: {id}", id);
                            throw new FieldAccessException();
                        }

                        if (response.EntityVersion == messageObject.EntityVersion)
                            return;
                        
                        crmObject.Id = response.SourceEntityId;
                        sendObject.Data.Id = response.SourceEntityId;
                        var result = await _sessionGateAway.CreateOrUpdate(sendObject);

                        if (result == null)
                        {
                            _logger.LogError("response suitecrm was null for entity: {entity}", sendObject.ToString());
                            throw new FieldAccessException();
                        }                       
                        
                        await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), EntityTypeEnum.SESSION, messageObject.EntityVersion);

                        break;
                    case MethodEnum.DELETE:                        
                        
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - account - guid: {id}", id);
                            return;
                        }
                        await _sessionGateAway.Delete(response!.SourceEntityId);

                        break;
                    default:
                        _logger.LogError("methode notFound: {method}", messageObject.Method);
                        break;
                }

                return;
            }
            _logger.LogError("uuidNumber not valid: {uuid}", messageObject.UUID_Nr);
        }
    }
}
