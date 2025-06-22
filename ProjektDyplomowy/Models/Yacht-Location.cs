using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class Yacht_Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Address { get; set; }
        public string? MapUrl { get; set; }

        public ICollection<Yacht> Yachts { get; set; }
    }
}
