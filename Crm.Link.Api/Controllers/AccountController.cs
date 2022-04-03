using Crm.Link.Api.GateAway;
using Crm.Link.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Crm.Link.Api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountGateAway accountGateAway;

        public AccountController(IAccountGateAway accountGateAway)
        {
            this.accountGateAway = accountGateAway;
        }

        [HttpGet]
        [Route(nameof(Test))]
        public async Task<IActionResult> Test()
        {
            var response = await accountGateAway.Create();

            var text = await response.Content.ReadAsStringAsync();
            return Ok(text);
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete()
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
