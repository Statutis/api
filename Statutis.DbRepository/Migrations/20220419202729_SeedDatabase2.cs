using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    public partial class SeedDatabase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "GroupId", "Description", "MainGroupId", "Name" },
                values: new object[,]
                {
                    { new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), "Groupe par défaut", null, "Défaut" },
                    { new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), "Groupe par défaut", null, "Statutis" }
                });

            migrationBuilder.InsertData(
                table: "Team",
                columns: new[] { "TeamId", "Color", "MainTeamId", "Name" },
                values: new object[,]
                {
                    { new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa"), "#34495e", null, "Default" },
                    { new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"), "#e74c3c", null, "Administrateur" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Email", "Avatar", "Password", "Roles", "Username" },
                values: new object[] { "contact@silvain.eu", null, "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$Wg1s/1X6O1f1ZQCQtAvkGaCMwteH+F2h6p6AGDM8om4", "ROLE_ADMIN", "admin" });

            migrationBuilder.InsertData(
                table: "GroupTeam",
                columns: new[] { "GroupsGroupId", "TeamsTeamId" },
                values: new object[,]
                {
                    { new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa") },
                    { new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42") },
                    { new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa") },
                    { new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42") }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "ServiceId", "Description", "GroupId", "Host", "Name", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"), "DNS A pour silvain.eu", new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), "silvain.eu", "DNS Silvain.eu", "DNS" },
                    { new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"), "Serveur backend de statutis", new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), "https://api.statutis.silvain.eu", "API", "Site Web" },
                    { new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"), "Serveur frontend de statutis", new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), "https://statutis.silvain.eu", "Frontend", "Site Web" },
                    { new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"), "Serveur de google", new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), "8.8.8.8", "Serveur de Google", "Serveur" }
                });

            migrationBuilder.InsertData(
                table: "Team",
                columns: new[] { "TeamId", "Color", "MainTeamId", "Name" },
                values: new object[] { new Guid("9de07aec-ce5d-43bc-a909-c648a3b4956a"), "#95a5a6", new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa"), "Sub Team" });

            migrationBuilder.InsertData(
                table: "TeamUser",
                columns: new[] { "TeamsTeamId", "UsersEmail" },
                values: new object[] { new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"), "contact@silvain.eu" });

            migrationBuilder.InsertData(
                table: "DnsService",
                columns: new[] { "ServiceId", "Result", "Type" },
                values: new object[] { new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"), "89.234.182.183", "A" });

            migrationBuilder.InsertData(
                table: "HttpService",
                columns: new[] { "ServiceId", "Code", "Port", "RedirectUrl" },
                values: new object[,]
                {
                    { new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"), 404, 443, null },
                    { new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"), 200, 443, null }
                });

            migrationBuilder.InsertData(
                table: "PingService",
                column: "ServiceId",
                value: new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DnsService",
                keyColumn: "ServiceId",
                keyValue: new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"));

            migrationBuilder.DeleteData(
                table: "GroupTeam",
                keyColumns: new[] { "GroupsGroupId", "TeamsTeamId" },
                keyValues: new object[] { new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa") });

            migrationBuilder.DeleteData(
                table: "GroupTeam",
                keyColumns: new[] { "GroupsGroupId", "TeamsTeamId" },
                keyValues: new object[] { new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"), new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42") });

            migrationBuilder.DeleteData(
                table: "GroupTeam",
                keyColumns: new[] { "GroupsGroupId", "TeamsTeamId" },
                keyValues: new object[] { new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa") });

            migrationBuilder.DeleteData(
                table: "GroupTeam",
                keyColumns: new[] { "GroupsGroupId", "TeamsTeamId" },
                keyValues: new object[] { new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"), new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42") });

            migrationBuilder.DeleteData(
                table: "HttpService",
                keyColumn: "ServiceId",
                keyValue: new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"));

            migrationBuilder.DeleteData(
                table: "HttpService",
                keyColumn: "ServiceId",
                keyValue: new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"));

            migrationBuilder.DeleteData(
                table: "PingService",
                keyColumn: "ServiceId",
                keyValue: new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"));

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "TeamId",
                keyValue: new Guid("9de07aec-ce5d-43bc-a909-c648a3b4956a"));

            migrationBuilder.DeleteData(
                table: "TeamUser",
                keyColumns: new[] { "TeamsTeamId", "UsersEmail" },
                keyValues: new object[] { new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"), "contact@silvain.eu" });

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"));

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"));

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"));

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ServiceId",
                keyValue: new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"));

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "TeamId",
                keyValue: new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa"));

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "TeamId",
                keyValue: new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Email",
                keyValue: "contact@silvain.eu");

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "GroupId",
                keyValue: new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"));

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "GroupId",
                keyValue: new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"));
        }
    }
}
