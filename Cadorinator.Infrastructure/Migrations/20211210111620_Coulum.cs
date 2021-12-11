using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class Coulum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Provider",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_CityId",
                table: "Provider",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_City_CityId",
                table: "City",
                column: "CityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Provider_City_CityId",
                table: "Provider",
                column: "CityId",
                principalTable: "City",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provider_City_CityId",
                table: "Provider");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "IX_Provider_CityId",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Provider");
        }
    }
}
