using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Crm.Link.UUID;
using Crm.Link.UUID.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace Crm.Link.RabbitMq.Consumer
{
    public class ContactConsumer : ConsumerBase<AttendeeEvent>, IHostedService
    {
        protected override string QueueName => "Contact";
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
                    //Channel?.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
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
            var crmObject = new ContactModel
            {
                FirstName = $"{messageObject.Name}",
                LastName = messageObject.LastName,
                Email = messageObject.Email,
            };

            ResourceDto response;
            switch (messageObject.Method)
            {
                case MethodEnum.CREATE:
                    var resp = await _contactGateAway.CreateOrUpdate(crmObject);

                    var test = await resp.Content.ReadAsStringAsync();
                    _logger.LogInformation(test);
                    //add to uuid
                    if (resp.IsSuccessStatusCode)
                    {
                        await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), EntityTypeEnum.Attendee, "0000", 1);
                    }
                    break;
                case MethodEnum.UPDATE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
                    {
                        response = await _uUIDGateAway.GetResource(id);
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - contact");
                        }
                        else
                        {
                            crmObject.Id = response.SourceEntityId;
                            var result = await _contactGateAway.CreateOrUpdate(crmObject);

                            if (result.IsSuccessStatusCode)
                            {
                                await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), EntityTypeEnum.Attendee, messageObject.EntityVersion);
                            }
                        }
                    }
                    break;
                case MethodEnum.DELETE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out var mesId))
                    {
                        var del = await _uUIDGateAway.GetResource(mesId);
                        await _contactGateAway.Delete(del.SourceEntityId);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
