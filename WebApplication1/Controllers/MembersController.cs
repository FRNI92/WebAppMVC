using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult Members()
        {
            return View();
        }
    }
}
