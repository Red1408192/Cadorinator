using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class SourceEndpoint_To_scehdule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("SourceEndpoint", "ProjectionsSchedule", type: "VARCHAR(100)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("SourceEndpoint", "ProjectionsSchedule");
        }
    }
}
