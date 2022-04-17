using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class SeedServiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ServiceType",
                columns: new[] { "Name", "Color", "Icon" },
                values: new object[,]
                {
                    { "DNS", null, null },
                    { "Serveur", null, null },
                    { "Site Web", null, null },
                    { "SMTP", null, null },
                    { "SSH", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceType",
                keyColumn: "Name",
                keyValue: "DNS");

            migrationBuilder.DeleteData(
                table: "ServiceType",
                keyColumn: "Name",
                keyValue: "Serveur");

            migrationBuilder.DeleteData(
                table: "ServiceType",
                keyColumn: "Name",
                keyValue: "Site Web");

            migrationBuilder.DeleteData(
                table: "ServiceType",
                keyColumn: "Name",
                keyValue: "SMTP");

            migrationBuilder.DeleteData(
                table: "ServiceType",
                keyColumn: "Name",
                keyValue: "SSH");
        }
    }
}
