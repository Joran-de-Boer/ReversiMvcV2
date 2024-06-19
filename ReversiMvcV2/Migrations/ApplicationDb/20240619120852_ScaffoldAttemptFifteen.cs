using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvcV2.Migrations.ApplicationDb
{
    public partial class ScaffoldAttemptFifteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22e40406-8a9d-2d82-912c-5d6a640ee696",
                column: "ConcurrencyStamp",
                value: "8fca4728-feea-463a-aac2-d95fd7a62fd3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47D6DC9B-9B5B-4F9C-A971-82A6BE3ADD8C",
                column: "ConcurrencyStamp",
                value: "39c795fe-8b6e-44ff-8c75-a2fc2340049a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b421e928-0613-9ebd-a64c-f10b6a706e73",
                column: "ConcurrencyStamp",
                value: "e99528f9-4f0d-4976-b61e-1bc7a5962933");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22e40406-8a9d-2d82-912c-5d6a640ee696",
                column: "ConcurrencyStamp",
                value: "0fb7dde9-f3e6-47d3-a161-adeddf30c7c8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47D6DC9B-9B5B-4F9C-A971-82A6BE3ADD8C",
                column: "ConcurrencyStamp",
                value: "fd9aca24-c5f8-49f9-af63-1481d5430dbb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b421e928-0613-9ebd-a64c-f10b6a706e73",
                column: "ConcurrencyStamp",
                value: "1882b81e-fd48-4a6c-8266-b8bbc71d6d7b");
        }
    }
}
