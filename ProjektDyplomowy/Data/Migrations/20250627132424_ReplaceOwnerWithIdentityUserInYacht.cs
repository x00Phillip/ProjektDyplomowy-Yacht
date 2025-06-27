using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektDyplomowy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceOwnerWithIdentityUserInYacht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yacht_Owner_OwnerId",
                table: "Yacht");

            migrationBuilder.DropTable(
                name: "Owner");

            migrationBuilder.AddForeignKey(
                name: "FK_Yacht_AspNetUsers_OwnerId",
                table: "Yacht",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yacht_AspNetUsers_OwnerId",
                table: "Yacht");

            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Owner_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owner_UserId1",
                table: "Owner",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Yacht_Owner_OwnerId",
                table: "Yacht",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
