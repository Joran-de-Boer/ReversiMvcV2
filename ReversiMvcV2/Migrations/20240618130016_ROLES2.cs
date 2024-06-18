using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvcV2.Migrations
{
    public partial class ROLES2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Spelers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Spelers");
        }
    }
}
