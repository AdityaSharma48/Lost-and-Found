using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lost_and_Found.Models
{
    public class ReportLostModal
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }  
        public string? Name { get; set; }
        public string? Item { get; set; }
        public string? Location { get; set; }
        public DateTime? DateLost { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? phone { get; set; }
        public bool? IsFound { get; set; }
    }
}

