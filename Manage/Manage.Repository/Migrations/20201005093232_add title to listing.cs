using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class addtitletolisting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Listings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Listings");
        }
    }
}
