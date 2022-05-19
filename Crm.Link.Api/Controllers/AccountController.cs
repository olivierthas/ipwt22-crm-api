using Crm.Link.RabbitMq.Messages;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.Suitcrm.Tools.Models;
using Crm.Link.UUID;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountGateAway _accountGateAway;
        private readonly AccountPublisher _accountPublisher;
        private readonly IUUIDGateAway _uUIDGateAway;

        public AccountController(
            IAccountGateAway accountGateAway,
            AccountPublisher accountPublisher,
            IUUIDGateAway uUIDGateAway)
        {

            _accountGateAway = accountGateAway;
            _accountPublisher = accountPublisher;
            _uUIDGateAway = uUIDGateAway;
        }

        [HttpGet]
        [Route(nameof(Test))]
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(AccountModel account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));
            // map data naar xml
            var @event = new AccountEvent
            {
                EntityType = "Account",
                Name = account.Name,
                Email = account.Email,
                Source = SourceEnum.CRM,
                SourceEntityId = account.Id,
                VatNumber = null
        };

            var response = await _uUIDGateAway.GetGuid(account.Id, SourceEnum.CRM.ToString(), "Account");

            if (response == null)
            {
                var resp = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), "Account", account.Id.ToString(), 1);
                @event.EntityVersion = 1;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.CREATE;
            }
            else
            {
                var resp = await _uUIDGateAway.UpdateEntity(account.Id, SourceEnum.CRM.ToString(), "Account");
                @event.EntityVersion = resp.EntityVersion;
                @event.UUID_Nr = resp.Uuid.ToString();
                @event.Method = MethodEnum.UPDATE;
            }
            
            _accountPublisher.Publish(@event);
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _uUIDGateAway.GetGuid(id, SourceEnum.CRM.ToString(), "Account");
            var @event = new AccountEvent
            {
                UUID_Nr = response.Uuid.ToString(),
                Method = MethodEnum.DELETE,
            };

            _accountPublisher.Publish(@event);
            return Ok();
        }        
    }
}
