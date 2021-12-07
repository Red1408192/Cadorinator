using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class commands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingCommand",
                columns: t => new
                {
                    PendingCommandId = t.Column<long>(type:"INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                    CommandId = t.Column<byte>(type: "TINYINT", nullable:false),
                    TimeStamp = t.Column<DateTime>(type: "DATETIME", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.PendingCommandId);
                });
            migrationBuilder.CreateIndex(
                name: "IX_PendingCommand_PendingCommandId",
                table: "PendingCommand",
                column: "PendingCommandId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("PendingCommand");
        }
    }
}
