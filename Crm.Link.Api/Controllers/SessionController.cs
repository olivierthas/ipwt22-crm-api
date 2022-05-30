using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Crm.Link.UUID;
using Crm.Link.UUID.Model;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly SessionPublisher _sessionPublisher;
        private readonly SessionContactPublisher _sessionContactPublisher;
        private readonly IUUIDGateAway _uUIDGateAway;
        private readonly ISessionGateAway _sessionGateAway;
        private readonly ILogger<SessionController> _logger;

        public SessionController(
            SessionPublisher sessionPublisher,
            SessionContactPublisher sessionContactPublisher,
            IUUIDGateAway uUIDGateAway,
            ISessionGateAway sessionGateAway,
            ILogger<SessionController> logger)
        {
            _sessionPublisher = sessionPublisher;
            _sessionContactPublisher = sessionContactPublisher;
            _uUIDGateAway = uUIDGateAway;
            _sessionGateAway = sessionGateAway;
            _logger = logger;
        }

        [HttpGet]
        [Route(nameof(Test))]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(MeetingModel meeting)
        {
            if (meeting == null)
            {
                var date = DateTime.UtcNow;
                _logger.LogError("BadRequest on SessionController : {date}", date);
                return BadRequest();
            }
            
            
            var response = await _uUIDGateAway.GetGuid(meeting.Id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Session);
            
            _logger.LogInformation($"{meeting.StartDate}, {meeting.EndDate}");

            var @event = new SessionEvent
            {
                Source = SourceEnum.CRM,
                Title = meeting.Name,
                OrganiserUUID = "",
                StartDateUTC = meeting.StartDate == DateTime.MinValue ? DateTime.UtcNow.AddHours(2) : meeting.StartDate,
                EndDateUTC = meeting.EndDate == DateTime.MinValue ? DateTime.UtcNow.AddHours(3) : meeting.EndDate,
                IsActive = true,
                EntityType = "Meeting",
                SourceEntityId = meeting.Id
            };
            var type = MapEntityType(meeting.ParentType);            

            if (type != null)
            {
                var organizer = await _uUIDGateAway.GetGuid(meeting.ParentId, SourceEnum.CRM.ToString(), type.Value);
                if (organizer != null)
                    @event.OrganiserUUID = organizer.Uuid.ToString();
            }

            ResourceDto? resp;
            if (response == null)
            {
                resp = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Session, meeting.Id, 1);
                if (resp == null)
                {
                    _logger.LogError("uuid response was null: {tostring}", meeting.ToString());
                    return Ok();
                }

                @event.EntityVersion = 1;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.CREATE;
            }
            else
            {
                resp = await _uUIDGateAway.UpdateEntity(meeting.Id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Session);
                if (resp == null)
                {
                    _logger.LogError("uuid response was null: {tostring}", meeting.ToString());
                    return Ok();
                }

                @event.EntityVersion = resp!.EntityVersion;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.UPDATE;
            }

            _sessionPublisher.Publish(@event);

            var contacts = await _sessionGateAway.GetContacts(meeting.Id);
            
            if (contacts != null)
            {
                foreach(var contact in contacts.Data)
                {
                    var contactUuid = await _uUIDGateAway.GetGuid(contact.Id, SourceEnum.CRM.ToString(), EntityTypeEnum.Attendee);
                    if (contactUuid == null)
                    {
                        _logger.LogError("response uuid was null for contact id: {id}", contact.Id);
                        continue;
                    }

                    var sessionContact = new SessionAttendeeEvent
                    {
                        EntityType = EntityTypeEnum.SessionAttendee.ToString(),
                        AttendeeUUID = contactUuid.Uuid.ToString(),
                        InvitationStatus = InvitationStatusEnum.PENDING,
                        Method = MethodEnum.CREATE,
                        SessionUUID = resp.Uuid.ToString(),
                        EntityVersion = 1,
                        Source = SourceEnum.CRM,
                        SourceEntityId = 0,
                        UUID_Nr = ""
                    };

                    _sessionContactPublisher.Publish(sessionContact);
                }
            }

            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // uuid
            var response = await _uUIDGateAway.GetGuid(id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Session);
            if (response != null)
            {
                var @event = new SessionEvent
                {
                    UUID_Nr = response.Uuid.ToString(),
                    Method = MethodEnum.DELETE,
                    EndDateUTC = DateTime.UtcNow,
                    StartDateUTC = DateTime.UtcNow,
                    EntityType = EntityTypeEnum.Session.ToString(),
                    EntityVersion = response.EntityVersion,
                    OrganiserUUID = "",
                    Source = SourceEnum.CRM,
                    SourceEntityId = id,
                    Title = "",
                    IsActive = false,
                };

                _sessionPublisher.Publish(@event);

                return Ok();
            }

            _logger.LogError("response UUIDMaster was null for: {id}", id);
            return BadRequest();
        }

        private UUID.Model.EntityTypeEnum? MapEntityType(string type)
        {
            return type.ToLower() switch
            {
                "contacts" => UUID.Model.EntityTypeEnum.Attendee,
                "accounts" => UUID.Model.EntityTypeEnum.Account,
                "meetings" => UUID.Model.EntityTypeEnum.Session,
                _ => null,
            };
        }

    }
}
