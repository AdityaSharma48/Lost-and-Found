using Lost_and_Found.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lost_and_Found.Models
{
    public class ReportUserModal
    {
        [Key]
        public int ReportId { get; set; }
        public int LostItemId { get; set; }
        public int ReporterUserId { get; set; }
        public string? ReporterUserName { get; set; }
        public int OwnerUserId { get; set; }
        public string? OwnerUserName { get; set; }
        public string? Location { get ; set; }
        public string? Message { get; set; }
        public DateTime ReportDate { get; set; }
        public string? Status { get; set; }
        public string? PhotoProofPath { get; set; }  
    }
}
