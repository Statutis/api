using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class RemoveHttpServicePort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "HttpService");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "HttpService",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "HttpService",
                keyColumn: "ServiceId",
                keyValue: new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"),
                column: "Port",
                value: 443);

            migrationBuilder.UpdateData(
                table: "HttpService",
                keyColumn: "ServiceId",
                keyValue: new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"),
                column: "Port",
                value: 443);
        }
    }
}
