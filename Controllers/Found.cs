using Lost_and_Found.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class Found : Controller
    {
        private readonly AppDBContext dbContext;
        public Found(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult FoundIndex()
        {
            var userId = HttpContext?.Session?.GetInt32("UserId");
            var profile = dbContext.SignUp.FirstOrDefault(s => s.id == userId);
            ViewBag.Profile = profile;
            return View();
        }


        [HttpPost]
        public IActionResult FetchData(int userId)
        {
            var items = dbContext.LostItems
                        .Where(s => s.UserId == userId && s.IsFound == true)
                        .Select(s => new
                        {
                            s.Id,
                            s.UserId,
                            s.Item,
                            s.Name,
                            s.Location,
                            s.Description,
                            PhotoPath = s.PhotoPath,
                            s.phone,
                            s.DateLost
                        })
                        .ToList();
            return Json(items);
        }


        [HttpPost]
        public IActionResult Search(int userId, string item)
        {
            var items = dbContext.LostItems
                        .Where(s => s.UserId == userId && s.Item == item && s.IsFound == true)
                        .Select(s => new
                        {
                            s.Id,
                            s.UserId,
                            s.Item,
                            s.Name,
                            s.Location,
                            s.Description,
                            PhotoPath = s.PhotoPath,
                            s.phone,
                            s.DateLost
                        })
                        .ToList();
            return Json(items);
        }
    }
}
