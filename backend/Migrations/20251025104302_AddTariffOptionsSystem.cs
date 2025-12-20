using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTariffOptionsSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration_days",
                table: "admin_tariffs");

            migrationBuilder.DropColumn(
                name: "price",
                table: "admin_tariffs");

            migrationBuilder.AddColumn<int>(
                name: "tariff_option_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tariff_option_id",
                table: "donation_transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminTariffGroupId",
                table: "admin_tariffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "admin_tariff_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_tariff_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_admin_tariff_groups_Servers_server_id",
                        column: x => x.server_id,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "admin_tariff_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tariff_id = table.Column<int>(type: "int", nullable: false),
                    duration_days = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_tariff_options", x => x.id);
                    table.ForeignKey(
                        name: "FK_admin_tariff_options_admin_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "admin_tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_user_admin_privileges_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_tariff_option_id",
                table: "donation_transactions",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariffs_AdminTariffGroupId",
                table: "admin_tariffs",
                column: "AdminTariffGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariff_groups_server_id_is_active",
                table: "admin_tariff_groups",
                columns: new[] { "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariff_options_tariff_id_is_active",
                table: "admin_tariff_options",
                columns: new[] { "tariff_id", "is_active" });

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId",
                table: "admin_tariffs",
                column: "AdminTariffGroupId",
                principalTable: "admin_tariff_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId",
                table: "admin_tariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropTable(
                name: "admin_tariff_groups");

            migrationBuilder.DropTable(
                name: "admin_tariff_options");

            migrationBuilder.DropIndex(
                name: "IX_user_admin_privileges_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_admin_tariffs_AdminTariffGroupId",
                table: "admin_tariffs");

            migrationBuilder.DropColumn(
                name: "tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropColumn(
                name: "tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropColumn(
                name: "AdminTariffGroupId",
                table: "admin_tariffs");

            migrationBuilder.AddColumn<int>(
                name: "duration_days",
                table: "admin_tariffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "admin_tariffs",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
