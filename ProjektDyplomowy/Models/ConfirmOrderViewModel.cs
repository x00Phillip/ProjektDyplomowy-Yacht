namespace ProjektDyplomowy.Models
{
    public class ConfirmOrderViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RentalDays => (EndDate - StartDate).Days;
        public decimal PricePerDay { get; set; }
        public decimal SkipperFeePerDay { get; set; } = 250.00m; // example default value
        public bool WithSkipper { get; set; }
        public decimal TotalPrice =>
            RentalDays * PricePerDay + (WithSkipper ? RentalDays * SkipperFeePerDay : 0);
        public int YachtId { get; set; }
        public string YachtBrand { get; set; }
        public string YachtModel { get; set; }

        // Optional: license info if no skipper
        public string? SailingLicenseNumber { get; set; }
        public string? RadioOperatorLicense { get; set; }
        public DateTime? LicenseValidUntil { get; set; }

        public bool ShowLicenseFields => !WithSkipper;
    }
}
