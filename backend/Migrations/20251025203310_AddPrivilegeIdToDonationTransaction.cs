using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivilegeIdToDonationTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "privilege_id",
                table: "donation_transactions",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropColumn(
                name: "privilege_id",
                table: "donation_transactions");
        }
    }
}
