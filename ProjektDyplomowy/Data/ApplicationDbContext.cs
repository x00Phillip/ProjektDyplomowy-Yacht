using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Models;
using static System.Net.WebRequestMethods;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Yacht>()
                .HasOne(y => y.Owner)
                .WithMany()
                .HasForeignKey(y => y.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<YachtFavorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            var testUserId = "owner-test-id";
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = testUserId,
                UserName = "owner@example.com",
                NormalizedUserName = "OWNER@EXAMPLE.COM",
                Email = "owner@example.com",
                NormalizedEmail = "OWNER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null!, "Test123!"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
            });
            modelBuilder.Entity<Yacht_Location>().HasData(
                new Yacht_Location { Id = 1, Name = "Gdynia Marina", Address = "al. Jana Pawła II 13A", MapUrl = "https://g.co/kgs/LdBAsxD" },
                new Yacht_Location { Id = 2, Name = "Port Hercules, Monaco", Address = "6 Quai Antoine 1er", MapUrl = "https://g.co/kgs/MxEKAUo" },
                new Yacht_Location { Id = 3, Name = "Split, Croatia", Address= "21000, Bačvice, Split", MapUrl = "https://g.co/kgs/vkF3t5g" }
            );
            modelBuilder.Entity<Yacht>().HasData(
                new Yacht
                {
                    Id = 1,
                    Brand = "Beneteau",
                    Model = "Oceanis 40",
                    Year = 2015,
                    LengthInMeters = 12,
                    MaxPersons = 8,
                    DailyRate = 1200m,
                    NumberOfCabins = 3,
                    NumberOfBathrooms = 2,
                    OwnerId = testUserId,
                    Yacht_LocationId = 1,
                    Type = YachtType.Sailing,
                    HasKitchen = true,
                    HasAirConditioning = false,
                    HasWiFi = true,
                    Image = "/images/oceanis40.jpg"
                },
                new Yacht
                {
                    Id = 2,
                    Brand = "Sunseeker",
                    Model = "Manhattan 52",
                    Year = 2020,
                    LengthInMeters = 16,
                    MaxPersons = 10,
                    DailyRate = 4500m,
                    NumberOfCabins = 4,
                    NumberOfBathrooms = 3,
                    OwnerId = testUserId,
                    Yacht_LocationId = 3,
                    Type = YachtType.Luxury,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/sunseeker.jpeg"
                },
                new Yacht
                {
                    Id = 3,
                    Brand = "Lagoon",
                    Model = "450F",
                    Year = 2018,
                    LengthInMeters = 14,
                    MaxPersons = 12,
                    DailyRate = 2800m,
                    NumberOfCabins = 4,
                    NumberOfBathrooms = 4,
                    OwnerId = testUserId,
                    Yacht_LocationId = 2,
                    Type = YachtType.Catamaran,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/Lagoon450F.jpg"
                },
                new Yacht
                {
                    Id = 4,
                    Brand = "Jeanneau",
                    Model = "Sun Odyssey 439",
                    Year = 2014,
                    LengthInMeters = 13,
                    MaxPersons = 9,
                    DailyRate = 1100m,
                    NumberOfCabins = 3,
                    NumberOfBathrooms = 2,
                    OwnerId = testUserId,
                    Yacht_LocationId = 1,
                    Type = YachtType.Sailing,
                    HasKitchen = true,
                    HasAirConditioning = false,
                    HasWiFi = false,
                    Image = "/images/SunOdyssey439.jpg"
                },
                new Yacht
                {
                    Id = 5,
                    Brand = "Azimut",
                    Model = "50 Flybridge",
                    Year = 2019,
                    LengthInMeters = 15,
                    MaxPersons = 12,
                    DailyRate = 5000m,
                    NumberOfCabins = 4,
                    NumberOfBathrooms = 3,
                    OwnerId = testUserId,
                    Yacht_LocationId = 3,
                    Type = YachtType.Luxury,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/Azumit50.webp"
                },
                new Yacht
                {
                    Id = 6,
                    Brand = "Fairline",
                    Model = "Phantom 48",
                    Year = 2013,
                    LengthInMeters = 15,
                    MaxPersons = 10,
                    DailyRate = 3800m,
                    NumberOfCabins = 3,
                    NumberOfBathrooms = 2,
                    OwnerId = testUserId,
                    Yacht_LocationId = 1,
                    Type = YachtType.Motor,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/FairLinePhantom48.jpg"
                },
                new Yacht
                {
                    Id = 7,
                    Brand = "Bavaria",
                    Model = "Cruiser 46",
                    Year = 2017,
                    LengthInMeters = 14,
                    MaxPersons = 10,
                    DailyRate = 1500m,
                    NumberOfCabins = 4,
                    NumberOfBathrooms = 3,
                    OwnerId = testUserId,
                    Yacht_LocationId = 1,
                    Type = YachtType.Sailing,
                    HasKitchen = true,
                    HasAirConditioning = false,
                    HasWiFi = true,
                    Image = "/images/BavariaCruiser46.jpg"
                },
                new Yacht
                {
                    Id = 8,
                    Brand = "Fountaine Pajot",
                    Model = "Saba 50",
                    Year = 2021,
                    LengthInMeters = 15,
                    MaxPersons = 14,
                    DailyRate = 6000m,
                    NumberOfCabins = 6,
                    NumberOfBathrooms = 5,
                    OwnerId = testUserId,
                    Yacht_LocationId = 2,
                    Type = YachtType.Catamaran,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/FountainePajotSaba50.jpg"
                },
                new Yacht
                {
                    Id = 9,
                    Brand = "Sea Ray",
                    Model = "SLX 400",
                    Year = 2022,
                    LengthInMeters = 12,
                    MaxPersons = 8,
                    DailyRate = 3500m,
                    NumberOfCabins = 2,
                    NumberOfBathrooms = 1,
                    OwnerId = testUserId,
                    Yacht_LocationId = 2,
                    Type = YachtType.Motor,
                    HasKitchen = false,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/SeeRay.jfif"
                },
                new Yacht
                {
                    Id = 10,
                    Brand = "Princess",
                    Model = "F55",
                    Year = 2020,
                    LengthInMeters = 17,
                    MaxPersons = 12,
                    DailyRate = 7000m,
                    NumberOfCabins = 5,
                    NumberOfBathrooms = 4,
                    OwnerId = testUserId,
                    Yacht_LocationId = 3,
                    Type = YachtType.Luxury,
                    HasKitchen = true,
                    HasAirConditioning = true,
                    HasWiFi = true,
                    Image = "/images/PrincessF55.webp"
                }
            );
        }
    }
}
