using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Mark { get; set; }
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
        public List<RatingImage> Images { get; set; } = new();
    }
    public class RatingImage
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public int RatingId { get; set; }
        public Rating Rating { get; set; }
    }
}
