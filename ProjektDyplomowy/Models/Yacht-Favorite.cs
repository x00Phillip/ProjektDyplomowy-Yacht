using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class YachtFavorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public int YachtId { get; set; }
        public Yacht Yacht { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
