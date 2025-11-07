using Lost_and_Found.DataAccess;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lost_and_Found.Controllers
{
    public class Profile : Controller
    {

        private readonly AppDBContext dbContext;
        public Profile(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult ProfileIndex()
        {
            var userId = HttpContext?.Session?.GetInt32("UserId");
            var profile = dbContext.SignUp.FirstOrDefault(s => s.id == userId);

            bool hasProfile = false;

            if (profile != null)
            {
                ViewBag.Profile = profile;

                // Check if at least one key profile field is filled
                if (!string.IsNullOrEmpty(profile.address) ||
                    !string.IsNullOrEmpty(profile.PhotoPath) ||
                    !string.IsNullOrEmpty(profile.phoneno))
                {
                    hasProfile = true;
                }
            }

            // ✅ Always assign the final result here
            ViewBag.HasProfile = hasProfile;

            return View();
        }



        [HttpPost]
        public IActionResult SaveData(string email, string phone, string address, int userId, IFormFile? photo)
        {
            string photoPath = null;

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPic");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                photoPath = "/UserPic/" + uniqueFileName;
            }

            var updateData = dbContext.SignUp.FirstOrDefault(s => s.id == userId && s.email == email);
            if (updateData != null)
            {
                updateData.address = address;
                updateData.PhotoPath = photoPath;
                updateData.phoneno = phone;

                dbContext.SaveChanges();
                return Json(new { success = true, message = "Data saved successfully" });
            }
            return Json(new { success = false, message = "User not found" });
        }


        [HttpPost]

        public IActionResult UpdateData(string name, string email, string phone, string address, int userId, IFormFile? photo)
        {
            string photoPath = null;

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPic");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                photoPath = "/UserPic/" + uniqueFileName;
            }

            var updateData = dbContext.SignUp.FirstOrDefault(s => s.id == userId);

            if (updateData == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            if (!string.Equals(updateData.email, email, StringComparison.OrdinalIgnoreCase))
            {
                bool emailExists = dbContext.SignUp.Any(s => s.email == email);
                if (emailExists)
                {
                    return Json(new { success = false, message = "This email is already in use. Please use another email." });
                }

                updateData.email = email;
            }

            updateData.address = address;
            updateData.phoneno = phone;
            if (photoPath != null)
                updateData.PhotoPath = photoPath;

            dbContext.SaveChanges();

            return Json(new { success = true, message = "Profile updated successfully!" });
        }
    }
}
