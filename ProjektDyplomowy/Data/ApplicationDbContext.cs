using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Models;

namespace ProjektDyplomowy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjektDyplomowy.Models.Yacht> Yacht { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.Rental> Rental { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.Rating> Rating { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.Yacht_Location> Yacht_Location { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.Payment> Payment { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.SupportMessage> SupportMessage { get; set; } = default!;
        public DbSet<ProjektDyplomowy.Models.SkipperLicense> SkipperLicense { get; set; } = default!;
    }
}
