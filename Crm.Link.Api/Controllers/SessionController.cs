using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly SessionPublisher _sessionPublisher;

        public SessionController(SessionPublisher sessionPublisher)
        {
            _sessionPublisher = sessionPublisher;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(MeetingModel meeting)
        {
            // call uid

            var @event = new SessionEvent
            {
                UUID_Nr = "",
                EntityVersion = 1,
                Method = MethodEnum.CREATE,
                Source = SourceEnum.CRM,
                Title = meeting.Name,
                OrganiserUUID = "",
                StartDateUTC = meeting.StartDate,
                EndDateUTC = meeting.EndDate,
                IsActive = false,
                EntityType = "SuperBrol",
                SourceEntityId = meeting.Id
            };

            _sessionPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // uuid
            var @event = new SessionEvent
            {
                UUID_Nr = "",
                Method = MethodEnum.DELETE,
            };

            _sessionPublisher.Publish(@event);
            return Ok();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(MeetingModel meeting)
        {
            // call uid

            var @event = new SessionEvent
            {
                UUID_Nr = "",
                EntityVersion = 1,
                Method = MethodEnum.CREATE,
                Source = SourceEnum.CRM,
                Title = meeting.Name,
                OrganiserUUID = "",
                StartDateUTC = meeting.StartDate,
                EndDateUTC = meeting.EndDate,
                IsActive = false,
                EntityType = "SuperBrol",
                SourceEntityId = meeting.Id
            };

            _sessionPublisher.Publish(@event);

            return Ok();
        }
    }
}
