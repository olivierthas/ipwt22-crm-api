using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
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
    public class ContactConsumer : ConsumerBase<AttendeeEvent>, IHostedService
    {
        protected override string QueueName => "CrmAttendee";

        public override string ClientType => "CONSUMER";

        private readonly IContactGateAway _contactGateAway;
        private readonly IUUIDGateAway _uUIDGateAway;

        public ContactConsumer(
            ConnectionProvider connectionProvider,
            ILogger<ConsumerBase<AttendeeEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            IContactGateAway contactGateAway,
            IUUIDGateAway uUIDGateAway)
            : base(connectionProvider, consumerLogger, logger)
        {
            _contactGateAway = contactGateAway;
            _uUIDGateAway = uUIDGateAway;
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

        protected async override Task HandleMessage(AttendeeEvent messageObject)
        {   
            if (messageObject == null)
            {
                _logger.LogError("message was Empty - handelMessage - contact");
                return;
            }

            if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
            {
                ResourceDto? response;
                response = await _uUIDGateAway.GetResource(id, SourceEnum.CRM.ToString());

                var crmObject = new ContactModel
                {
                    FirstName = $"{messageObject.Name}",
                    LastName = messageObject.LastName,
                    Email = messageObject.Email,
                };

                var sendObject = new ModuleModel
                {
                    Data = new BaseModel
                    {
                        Type = "Contacts",
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
                            
                        var resp = await _contactGateAway.CreateOrUpdate(sendObject);                        
                        if (resp == null)
                        {
                            _logger.LogError("response suitecrm was null for entity: {entity}", sendObject.ToString());
                            throw new FieldAccessException();
                        }

                        _ = await _uUIDGateAway.PublishEntity(id, SourceEnum.CRM.ToString(), EntityTypeEnum.Attendee, resp.Data.Id, 1);                    
                        
                        break;
                    case MethodEnum.UPDATE:

                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - contact - guid: {id}", id);
                            return;
                        }

                        if (response.EntityVersion == messageObject.EntityVersion)
                            return;

                        crmObject.Id = response.SourceEntityId;
                        var result = await _contactGateAway.CreateOrUpdate(sendObject);

                        if (result == null)
                        {
                            _logger.LogError("response suitecrm was null for entity: {entity}", sendObject.ToString());
                            throw new FieldAccessException();
                        }
                        
                        await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), EntityTypeEnum.Attendee, messageObject.EntityVersion);
                        
                        break;
                    case MethodEnum.DELETE:                        
                        
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - account - guid: {id}", id);
                            return;
                        }
                        await _contactGateAway.Delete(response.SourceEntityId);                      
                        break;
                    default:
                        _logger.LogError("methode notFound: {method}", messageObject.Method);
                        break;
                }
                
                return;
            }            
            
            _logger.LogError("uuiDNumber not falid: {uuid}", messageObject.UUID_Nr);
        }
    }
}
