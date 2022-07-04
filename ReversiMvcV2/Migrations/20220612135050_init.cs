using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvcV2.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalGewonnen = table.Column<int>(type: "int", nullable: false),
                    AantalVerloren = table.Column<int>(type: "int", nullable: false),
                    AantalGelijk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spelers");
        }
    }
}
