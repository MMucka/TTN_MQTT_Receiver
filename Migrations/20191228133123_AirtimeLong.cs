using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class AirtimeLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Airtime",
                table: "Message",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Airtime",
                table: "Message",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
