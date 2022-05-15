using Crm.Link.RabbitMq.Messages;
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
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(AccountModel account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));
            // map data naar xml

            // call uuid and get uuid_nr
            var @event = new AccountEvent
            {
                UUID_Nr = 0.ToString(),
                EntityType = "",
                EntityVersion = 1,
                Name = account.Name,
                LastName = "",
                Email = account.Email,
                Method = MethodEnum.CREATE, // if version is 1
                Source = SourceEnum.CRM,
                SourceEntityId = 0, // account.Id,
            };
            
            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public IActionResult Delete(string id)
        {
            // do a delete on uuid ? softDelete?
            // 

            var @event = new AccountEvent
            {
                UUID_Nr = "",
                Method = MethodEnum.DELETE,
            };

            accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public IActionResult Update(AccountModel account)
        {
            // call uuid 
            // get version number
            var @event = new AccountEvent
            {
                UUID_Nr = 0.ToString(),
                EntityType = "",
                EntityVersion = 20,
                Version = 1,
                Name = account.Name,
                LastName = "",
                Email = account.Email,
                Method = MethodEnum.UPDATE,
                Source = SourceEnum.CRM,
                SourceEntityId = 0, // account.Id, WTF we dont need this
            };

            accountPublisher.Publish(@event);

            return Ok();
        }
    }
}
