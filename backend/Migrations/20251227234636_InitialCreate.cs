using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "donation_packages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    suggested_amounts = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_donation_packages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nav_sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_external = table.Column<bool>(type: "boolean", nullable: false),
                    open_in_new_tab = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_nav_sections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    rcon_password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    map_name = table.Column<string>(type: "text", nullable: false),
                    current_players = table.Column<int>(type: "integer", nullable: false),
                    max_players = table.Column<int>(type: "integer", nullable: false),
                    is_online = table.Column<bool>(type: "boolean", nullable: false),
                    last_checked = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_servers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    data_type = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_site_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "slider_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    buttons = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_slider_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "smtp_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    host = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    enable_ssl = table.Column<bool>(type: "boolean", nullable: false),
                    from_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    from_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_configured = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_smtp_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    steam_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    steam_profile_url = table.Column<string>(type: "text", nullable: true),
                    last_ip = table.Column<string>(type: "text", nullable: true),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    blocked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    block_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "yoomoney_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wallet_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    secret_key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_configured = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_yoomoney_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "admin_tariff_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_admin_tariff_groups", x => x.id);
                    table.ForeignKey(
                        name: "f_k_admin_tariff_groups__servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sourcebans_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    host = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    database = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_configured = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sourcebans_settings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_sourcebans_settings_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vip_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    host = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    database = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_configured = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vip_settings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vip_settings_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vip_tariffs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    group_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vip_tariffs", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vip_tariffs_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocked_ips",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    blocked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    blocked_by_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_blocked_ips", x => x.id);
                    table.ForeignKey(
                        name: "f_k_blocked_ips__users_blocked_by_user_id",
                        column: x => x.blocked_by_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "custom_pages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cover_image = table.Column<string>(type: "text", nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    view_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_custom_pages", x => x.id);
                    table.ForeignKey(
                        name: "f_k_custom_pages__users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cover_image = table.Column<string>(type: "text", nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    view_count = table.Column<int>(type: "integer", nullable: false),
                    like_count = table.Column<int>(type: "integer", nullable: false),
                    comment_count = table.Column<int>(type: "integer", nullable: false),
                    is_start_notification_sent = table.Column<bool>(type: "boolean", nullable: false),
                    is_end_notification_sent = table.Column<bool>(type: "boolean", nullable: false),
                    is_created_notification_sent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_events", x => x.id);
                    table.ForeignKey(
                        name: "f_k_events__users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cover_image = table.Column<string>(type: "text", nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    view_count = table.Column<int>(type: "integer", nullable: false),
                    like_count = table.Column<int>(type: "integer", nullable: false),
                    comment_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_news", x => x.id);
                    table.ForeignKey(
                        name: "f_k_news__users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    related_entity_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notifications", x => x.id);
                    table.ForeignKey(
                        name: "f_k_notifications__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_password_reset_tokens", x => x.id);
                    table.ForeignKey(
                        name: "f_k_password_reset_tokens__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "telegram_subscribers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    subscribed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_telegram_subscribers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_telegram_subscribers__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "vip_applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    steam_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    hours_per_week = table.Column<int>(type: "integer", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    admin_id = table.Column<int>(type: "integer", nullable: true),
                    admin_comment = table.Column<string>(type: "text", nullable: true),
                    vip_group = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    duration_days = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vip_applications", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vip_applications_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_vip_applications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admin_tariffs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    flags = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    group_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    immunity = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    admin_tariff_group_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_admin_tariffs", x => x.id);
                    table.ForeignKey(
                        name: "f_k_admin_tariffs__servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_admin_tariffs_admin_tariff_groups_admin_tariff_group_id",
                        column: x => x.admin_tariff_group_id,
                        principalTable: "admin_tariff_groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "vip_tariff_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariff_id = table.Column<int>(type: "integer", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vip_tariff_options", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vip_tariff_options_vip_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "vip_tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "custom_page_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    custom_page_id = table.Column<int>(type: "integer", nullable: false),
                    media_url = table.Column<string>(type: "text", nullable: false),
                    media_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_custom_page_media", x => x.id);
                    table.ForeignKey(
                        name: "f_k_custom_page_media_custom_pages_custom_page_id",
                        column: x => x.custom_page_id,
                        principalTable: "custom_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "custom_page_views",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    custom_page_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    view_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_custom_page_views", x => x.id);
                    table.ForeignKey(
                        name: "f_k_custom_page_views__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_custom_page_views_custom_pages_custom_page_id",
                        column: x => x.custom_page_id,
                        principalTable: "custom_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nav_section_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    custom_page_id = table.Column<int>(type: "integer", nullable: true),
                    open_in_new_tab = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_nav_section_items", x => x.id);
                    table.ForeignKey(
                        name: "f_k_nav_section_items_custom_pages_custom_page_id",
                        column: x => x.custom_page_id,
                        principalTable: "custom_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "f_k_nav_section_items_nav_sections_section_id",
                        column: x => x.section_id,
                        principalTable: "nav_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    parent_comment_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_event_comments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_event_comments__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_event_comments_event_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "event_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_event_comments_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_event_likes", x => x.id);
                    table.ForeignKey(
                        name: "f_k_event_likes__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_event_likes_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    media_url = table.Column<string>(type: "text", nullable: false),
                    media_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_event_media", x => x.id);
                    table.ForeignKey(
                        name: "f_k_event_media_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_views",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    view_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_event_views", x => x.id);
                    table.ForeignKey(
                        name: "f_k_event_views__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_event_views_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    news_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    parent_comment_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_news_comments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_news_comments__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_news_comments_news_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "news_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_news_comments_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    news_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_news_likes", x => x.id);
                    table.ForeignKey(
                        name: "f_k_news_likes__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_news_likes_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    news_id = table.Column<int>(type: "integer", nullable: false),
                    media_url = table.Column<string>(type: "text", nullable: false),
                    media_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_news_media", x => x.id);
                    table.ForeignKey(
                        name: "f_k_news_media_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_views",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    news_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    view_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_news_views", x => x.id);
                    table.ForeignKey(
                        name: "f_k_news_views__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_news_views_news_news_id",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admin_tariff_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariff_id = table.Column<int>(type: "integer", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    requires_password = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_admin_tariff_options", x => x.id);
                    table.ForeignKey(
                        name: "f_k_admin_tariff_options_admin_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "admin_tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_admin_privileges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    steam_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    tariff_id = table.Column<int>(type: "integer", nullable: true),
                    tariff_option_id = table.Column<int>(type: "integer", nullable: true),
                    flags = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    group_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    immunity = table.Column<int>(type: "integer", nullable: false),
                    starts_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    sourcebans_admin_id = table.Column<int>(type: "integer", nullable: true),
                    transaction_id = table.Column<int>(type: "integer", nullable: true),
                    admin_password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_admin_privileges", x => x.id);
                    table.ForeignKey(
                        name: "f_k_user_admin_privileges_admin_tariff_options_tariff_option_id",
                        column: x => x.tariff_option_id,
                        principalTable: "admin_tariff_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_user_admin_privileges_admin_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "admin_tariffs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_user_admin_privileges_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_admin_privileges_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "donation_transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    steam_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tariff_id = table.Column<int>(type: "integer", nullable: true),
                    tariff_option_id = table.Column<int>(type: "integer", nullable: true),
                    vip_tariff_id = table.Column<int>(type: "integer", nullable: true),
                    vip_tariff_option_id = table.Column<int>(type: "integer", nullable: true),
                    privilege_id = table.Column<int>(type: "integer", nullable: true),
                    server_id = table.Column<int>(type: "integer", nullable: true),
                    admin_password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    payment_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    pending_expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_donation_transactions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_donation_transactions__servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions_admin_tariff_options_tariff_option_id",
                        column: x => x.tariff_option_id,
                        principalTable: "admin_tariff_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions_admin_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "admin_tariffs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions_user_admin_privileges_privilege_id",
                        column: x => x.privilege_id,
                        principalTable: "user_admin_privileges",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions_vip_tariff_options_vip_tariff_option_~",
                        column: x => x.vip_tariff_option_id,
                        principalTable: "vip_tariff_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_donation_transactions_vip_tariffs_vip_tariff_id",
                        column: x => x.vip_tariff_id,
                        principalTable: "vip_tariffs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_vip_privileges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    steam_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    tariff_id = table.Column<int>(type: "integer", nullable: true),
                    tariff_option_id = table.Column<int>(type: "integer", nullable: true),
                    group_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    starts_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    transaction_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_vip_privileges", x => x.id);
                    table.ForeignKey(
                        name: "f_k_user_vip_privileges_donation_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "donation_transactions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_user_vip_privileges_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_vip_privileges_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_vip_privileges_vip_tariff_options_tariff_option_id",
                        column: x => x.tariff_option_id,
                        principalTable: "vip_tariff_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_user_vip_privileges_vip_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "vip_tariffs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariff_groups_server_id_is_active",
                table: "admin_tariff_groups",
                columns: new[] { "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariff_options_tariff_id_is_active",
                table: "admin_tariff_options",
                columns: new[] { "tariff_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "i_x_admin_tariffs_admin_tariff_group_id",
                table: "admin_tariffs",
                column: "admin_tariff_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_admin_tariffs_server_id_is_active",
                table: "admin_tariffs",
                columns: new[] { "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "i_x_blocked_ips_blocked_by_user_id",
                table: "blocked_ips",
                column: "blocked_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_blocked_ips_ip_address",
                table: "blocked_ips",
                column: "ip_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_custom_page_media_custom_page_id",
                table: "custom_page_media",
                column: "custom_page_id");

            migrationBuilder.CreateIndex(
                name: "i_x_custom_page_views_user_id",
                table: "custom_page_views",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_page_views_custom_page_id_ip_address_view_date",
                table: "custom_page_views",
                columns: new[] { "custom_page_id", "ip_address", "view_date" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_page_views_custom_page_id_user_id_view_date",
                table: "custom_page_views",
                columns: new[] { "custom_page_id", "user_id", "view_date" });

            migrationBuilder.CreateIndex(
                name: "i_x_custom_pages_author_id",
                table: "custom_pages",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_pages_slug",
                table: "custom_pages",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_privilege_id",
                table: "donation_transactions",
                column: "privilege_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_server_id",
                table: "donation_transactions",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_tariff_id",
                table: "donation_transactions",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_tariff_option_id",
                table: "donation_transactions",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_vip_tariff_id",
                table: "donation_transactions",
                column: "vip_tariff_id");

            migrationBuilder.CreateIndex(
                name: "i_x_donation_transactions_vip_tariff_option_id",
                table: "donation_transactions",
                column: "vip_tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_steam_id",
                table: "donation_transactions",
                column: "steam_id");

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_transaction_id",
                table: "donation_transactions",
                column: "transaction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_donation_transactions_user_id_status",
                table: "donation_transactions",
                columns: new[] { "user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_event_comments_event_id",
                table: "event_comments",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "i_x_event_comments_parent_comment_id",
                table: "event_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_event_comments_user_id",
                table: "event_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_event_likes_user_id",
                table: "event_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_likes_event_id_user_id",
                table: "event_likes",
                columns: new[] { "event_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_event_media_event_id",
                table: "event_media",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "i_x_event_views_user_id",
                table: "event_views",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_views_event_id_ip_address_view_date",
                table: "event_views",
                columns: new[] { "event_id", "ip_address", "view_date" });

            migrationBuilder.CreateIndex(
                name: "IX_event_views_event_id_user_id_view_date",
                table: "event_views",
                columns: new[] { "event_id", "user_id", "view_date" });

            migrationBuilder.CreateIndex(
                name: "i_x_events_author_id",
                table: "events",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_slug",
                table: "events",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_nav_section_items_custom_page_id",
                table: "nav_section_items",
                column: "custom_page_id");

            migrationBuilder.CreateIndex(
                name: "IX_nav_section_items_section_id_order",
                table: "nav_section_items",
                columns: new[] { "section_id", "order" });

            migrationBuilder.CreateIndex(
                name: "IX_nav_sections_is_published",
                table: "nav_sections",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "IX_nav_sections_order",
                table: "nav_sections",
                column: "order");

            migrationBuilder.CreateIndex(
                name: "i_x_news_author_id",
                table: "news",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_news_slug",
                table: "news",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_news_comments_news_id",
                table: "news_comments",
                column: "news_id");

            migrationBuilder.CreateIndex(
                name: "i_x_news_comments_parent_comment_id",
                table: "news_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_news_comments_user_id",
                table: "news_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_news_likes_user_id",
                table: "news_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_news_likes_news_id_user_id",
                table: "news_likes",
                columns: new[] { "news_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_news_media_news_id",
                table: "news_media",
                column: "news_id");

            migrationBuilder.CreateIndex(
                name: "i_x_news_views_user_id",
                table: "news_views",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_news_views_news_id_ip_address_view_date",
                table: "news_views",
                columns: new[] { "news_id", "ip_address", "view_date" });

            migrationBuilder.CreateIndex(
                name: "IX_news_views_news_id_user_id_view_date",
                table: "news_views",
                columns: new[] { "news_id", "user_id", "view_date" });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_at",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id_is_read",
                table: "notifications",
                columns: new[] { "user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_token",
                table: "password_reset_tokens",
                column: "token");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_user_id_is_used",
                table: "password_reset_tokens",
                columns: new[] { "user_id", "is_used" });

            migrationBuilder.CreateIndex(
                name: "IX_site_settings_category",
                table: "site_settings",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_site_settings_key",
                table: "site_settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sourcebans_settings_server_id",
                table: "sourcebans_settings",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "i_x_telegram_subscribers_user_id",
                table: "telegram_subscribers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_telegram_subscribers_chat_id",
                table: "telegram_subscribers",
                column: "chat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_user_admin_privileges_server_id",
                table: "user_admin_privileges",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_admin_privileges_tariff_id",
                table: "user_admin_privileges",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_admin_privileges_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_admin_privileges_expires_at",
                table: "user_admin_privileges",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_user_admin_privileges_steam_id_server_id_is_active",
                table: "user_admin_privileges",
                columns: new[] { "steam_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_user_admin_privileges_user_id_server_id_is_active",
                table: "user_admin_privileges",
                columns: new[] { "user_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "i_x_user_vip_privileges_server_id",
                table: "user_vip_privileges",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_vip_privileges_tariff_id",
                table: "user_vip_privileges",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_vip_privileges_tariff_option_id",
                table: "user_vip_privileges",
                column: "tariff_option_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_vip_privileges_transaction_id",
                table: "user_vip_privileges",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_expires_at",
                table: "user_vip_privileges",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_steam_id_server_id_is_active",
                table: "user_vip_privileges",
                columns: new[] { "steam_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_user_vip_privileges_user_id_server_id_is_active",
                table: "user_vip_privileges",
                columns: new[] { "user_id", "server_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_vip_applications_server_id",
                table: "vip_applications",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_vip_applications_user_id_server_id_status",
                table: "vip_applications",
                columns: new[] { "user_id", "server_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_vip_settings_server_id",
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
                name: "blocked_ips");

            migrationBuilder.DropTable(
                name: "custom_page_media");

            migrationBuilder.DropTable(
                name: "custom_page_views");

            migrationBuilder.DropTable(
                name: "donation_packages");

            migrationBuilder.DropTable(
                name: "event_comments");

            migrationBuilder.DropTable(
                name: "event_likes");

            migrationBuilder.DropTable(
                name: "event_media");

            migrationBuilder.DropTable(
                name: "event_views");

            migrationBuilder.DropTable(
                name: "nav_section_items");

            migrationBuilder.DropTable(
                name: "news_comments");

            migrationBuilder.DropTable(
                name: "news_likes");

            migrationBuilder.DropTable(
                name: "news_media");

            migrationBuilder.DropTable(
                name: "news_views");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "password_reset_tokens");

            migrationBuilder.DropTable(
                name: "site_settings");

            migrationBuilder.DropTable(
                name: "slider_images");

            migrationBuilder.DropTable(
                name: "smtp_settings");

            migrationBuilder.DropTable(
                name: "sourcebans_settings");

            migrationBuilder.DropTable(
                name: "telegram_subscribers");

            migrationBuilder.DropTable(
                name: "user_vip_privileges");

            migrationBuilder.DropTable(
                name: "vip_applications");

            migrationBuilder.DropTable(
                name: "vip_settings");

            migrationBuilder.DropTable(
                name: "yoomoney_settings");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "custom_pages");

            migrationBuilder.DropTable(
                name: "nav_sections");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "donation_transactions");

            migrationBuilder.DropTable(
                name: "user_admin_privileges");

            migrationBuilder.DropTable(
                name: "vip_tariff_options");

            migrationBuilder.DropTable(
                name: "admin_tariff_options");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vip_tariffs");

            migrationBuilder.DropTable(
                name: "admin_tariffs");

            migrationBuilder.DropTable(
                name: "admin_tariff_groups");

            migrationBuilder.DropTable(
                name: "servers");
        }
    }
}
