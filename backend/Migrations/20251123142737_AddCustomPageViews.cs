using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomPageViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomPageViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomPageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomPageViews_CustomPages_CustomPageId",
                        column: x => x.CustomPageId,
                        principalTable: "CustomPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomPageViews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageViews_CustomPageId_IpAddress_ViewDate",
                table: "CustomPageViews",
                columns: new[] { "CustomPageId", "IpAddress", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageViews_CustomPageId_UserId_ViewDate",
                table: "CustomPageViews",
                columns: new[] { "CustomPageId", "UserId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageViews_UserId",
                table: "CustomPageViews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomPageViews");
        }
    }
}
