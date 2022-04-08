using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class addprocessoraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessorAction",
                table: "Listings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessorAction",
                table: "Listings");
        }
    }
}
