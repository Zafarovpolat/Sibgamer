using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSourceBansForMultiServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "sourcebans_settings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "server_id",
                table: "sourcebans_settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sourcebans_settings_server_id",
                table: "sourcebans_settings",
                column: "server_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sourcebans_settings_Servers_server_id",
                table: "sourcebans_settings",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sourcebans_settings_Servers_server_id",
                table: "sourcebans_settings");

            migrationBuilder.DropIndex(
                name: "IX_sourcebans_settings_server_id",
                table: "sourcebans_settings");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "sourcebans_settings");

            migrationBuilder.DropColumn(
                name: "server_id",
                table: "sourcebans_settings");
        }
    }
}
