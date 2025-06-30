using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class Yacht
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = default!;

        [Required]
        public string Model { get; set; } = default!;

        [Required]
        [Range(2000, int.MaxValue, ErrorMessage = "Year must be greater than 2000")]
        public int Year { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Lenght mus be greater than 0.")]
        public int LengthInMeters { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "People Capacity must be greater than 0.")]
        public int MaxPersons { get; set; }

        [Required]
        [Precision(18, 2)]
        [Range(0.0, double.MaxValue, ErrorMessage = "Daily rate must be greater than 0.")]
        public decimal DailyRate { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of cabins must be greater than 0.")]
        public int NumberOfCabins { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of bathrooms must be greater than 0.")]
        public int NumberOfBathrooms { get; set; }

        [Required]
        public string OwnerId { get; set; } = default!;
        public IdentityUser Owner { get; set; } = default!;

        [Required]
        public int Yacht_LocationId { get; set; }
        public Yacht_Location Yacht_Location { get; set; } = default!;

        [Required]
        public YachtType Type { get; set; }

        public bool HasKitchen { get; set; }

        public bool HasAirConditioning { get; set; }

        public bool HasWiFi { get; set; }

        public bool IsAvailable => Rentals.All(r => r.RentalEnd < DateTime.Now);

        public string Image { get; set; } = "https://staging.simple.tn/wp-content/uploads/2024/07/default.png";

        public ICollection<Rental> Rentals { get; set; } = [];

    }
    public enum YachtType
    {
        Sailing,
        Motor,
        Catamaran,
        Luxury,
        Other
    }
}