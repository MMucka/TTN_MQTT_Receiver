using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class AddApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppId",
                table: "Applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppKey",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AppKey",
                table: "Applications");
        }
    }
}
