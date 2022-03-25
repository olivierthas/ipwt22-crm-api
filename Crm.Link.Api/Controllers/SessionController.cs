using Microsoft.AspNetCore.Mvc;

namespace Crm.Link.Api.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
