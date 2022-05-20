using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.Models;
using Crm.Link.UUID;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly SessionPublisher _sessionPublisher;
        private readonly IUUIDGateAway _uUIDGateAway;
        private readonly ILogger<SessionController> _logger;

        public SessionController(
            SessionPublisher sessionPublisher,
            IUUIDGateAway uUIDGateAway,
            ILogger<SessionController> logger)
        {
            _sessionPublisher = sessionPublisher;
            _uUIDGateAway = uUIDGateAway;
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

            // call uid
            var response = await _uUIDGateAway.GetGuid(meeting.Id, SourceEnum.CRM.ToString(), "Meeting");

            var @event = new SessionEvent
            {
                Source = SourceEnum.CRM,
                Title = meeting.Name,
                OrganiserUUID = "",
                StartDateUTC = meeting.StartDate,
                EndDateUTC = meeting.EndDate,
                IsActive = true,
                EntityType = "Meeting",
                SourceEntityId = meeting.Id
            };

            if (response == null)
            {
                var resp = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), "Account", meeting.Id, 1);
                @event.EntityVersion = 1;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.CREATE;
            }
            else
            {
                var resp = await _uUIDGateAway.UpdateEntity(meeting.Id, SourceEnum.CRM.ToString(), "Account");
                @event.EntityVersion = resp.EntityVersion;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.UPDATE;
            }

            _sessionPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // uuid
            var response = await _uUIDGateAway.GetGuid(id, SourceEnum.CRM.ToString(), "Meeting");
            if (response != null)
            {
                var @event = new SessionEvent
                {
                    UUID_Nr = response.Uuid.ToString(),
                    Method = MethodEnum.DELETE,
                };

                _sessionPublisher.Publish(@event);

                return Ok();
            }

            _logger.LogError("response UUIDMaster was null for: {id}", id);
            return BadRequest();
        }
    }
}
