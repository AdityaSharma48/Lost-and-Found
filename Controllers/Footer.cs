using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class Footer : Controller
    {
        public IActionResult FooterIndex()
        {
            return View();
        }
    }
}
