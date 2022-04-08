using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class addfieldtolive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublishedLive",
                table: "Listings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublishedLive",
                table: "Listings");
        }
    }
}
