using System.ComponentModel.DataAnnotations;

namespace Lost_and_Found.Models
{
    public class SignModal
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? phoneno { get; set; }
        public string? address { get; set; }
        public string? PhotoPath { get; set; }

    }
}
