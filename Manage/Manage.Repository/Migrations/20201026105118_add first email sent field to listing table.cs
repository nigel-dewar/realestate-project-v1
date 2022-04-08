using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class addfirstemailsentfieldtolistingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedCronJobId",
                table: "Listings");

            migrationBuilder.AddColumn<bool>(
                name: "FirstEmailSent",
                table: "Listings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstEmailSent",
                table: "Listings");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmedCronJobId",
                table: "Listings",
                nullable: true);
        }
    }
}
