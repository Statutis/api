using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MainGroupId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_Group_MainGroupId",
                        column: x => x.MainGroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Icon = table.Column<byte[]>(type: "bytea", nullable: true),
                    Color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceType", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    MainTeamId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Team_MainTeamId",
                        column: x => x.MainTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Avatar = table.Column<byte[]>(type: "bytea", nullable: true),
                    Roles = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => new { x.Name, x.GroupId });
                    table.ForeignKey(
                        name: "FK_Service_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Service_ServiceType_Name",
                        column: x => x.Name,
                        principalTable: "ServiceType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupTeam",
                columns: table => new
                {
                    GroupsGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamsTeamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTeam", x => new { x.GroupsGroupId, x.TeamsTeamId });
                    table.ForeignKey(
                        name: "FK_GroupTeam_Group_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTeam_Team_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    TeamsTeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersEmail = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUser", x => new { x.TeamsTeamId, x.UsersEmail });
                    table.ForeignKey(
                        name: "FK_TeamUser_Team_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUser_User_UsersEmail",
                        column: x => x.UsersEmail,
                        principalTable: "User",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DnsService",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnsService", x => new { x.Name, x.GroupId });
                    table.ForeignKey(
                        name: "FK_DnsService_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsService_Service_Name_GroupId",
                        columns: x => new { x.Name, x.GroupId },
                        principalTable: "Service",
                        principalColumns: new[] { "Name", "GroupId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsService_ServiceType_Name",
                        column: x => x.Name,
                        principalTable: "ServiceType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HttpService",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    RedirectUrl = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpService", x => new { x.Name, x.GroupId });
                    table.ForeignKey(
                        name: "FK_HttpService_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HttpService_Service_Name_GroupId",
                        columns: x => new { x.Name, x.GroupId },
                        principalTable: "Service",
                        principalColumns: new[] { "Name", "GroupId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HttpService_ServiceType_Name",
                        column: x => x.Name,
                        principalTable: "ServiceType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PingService",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingService", x => new { x.Name, x.GroupId });
                    table.ForeignKey(
                        name: "FK_PingService_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PingService_Service_Name_GroupId",
                        columns: x => new { x.Name, x.GroupId },
                        principalTable: "Service",
                        principalColumns: new[] { "Name", "GroupId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PingService_ServiceType_Name",
                        column: x => x.Name,
                        principalTable: "ServiceType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SshService",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Bash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IsSshKey = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SshService", x => new { x.Name, x.GroupId });
                    table.ForeignKey(
                        name: "FK_SshService_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SshService_Service_Name_GroupId",
                        columns: x => new { x.Name, x.GroupId },
                        principalTable: "Service",
                        principalColumns: new[] { "Name", "GroupId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SshService_ServiceType_Name",
                        column: x => x.Name,
                        principalTable: "ServiceType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DnsService_GroupId",
                table: "DnsService",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_MainGroupId",
                table: "Group",
                column: "MainGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name_MainGroupId",
                table: "Group",
                columns: new[] { "Name", "MainGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeam_TeamsTeamId",
                table: "GroupTeam",
                column: "TeamsTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_HttpService_GroupId",
                table: "HttpService",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PingService_GroupId",
                table: "PingService",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_GroupId",
                table: "Service",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SshService_GroupId",
                table: "SshService",
                column: "GroupId");

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
                name: "IX_TeamUser_UsersEmail",
                table: "TeamUser",
                column: "UsersEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DnsService");

            migrationBuilder.DropTable(
                name: "GroupTeam");

            migrationBuilder.DropTable(
                name: "HttpService");

            migrationBuilder.DropTable(
                name: "PingService");

            migrationBuilder.DropTable(
                name: "SshService");

            migrationBuilder.DropTable(
                name: "TeamUser");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "ServiceType");
        }
    }
}
