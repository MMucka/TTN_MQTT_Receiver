using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class FixDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gateways_Applications_ApplicationId",
                table: "Gateways");

            migrationBuilder.DropIndex(
                name: "IX_Gateways_ApplicationId",
                table: "Gateways");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Gateways");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ApplicationId",
                table: "Devices",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Applications_ApplicationId",
                table: "Devices",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Applications_ApplicationId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ApplicationId",
                table: "Devices");

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId",
                table: "Gateways",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gateways_ApplicationId",
                table: "Gateways",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gateways_Applications_ApplicationId",
                table: "Gateways",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
