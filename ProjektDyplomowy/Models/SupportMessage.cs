using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class SupportMessage
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string Message { get; set; }

        public bool IsFromSupport { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
