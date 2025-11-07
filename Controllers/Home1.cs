using Lost_and_Found.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lost_and_Found.Controllers
{
    public class Home1 : Controller
    {
        private readonly AppDBContext dbContext;
        public Home1(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult HomeIndex()
        {
            var userId = HttpContext?.Session?.GetInt32("UserId");
            var profile = dbContext.SignUp.FirstOrDefault(s => s.id == userId);
            ViewBag.Profile = profile;
            return View();
        }
    }
}
