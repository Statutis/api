using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class AddServicePublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Service");
        }
    }
}
