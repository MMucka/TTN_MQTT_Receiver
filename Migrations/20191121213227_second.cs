using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payload_raw",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "DevID",
                table: "Messages",
                newName: "DevId");

            migrationBuilder.AddColumn<string>(
                name: "PayloadRaw",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayloadRaw",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "DevId",
                table: "Messages",
                newName: "DevID");

            migrationBuilder.AddColumn<string>(
                name: "Payload_raw",
                table: "Messages",
                type: "text",
                nullable: true);
        }
    }
}
