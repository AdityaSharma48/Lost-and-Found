using Lost_and_Found.DataAccess;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lost_and_Found.Controllers
{
    public class ReportLost : Controller
    {

        private readonly AppDBContext dbContext;
        public ReportLost(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult ReportLostIndex()
        {
            var userId = HttpContext?.Session?.GetInt32("UserId");
            var profile = dbContext.SignUp.FirstOrDefault(s => s.id == userId);
            ViewBag.Profile = profile;
            return View();
        }


      
        [HttpPost]
        public IActionResult SaveData(string name, string item, string location, string date, string description, int userId, IFormFile? photo, string phone)
        {
            string photoPath = null;

            // ✅ Handle file upload
            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                photoPath = "/uploads/" + uniqueFileName;
            }

            // ✅ Parse date safely
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                parsedDate = DateTime.Now;
            }

            // ✅ Ensure valid SQL datetime range
            if (parsedDate < new DateTime(1753, 1, 1))
                parsedDate = DateTime.Now;

            // ✅ Save record
            var SetData = new ReportLostModal
            {
                Name = name,
                Item = item,
                Location = location,
                DateLost = parsedDate,
                Description = description,
                PhotoPath = photoPath,
                UserId = userId,
                CreatedAt = DateTime.Now, // 🔥 Important line
                phone = phone,
                IsFound = false
            };

            dbContext.LostItems.Add(SetData);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Record saved successfully", photoPath });
        }
    }
}
