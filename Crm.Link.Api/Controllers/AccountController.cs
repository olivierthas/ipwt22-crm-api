using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountGateAway accountGateAway;
        private readonly AccountPublisher accountPublisher;

        public AccountController(IAccountGateAway accountGateAway, AccountPublisher accountPublisher)
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
        public async Task<IActionResult> Create(AccountModel account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));
            // map data naar xml

            var @event = new AttendeeEvent
            {
                UUID = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                Methode = RabbitMq.Messages.MethodeEnum.CREATE,
                Name = account.Name,
                LastName = account.Name,
                Email = account.Email,
                VatNumber = "",
                Version = 1,
            };

            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
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
