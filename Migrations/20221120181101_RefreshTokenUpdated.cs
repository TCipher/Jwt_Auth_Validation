using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleToDoApi.Migrations
{
    public partial class RefreshTokenUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "RefreshTokens");
        }
    }
}
