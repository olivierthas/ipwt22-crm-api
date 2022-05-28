using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Crm.Link.UUID;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly ContactPublisher _contactPublisher;
        private readonly ILogger<ContactController> _logger;
        private readonly IUUIDGateAway _uUIDGateAway;

        public ContactController(
            ContactPublisher contactPublisher,
            ILogger<ContactController> logger,
            IUUIDGateAway uUIDGateAway)
        {
            _contactPublisher = contactPublisher;
            _logger = logger;
            _uUIDGateAway = uUIDGateAway;
        }

        [HttpGet]
        [Route(nameof(Test))]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(ContactModel contact)
        {
            if (contact == null)
            {
                var date = DateTime.UtcNow;
                _logger.LogError("BadRequest on SessionController : {date}", date);
                return BadRequest();
            }

            // call uid
            var response = await _uUIDGateAway.GetGuid(contact.Id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Attendee);

            var @event = new AttendeeEvent
            {
                UUID_Nr = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                SourceEntityId = contact.Id,
                EntityType = UUID.Model.EntityTypeEnum.Attendee.ToString(),
                Method = MethodEnum.CREATE,
                Name = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
            };

            if (response == null)
            {
                var resp = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Attendee, contact.Id, 1);
                @event.EntityVersion = 1;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.CREATE;
            }
            else
            {
                var resp = await _uUIDGateAway.UpdateEntity(contact.Id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Attendee);
                @event.EntityVersion = resp.EntityVersion;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.UPDATE;
            }
            _contactPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _uUIDGateAway.GetGuid(id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Attendee);
            if (response != null)
            {
                var @event = new AttendeeEvent
                {
                    UUID_Nr = response.Uuid.ToString(),
                    Method = MethodEnum.DELETE,
                };

                _contactPublisher.Publish(@event);

                return Ok();
            }

            _logger.LogError("response UUIDMaster was null for: {id}", id);
            return Ok();
        }
    }
}
