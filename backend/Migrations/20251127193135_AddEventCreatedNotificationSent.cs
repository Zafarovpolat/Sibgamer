using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEventCreatedNotificationSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedNotificationSent",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "vip_tariff_id",
                table: "donation_transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vip_tariff_option_id",
                table: "donation_transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_vip_tariff_id",
                table: "donation_transactions",
                column: "vip_tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_vip_tariff_option_id",
                table: "donation_transactions",
                column: "vip_tariff_option_id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id",
                table: "donation_transactions",
                column: "vip_tariff_option_id",
                principalTable: "vip_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_vip_tariffs_vip_tariff_id",
                table: "donation_transactions",
                column: "vip_tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_vip_tariffs_vip_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_vip_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_vip_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropColumn(
                name: "IsCreatedNotificationSent",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "vip_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropColumn(
                name: "vip_tariff_option_id",
                table: "donation_transactions");
        }
    }
}
