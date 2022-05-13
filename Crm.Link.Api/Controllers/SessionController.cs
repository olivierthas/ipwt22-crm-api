using Crm.Link.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        public SessionController()
        {

        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(MeetingModel meeting)
        {
            _ = meeting ?? throw new ArgumentNullException(nameof(meeting));
            // map data naar xml

            var @event = new MeetingEvent
            {
                UUID = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                Methode = RabbitMq.Messages.MethodeEnum.CREATE,
                Name = meeting.Name,
                Description = meeting.Description,
                Location = meeting.Location,
                StartDate = meeting.StartDate,
                EndDate = meeting.EndDate,
                OutlookID = meeting.OutlookID,
                Status = meeting.Status,
                Version = 1,
            };
            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _ = meeting ?? throw new ArgumentNullException(nameof(meeting));
            // map data naar xml

            var @event = new MeetingEvent
            {
                UUID = Guid.GetGuid(id).ToString(), // haal bijhorende UUID op
                Methode = RabbitMq.Messages.MethodeEnum.DELETE,
            };
            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(MeetingModel meeting)
        {
            return Ok();
        }
    }
}
