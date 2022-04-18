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
            return Ok();
        }

        [HttpDelete]
        [Route(nameof(Delete) + "{id}")]
        public async Task<IActionResult> Delete(int id)
        {
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
