using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilmName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstProjectionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.FilmId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Film_FilmId",
                table: "Film",
                column: "FilmId",
                unique: true);

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ProviderId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderDomain = table.Column<string>(type: "VARCHAR(60)", nullable: false),
                    ProviderName = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    ProviderSource = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ProviderId);
                    table.ForeignKey("FK_provider_ProviderSource",
                        x => x.ProviderSource,
                        principalTable: "ProviderSource",
                        principalColumn: "ProviderSourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_ProviderId",
                table: "Provider",
                column: "ProviderId",
                unique: true);

            migrationBuilder.CreateTable(
                name: "ProviderSource",
                columns: table => new
                {
                    ProviderSourceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderSourceName = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSource", x => x.ProviderSourceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSource_ProviderSourceId",
                table: "ProviderSource",
                column: "ProviderSourceId",
                unique: true);

            migrationBuilder.CreateTable(
                name: "ProjectionsSchedule",
                columns: table => new
                {
                    ProjectionsScheduleId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilmId = table.Column<long>(type: "INTEGER", nullable: false),
                    ThreaterId = table.Column<string>(type: "INTEGER", nullable: false),
                    ProviderId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProjectionTimestamp = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionsSchedule", x => x.ProjectionsScheduleId);
                    table.ForeignKey("FK_ProjectionsSchedule_Film",
                        x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_ProjectionsSchedule_Provider",
                        x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectionsSchedule_ProjectionsScheduleId",
                table: "ProjectionsSchedule",
                column: "ProjectionsScheduleId",
                unique: true);

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    SampleId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SampleTimestamp = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ProjectionsScheduleId = table.Column<long>(type: "INTEGER", nullable: false),
                    BoughtSeats = table.Column<short>(type: "SMALLINT", nullable: false),
                    LockedSeats = table.Column<short>(type: "SMALLINT", nullable: false),
                    ReservedSeats = table.Column<short>(type: "SMALLINT", nullable: false),
                    QuarantinedSeats = table.Column<short>(type: "SMALLINT", nullable: false),
                    TotalSeats = table.Column<short>(type: "SMALLINT", nullable: false),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.SampleId);
                    table.ForeignKey("FK_Sample_ProjectionsSchedule",
                        x => x.ProjectionsScheduleId,
                        principalTable: "ProjectionsSchedule",
                        principalColumn: "ProjectionsScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sample_SampleId",
                table: "Sample",
                column: "SampleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "ProviderSource");

            migrationBuilder.DropTable(
                name: "ProjectionsSchedule");

            migrationBuilder.DropTable(
                name: "Sample");
        }
    }
}
