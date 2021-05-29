using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Infra.Data.Migrations
{
    public partial class UpdateUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplacedByToken",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Revoked",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplacedByToken",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "RefreshToken");
        }
    }
}
