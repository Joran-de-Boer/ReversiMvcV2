using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvcV2.Migrations.ApplicationDb
{
    public partial class ROLES3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "22e40406-8a9d-2d82-912c-5d6a640ee696", "0fb7dde9-f3e6-47d3-a161-adeddf30c7c8", "Speler", "SPELER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "47D6DC9B-9B5B-4F9C-A971-82A6BE3ADD8C", "fd9aca24-c5f8-49f9-af63-1481d5430dbb", "Mediator", "MEDIATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b421e928-0613-9ebd-a64c-f10b6a706e73", "1882b81e-fd48-4a6c-8266-b8bbc71d6d7b", "Beheerder", "BEHEERDER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22e40406-8a9d-2d82-912c-5d6a640ee696");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47D6DC9B-9B5B-4F9C-A971-82A6BE3ADD8C");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b421e928-0613-9ebd-a64c-f10b6a706e73");
        }
    }
}
