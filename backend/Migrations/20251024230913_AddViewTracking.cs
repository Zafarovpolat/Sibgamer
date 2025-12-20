using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddViewTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventViews_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventViews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NewsViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsViews_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsViews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EventViews_EventId_IpAddress_ViewDate",
                table: "EventViews",
                columns: new[] { "EventId", "IpAddress", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EventViews_EventId_UserId_ViewDate",
                table: "EventViews",
                columns: new[] { "EventId", "UserId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EventViews_UserId",
                table: "EventViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsViews_NewsId_IpAddress_ViewDate",
                table: "NewsViews",
                columns: new[] { "NewsId", "IpAddress", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_NewsViews_NewsId_UserId_ViewDate",
                table: "NewsViews",
                columns: new[] { "NewsId", "UserId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_NewsViews_UserId",
                table: "NewsViews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventViews");

            migrationBuilder.DropTable(
                name: "NewsViews");
        }
    }
}
