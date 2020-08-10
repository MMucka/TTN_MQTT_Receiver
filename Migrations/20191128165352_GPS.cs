using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class GPS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayloadRaw",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayloadRaw",
                table: "Messages",
                type: "text",
                nullable: true);
        }
    }
}
