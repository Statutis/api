using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class MoveIsPublicToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Service");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Group",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Group",
                keyColumn: "GroupId",
                keyValue: new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                column: "IsPublic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Group",
                keyColumn: "GroupId",
                keyValue: new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                column: "IsPublic",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Group");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Service",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"),
                column: "IsPublic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"),
                column: "IsPublic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"),
                column: "IsPublic",
                value: true);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"),
                column: "IsPublic",
                value: true);
        }
    }
}
