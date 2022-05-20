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
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountGateAway accountGateAway,
            AccountPublisher accountPublisher,
            IUUIDGateAway uUIDGateAway,
            ILogger<AccountController> logger)
        {

            _accountGateAway = accountGateAway;
            _accountPublisher = accountPublisher;
            _uUIDGateAway = uUIDGateAway;
            _logger = logger;
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
            if (account == null)
            {
                var date = DateTime.UtcNow;
                _logger.LogError("BadRequest on SessionController : {date}", date);
                return BadRequest();
            }
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

            var response = await _uUIDGateAway.GetGuid(account.Id, SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Account);

            if (response == null)
            {
                var resp = await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Account, account.Id.ToString(), 1);
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
            if (response != null)
            {
                var @event = new AccountEvent
                {
                    UUID_Nr = response.Uuid.ToString(),
                    Method = MethodEnum.DELETE,
                };
                
                _accountPublisher.Publish(@event);
                return Ok();
            }


            _logger.LogError("response UUIDMaster was null for: {id}", id);
            return Ok();
        }        
    }
}
