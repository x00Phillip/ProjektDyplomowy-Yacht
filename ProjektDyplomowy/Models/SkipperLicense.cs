using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class SkipperLicense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        public string? RadioOperatorLicense { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool IsValid => ValidUntil >= DateTime.Now;
    }
}
