using Microsoft.AspNetCore.Mvc;

namespace MedLinkDashboard.Controllers
{
    public class UtilityController : Controller
    {
        [HttpGet("/ComingSoon")]
        public IActionResult ComingSoon()
        {
            return View("~/Views/Shared/ComingSoon.cshtml");
        }
    }
}
