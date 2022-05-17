using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IAccountGateAway accountGateAway;
        private readonly AccountPublisher accountPublisher;

        public ContactController(IAccountGateAway accountGateAway, AccountPublisher accountPublisher)
        {

            this.accountGateAway = accountGateAway;
            this.accountPublisher = accountPublisher;
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
            _ = contact ?? throw new ArgumentNullException(nameof(contact));
            // map data naar xml

            var @event = new AttendeeEvent
            {
                UUID_Nr = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                Method = MethodEnum.CREATE,
                Name = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Version = 1,
    };
            ///accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // map data naar xml

            var @event = new AttendeeEvent
            {
                UUID_Nr = "", // haal bijhorende UUID op
                Method = MethodEnum.DELETE,
            };
            //ContactPublisher.Publish(@event);
            return Ok();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(AccountModel account)
        {
            return Ok();
        }
    }
}
