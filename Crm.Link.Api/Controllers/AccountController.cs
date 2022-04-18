using Crm.Link.Api.GateAway;
using Crm.Link.Api.Models;
using Crm.Link.RabbitMq.Producer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            // var response = await accountGateAway.Create();

            // var text = await response.Content.ReadAsStringAsync();
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
                UUId = Guid.NewGuid().ToString(), // get uuid from uuidmaster
                CrudMethode = RabbitMq.Messages.MethodeEnum.CREATE,
                Name = account.Name,
                LastName = account.Name,
                Email = account.Email,
                VatNumber = null,
                Version = 1,
            };

            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update()
        {
            return Ok();
        }
    }
}
