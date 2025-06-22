using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektDyplomowy.Models
{
    public class Rental
    {
        [Key] 
        public int Id { get; set; }

        [ForeignKey("Client"), Required]
        public string ClientId { get; set; }
        public IdentityUser? Client { get; set; }

        [ForeignKey("Yacht"), Required]
        public int YachtId { get; set; }
        public Yacht? Yacht { get; set; }

        [Required]
        public DateTime RentalStart { get; set; }

        [Required]
        [CustomValidation(typeof(Rental), nameof(ValidateRentalPeriod))]
        public DateTime RentalEnd { get; set; }

        [Required, Range(0, double.MaxValue), Precision(18, 2)]
        public decimal Price { get; set; }

        public bool IsActive => DateTime.Now >= RentalStart && DateTime.Now < RentalEnd;

        [Required]
        public bool WithSkipper { get; set; }
        // License info (only required if WithSkipper == false)
        public string? SailingLicenseNumber { get; set; }
        public string? RadioOperatorLicense { get; set; }
        public DateTime? LicenseValidUntil { get; set; }

        public static ValidationResult? ValidateRentalPeriod(DateTime rentalEnd, ValidationContext context)
        {
            var instance = context.ObjectInstance as Rental;
            if (instance != null && rentalEnd <= instance.RentalStart)
            {
                return new ValidationResult("End Date must be after Start Date.");
            }

            return ValidationResult.Success;
        }

        public decimal CalculatePrice()
        {
            if (Yacht == null)
                throw new InvalidOperationException("Yacht is not assigned.");

            return (RentalEnd - RentalStart).Days * Yacht.DailyRate;
        }
    }
}
