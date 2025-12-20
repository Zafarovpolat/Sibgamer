using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddVipSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions");

            migrationBuilder.CreateTable(
                name: "vip_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_id = table.Column<int>(type: "int", nullable: false),
                    host = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    port = table.Column<int>(type: "int", nullable: false),
                    database = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_configured = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vip_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_vip_settings_Servers_server_id",
                        column: x => x.server_id,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vip_tariffs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    server_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    group_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vip_tariffs", x => x.id);
                    table.ForeignKey(
                        name: "FK_vip_tariffs_Servers_server_id",
                        column: x => x.server_id,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vip_tariff_options",
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
                    table.PrimaryKey("PK_vip_tariff_options", x => x.id);
                    table.ForeignKey(
                        name: "FK_vip_tariff_options_vip_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "vip_tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_vip_privileges",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    steam_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    server_id = table.Column<int>(type: "int", nullable: false),
                    tariff_id = table.Column<int>(type: "int", nullable: false),
                    tariff_option_id = table.Column<int>(type: "int", nullable: true),
                    group_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    starts_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    transaction_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_vip_privileges", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_vip_privileges_Servers_server_id",
                        column: x => x.server_id,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_vip_privileges_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_vip_privileges_donation_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "donation_transactions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_vip_privileges_vip_tariff_options_tariff_option_id",
                        column: x => x.tariff_option_id,
                        principalTable: "vip_tariff_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_vip_privileges_vip_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "vip_tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions",
                column: "privilege_id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions",
                column: "privilege_id",
                principalTable: "user_admin_privileges",
                principalColumn: "id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_expires_at",
                table: "user_vip_privileges",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_server_id",
                table: "user_vip_privileges",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_steam_id_server_id_is_active",
                table: "user_vip_privileges",
                columns: new[] { "steam_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_tariff_id",
                table: "user_vip_privileges",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_tariff_option_id",
                table: "user_vip_privileges",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_transaction_id",
                table: "user_vip_privileges",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_user_id_server_id_is_active",
                table: "user_vip_privileges",
                columns: new[] { "user_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_vip_settings_server_id",
                table: "vip_settings",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_vip_tariff_options_tariff_id_is_active",
                table: "vip_tariff_options",
                columns: new[] { "tariff_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_vip_tariffs_server_id_is_active",
                table: "vip_tariffs",
                columns: new[] { "server_id", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_vip_privileges");

            migrationBuilder.DropTable(
                name: "vip_settings");

            migrationBuilder.DropTable(
                name: "vip_tariff_options");

            migrationBuilder.DropTable(
                name: "vip_tariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions",
                column: "privilege_id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions",
                column: "privilege_id",
                principalTable: "user_admin_privileges",
                principalColumn: "id");
        }
    }
}
