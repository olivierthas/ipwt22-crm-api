using Crm.Link.Api.GateAway;
using Crm.Link.Api.Models;
using Crm.Link.RabbitMq.Producer;
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

            var @event = new ContactEvent
            {
                UUID = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                Methode = RabbitMq.Messages.MethodeEnum.CREATE,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Version = 1,
    };
            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _ = Contact ?? throw new ArgumentNullException(nameof(contact));
            // map data naar xml

            var @event = new ContactEvent
            {
                UUID = Guid.GetGuid(id).ToString(), // haal bijhorende UUID op
                Methode = RabbitMq.Messages.MethodeEnum.DELETE,
            };
            accountPublisher.Publish(@event);
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
