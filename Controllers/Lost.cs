using Lost_and_Found.DataAccess;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;




namespace Lost_and_Found.Controllers
{
    public class Lost : Controller
    {
        private readonly AppDBContext dbContext;
      
        public Lost(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult LostIndex()
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
                        .Where(s => s.UserId != userId)
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

        public IActionResult FetchData1(int userId)
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
                        .Where(s => s.UserId != userId && s.Item == item)
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
        public IActionResult ReportUser(int ReporterId, string ReporterName, string Location, DateTime Date, string Description, int OwnerUserId, int LostItemId, IFormFile Photo, IFormFile OwnerImg)
        {
            string photoPath = null;

            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReportImage");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }

                photoPath = "/ReportImage/" + uniqueFileName;
            }

            DateTime parsedDate = Date;
            if (parsedDate < new DateTime(1753, 1, 1))
                parsedDate = DateTime.Now;

            var user = dbContext.SignUp.FirstOrDefault(s => s.id == OwnerUserId);

            //var setData = new ReportUserModal
            //{
            //    ReporterUserId = ReporterId,
            //    ReporterUserName = ReporterName,
            //    Location = Location,
            //    ReportDate = parsedDate,
            //    Message = Description,
            //    OwnerUserId = OwnerUserId,
            //    OwnerUserName = user?.name,
            //    LostItemId = LostItemId,
            //    PhotoProofPath = photoPath
            //};

            //dbContext.Reports.Add(setData);
            //dbContext.SaveChanges();

            try
            {
                var owner = dbContext.SignUp.FirstOrDefault(s => s.id == OwnerUserId);
                var reporter = dbContext.SignUp.FirstOrDefault(s => s.id == ReporterId);

                if (owner != null && !string.IsNullOrEmpty(owner.email))
                {
                    string toEmail = owner.email;
                    string subject = "Lost Item Report Notification";

                    var message = new MailMessage
                    {
                        From = new MailAddress("adityasharmaas813@gmail.com", $"{reporter.name} via Lost & Found"),
                        Subject = subject,
                        IsBodyHtml = true
                    };

                    message.To.Add(toEmail);

                    // 🔹 Generate unique content ID for inline image
                    string contentId = Guid.NewGuid().ToString();

                    string body = $@"
                    <h3>Hello {owner.name},</h3>
                    <p><strong>{reporter.name}</strong> has reported an item that might belong to you.</p>
                    <p><b>Item Description:</b> {Description}</p>
                    <p><b>Location:</b> {Location}</p>
                    <p><b>Date:</b> {parsedDate:dd-MM-yyyy}</p>
                    <p><b>Reporter Contact:</b> {reporter.phoneno}</p>";

                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        body += $@"<p><b>Reported Item Photo:</b></p>
                           <img src='cid:{contentId}' style='max-width:400px; border-radius:8px; border:1px solid #ddd;' />";
                    }

                    body += "<br><p>Best regards,<br>Lost & Found Team</p>";

                    message.Body = body;

                    // 🔹 Attach image inline
                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        string absolutePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photoPath.TrimStart('/'));
                        if (System.IO.File.Exists(absolutePhotoPath))
                        {
                            var inlinePhoto = new Attachment(absolutePhotoPath);
                            inlinePhoto.ContentId = contentId;
                            inlinePhoto.ContentDisposition.Inline = true;
                            inlinePhoto.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                            message.Attachments.Add(inlinePhoto);
                        }
                    }

                    var smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("adityasharmaas813@gmail.com", "ynun amvl smms yeus"),
                        EnableSsl = true
                    };

                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Email sending failed: " + ex.Message);
            }

            return Json(new { success = true, message = "Report submitted successfully" });
        }
    }
}
