using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class Owner
    {
        [Key]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public ICollection<Yacht> Yachts { get; set; }
    }
}
