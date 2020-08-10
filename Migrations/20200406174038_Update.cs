using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Applications_ApplicationId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Gateways_Messages_MessageId",
                table: "Gateways");

            migrationBuilder.DropIndex(
                name: "IX_Gateways_MessageId",
                table: "Gateways");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ApplicationId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AppID",
                table: "Devices");

            migrationBuilder.AddColumn<long>(
                name: "DeviceId",
                table: "Messages",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId",
                table: "Gateways",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Devices",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gateways_Applications_ApplicationId",
                table: "Gateways");

            migrationBuilder.DropIndex(
                name: "IX_Gateways_ApplicationId",
                table: "Gateways");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Gateways");

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Devices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "AppID",
                table: "Devices",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gateways_MessageId",
                table: "Gateways",
                column: "MessageId");

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gateways_Messages_MessageId",
                table: "Gateways",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
