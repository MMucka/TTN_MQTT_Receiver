using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTTCloud.Migrations
{
    public partial class AESsupportr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AesIv",
                table: "Devices",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AesKey",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AesIv",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AesKey",
                table: "Devices");
        }
    }
}
