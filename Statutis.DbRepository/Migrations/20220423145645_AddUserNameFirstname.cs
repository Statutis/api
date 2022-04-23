using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class AddUserNameFirstname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Email",
                keyValue: "contact@silvain.eu",
                columns: new[] { "Firstname", "Name" },
                values: new object[] { "Super", "Administrateur" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");
        }
    }
}
