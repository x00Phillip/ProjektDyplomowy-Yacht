using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjektDyplomowy.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "owner-test-id", 0, "c2a6a6f4-dddd-4b6c-9813-83382e07dd0f", "owner@example.com", true, false, null, "OWNER@EXAMPLE.COM", "OWNER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEB3TXHivhAku6cgFenlvv67t7ugRESFb3PESnfGA79SvL/BhvHx13qcP+3MBHUZVKA==", null, false, "f5599408-94aa-4d5d-81f8-e170affdb202", false, "owner@example.com" });

            migrationBuilder.InsertData(
                table: "Yacht_Location",
                columns: new[] { "Id", "Address", "MapUrl", "Name" },
                values: new object[,]
                {
                    { 1, "al. Jana Pawła II 13A", "https://g.co/kgs/LdBAsxD", "Gdynia Marina" },
                    { 2, "6 Quai Antoine 1er", "https://g.co/kgs/MxEKAUo", "Port Hercules, Monaco" },
                    { 3, "21000, Bačvice, Split", "https://g.co/kgs/vkF3t5g", "Split, Croatia" }
                });

            migrationBuilder.InsertData(
                table: "Yacht",
                columns: new[] { "Id", "Brand", "DailyRate", "HasAirConditioning", "HasKitchen", "HasWiFi", "Image", "LengthInMeters", "MaxPersons", "Model", "NumberOfBathrooms", "NumberOfCabins", "OwnerId", "Type", "Yacht_LocationId", "Year" },
                values: new object[,]
                {
                    { 1, "Beneteau", 1200m, false, true, true, "/images/oceanis40.jpg", 12, 8, "Oceanis 40", 2, 3, "owner-test-id", 0, 1, 2015 },
                    { 2, "Sunseeker", 4500m, true, true, true, "/images/sunseeker.jpeg", 16, 10, "Manhattan 52", 3, 4, "owner-test-id", 3, 3, 2020 },
                    { 3, "Lagoon", 2800m, true, true, true, "/images/Lagoon450F.jpg", 14, 12, "450F", 4, 4, "owner-test-id", 2, 2, 2018 },
                    { 4, "Jeanneau", 1100m, false, true, false, "/images/SunOdyssey439.jpg", 13, 9, "Sun Odyssey 439", 2, 3, "owner-test-id", 0, 1, 2014 },
                    { 5, "Azimut", 5000m, true, true, true, "/images/Azumit50.webp", 15, 12, "50 Flybridge", 3, 4, "owner-test-id", 3, 3, 2019 },
                    { 6, "Fairline", 3800m, true, true, true, "/images/FairLinePhantom48.jpg", 15, 10, "Phantom 48", 2, 3, "owner-test-id", 1, 1, 2013 },
                    { 7, "Bavaria", 1500m, false, true, true, "/images/BavariaCruiser46.jpg", 14, 10, "Cruiser 46", 3, 4, "owner-test-id", 0, 1, 2017 },
                    { 8, "Fountaine Pajot", 6000m, true, true, true, "/images/FountainePajotSaba50.jpg", 15, 14, "Saba 50", 5, 6, "owner-test-id", 2, 2, 2021 },
                    { 9, "Sea Ray", 3500m, true, false, true, "/images/SeeRay.jfif", 12, 8, "SLX 400", 1, 2, "owner-test-id", 1, 2, 2022 },
                    { 10, "Princess", 7000m, true, true, true, "/images/PrincessF55.webp", 17, 12, "F55", 4, 5, "owner-test-id", 3, 3, 2020 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Yacht",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Yacht_Location",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "owner-test-id");

            migrationBuilder.DeleteData(
                table: "Yacht_Location",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Yacht_Location",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
