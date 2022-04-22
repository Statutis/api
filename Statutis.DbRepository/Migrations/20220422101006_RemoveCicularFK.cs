using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class RemoveCicularFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Group_MainGroupId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Team_MainTeamId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MainTeamId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_Name_MainTeamId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Group_MainGroupId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_Name_MainGroupId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "MainTeamId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MainGroupId",
                table: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Team",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                table: "Group",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Team_Name",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Group_Name",
                table: "Group");

            migrationBuilder.AddColumn<Guid>(
                name: "MainTeamId",
                table: "Team",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MainGroupId",
                table: "Group",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Team",
                keyColumn: "TeamId",
                keyValue: new Guid("9de07aec-ce5d-43bc-a909-c648a3b4956a"),
                column: "MainTeamId",
                value: new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa"));

            migrationBuilder.CreateIndex(
                name: "IX_Team_MainTeamId",
                table: "Team",
                column: "MainTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name_MainTeamId",
                table: "Team",
                columns: new[] { "Name", "MainTeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_MainGroupId",
                table: "Group",
                column: "MainGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name_MainGroupId",
                table: "Group",
                columns: new[] { "Name", "MainGroupId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Group_MainGroupId",
                table: "Group",
                column: "MainGroupId",
                principalTable: "Group",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Team_MainTeamId",
                table: "Team",
                column: "MainTeamId",
                principalTable: "Team",
                principalColumn: "TeamId");
        }
    }
}
