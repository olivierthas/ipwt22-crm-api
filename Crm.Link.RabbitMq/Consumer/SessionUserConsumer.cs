using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.GateAway;
using Microsoft.Extensions.Logging;

namespace Crm.Link.RabbitMq.Consumer
{
    public class SessionUserConsumer : ConsumerBase<SessionAttendeeEvent>
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

        protected async override Task HandelMessage(SessionAttendeeEvent messageObject)
        {
            try
            {
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
            }
        }
    }
}
