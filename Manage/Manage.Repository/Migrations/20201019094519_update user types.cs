using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class updateusertypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTypes_Users_UserProfileId",
                table: "UserTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserTypes_UserProfileId",
                table: "UserTypes");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserTypes");

            migrationBuilder.CreateTable(
                name: "UserProfileTypes",
                columns: table => new
                {
                    UserProfileId = table.Column<string>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileTypes", x => new { x.UserProfileId, x.UserTypeId });
                    table.ForeignKey(
                        name: "FK_UserProfileTypes_Users_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileTypes_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileTypes_UserTypeId",
                table: "UserProfileTypes",
                column: "UserTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfileTypes");

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "UserTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes_UserProfileId",
                table: "UserTypes",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypes_Users_UserProfileId",
                table: "UserTypes",
                column: "UserProfileId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
