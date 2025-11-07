using Lost_and_Found.DataAccess;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class SignUp : Controller
    {
        private readonly AppDBContext dbContext;
        public SignUp(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult SignIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveData(string name, string email, string password)
        {
            var result = "";
            var CheckData = dbContext.SignUp.FirstOrDefault(s => s.email == email);

            if (CheckData != null)
            {
                result = "User is already exist";
            }
            else
            {
                var setData = new SignModal();
                setData.name = name;
                setData.email = email;
                setData.password = password;

                dbContext.SignUp.Add(setData);
                dbContext.SaveChanges();

                result = "Created Successfully...";
            }
            return Json(result);
        }
    }
}
