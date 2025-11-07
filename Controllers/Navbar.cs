using Lost_and_Found.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lost_and_Found.Controllers
{
    public class Navbar : Controller
    {
        public IActionResult NavbarIndex()
        {
            return View();
        }
    }
}
