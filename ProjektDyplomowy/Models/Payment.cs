using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
        [Required]
        public string PaymentGateway { get; set; } = "PayU";
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }
}
