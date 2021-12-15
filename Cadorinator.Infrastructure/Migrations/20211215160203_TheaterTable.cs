using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class TheaterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Theater",
                columns: table => new
                {
                    TheaterId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderId = table.Column<long>(type: "INTEGER", nullable: false),
                    TheaterName = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theater", x => x.TheaterId);
                    table.ForeignKey(
                        name: "FK_Theater_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Theater_ProviderId",
                table: "Theater",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Theater_TheaterId",
                table: "Theater",
                column: "TheaterId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Theater");
        }
    }
}
