using Lost_and_Found.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class ReportFound : Controller
    {
        private readonly AppDBContext dbContext;
        public ReportFound(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult ReportFoundIndex()
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
                        .Where(s => s.UserId == userId)
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
                        .Where(s => s.UserId == userId && s.Item == item)
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

        public IActionResult UpdateStat(int ownerUserId, int lostItemId, int status)
        {
            var setData = dbContext.LostItems.FirstOrDefault(s => s.Id == lostItemId && s.UserId == ownerUserId);

            if (setData != null)
            {
                setData.IsFound = (status == 1);  
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Status updated successfully" });
            }

            return Json(new { success = false, message = "Item not found" });
        }
    }
}
