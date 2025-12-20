using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtToUserAdminPrivileges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_donation_transactions_transaction_id",
                table: "user_admin_privileges");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "user_admin_privileges",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_donation_transactions_transaction_id",
                table: "user_admin_privileges",
                column: "transaction_id",
                principalTable: "donation_transactions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_donation_transactions_transaction_id",
                table: "user_admin_privileges");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "user_admin_privileges");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_donation_transactions_transaction_id",
                table: "user_admin_privileges",
                column: "transaction_id",
                principalTable: "donation_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
