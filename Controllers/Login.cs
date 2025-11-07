using Lost_and_Found.DataAccess;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class Login : Controller
    {

        private readonly AppDBContext dbContext;
        public Login(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult LoginIndex()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("LoginIndex", "Login"); 
        }

        [HttpPost]
        public IActionResult SaveData(string email, string password)
        {
            var user = dbContext.SignUp.FirstOrDefault(s => s.email == email);

            if (user == null)
            {
                return Json(new { status = "error", message = "Email not registered"});
            }
            else if (user.password != password) 
            {
                return Json(new { status = "error", message = "Incorrect password" });
            }
            else
            {
                HttpContext.Session.SetString("UserEmail", user.email);
                HttpContext.Session.SetString("UserName", user.name);
                HttpContext.Session.SetInt32("UserId", user.id);
                return Json(new { status = "success", message = "Login successful" });
            }
        }
    }
}
