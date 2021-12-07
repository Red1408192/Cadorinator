using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class commandsParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "Parameters", table: "PendingCommand", type: "VARCHAR(200)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Parameters", table: "PendingCommand");
        }
    }
}
