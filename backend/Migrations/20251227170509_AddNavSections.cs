using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNavSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_tariff_groups_Servers_server_id",
                table: "admin_tariff_groups");

            migrationBuilder.DropForeignKey(
                name: "FK_admin_tariff_options_admin_tariffs_tariff_id",
                table: "admin_tariff_options");

            migrationBuilder.DropForeignKey(
                name: "FK_admin_tariffs_Servers_server_id",
                table: "admin_tariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId",
                table: "admin_tariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedIps_Users_BlockedByUserId",
                table: "BlockedIps");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomPageMedia_CustomPages_CustomPageId",
                table: "CustomPageMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomPages_Users_AuthorId",
                table: "CustomPages");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomPageViews_CustomPages_CustomPageId",
                table: "CustomPageViews");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomPageViews_Users_UserId",
                table: "CustomPageViews");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_Servers_server_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_Users_user_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_admin_tariffs_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_donation_transactions_vip_tariffs_vip_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EventComments_EventComments_ParentCommentId",
                table: "EventComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EventComments_Events_EventId",
                table: "EventComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EventComments_Users_UserId",
                table: "EventComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLikes_Events_EventId",
                table: "EventLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLikes_Users_UserId",
                table: "EventLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventMedia_Events_EventId",
                table: "EventMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_AuthorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_EventViews_Events_EventId",
                table: "EventViews");

            migrationBuilder.DropForeignKey(
                name: "FK_EventViews_Users_UserId",
                table: "EventViews");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Users_AuthorId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_NewsComments_ParentCommentId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_Users_UserId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsLikes_News_NewsId",
                table: "NewsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsLikes_Users_UserId",
                table: "NewsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsMedia_News_NewsId",
                table: "NewsMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsViews_News_NewsId",
                table: "NewsViews");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsViews_Users_UserId",
                table: "NewsViews");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_password_reset_tokens_Users_user_id",
                table: "password_reset_tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_sourcebans_settings_Servers_server_id",
                table: "sourcebans_settings");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramSubscribers_Users_UserId",
                table: "TelegramSubscribers");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_Servers_server_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_Users_user_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_admin_privileges_admin_tariffs_tariff_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_vip_privileges_Servers_server_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_vip_privileges_Users_user_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_vip_privileges_donation_transactions_transaction_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_vip_privileges_vip_tariff_options_tariff_option_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_user_vip_privileges_vip_tariffs_tariff_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "FK_vip_settings_Servers_server_id",
                table: "vip_settings");

            migrationBuilder.DropForeignKey(
                name: "FK_vip_tariff_options_vip_tariffs_tariff_id",
                table: "vip_tariff_options");

            migrationBuilder.DropForeignKey(
                name: "FK_vip_tariffs_Servers_server_id",
                table: "vip_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_yoomoney_settings",
                table: "yoomoney_settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vip_tariffs",
                table: "vip_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vip_tariff_options",
                table: "vip_tariff_options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vip_settings",
                table: "vip_settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_vip_privileges",
                table: "user_vip_privileges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_admin_privileges",
                table: "user_admin_privileges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sourcebans_settings",
                table: "sourcebans_settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_smtp_settings",
                table: "smtp_settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servers",
                table: "Servers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_password_reset_tokens",
                table: "password_reset_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_donation_transactions",
                table: "donation_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_donation_packages",
                table: "donation_packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin_tariffs",
                table: "admin_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin_tariff_options",
                table: "admin_tariff_options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin_tariff_groups",
                table: "admin_tariff_groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramSubscribers",
                table: "TelegramSubscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderImages",
                table: "SliderImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteSettings",
                table: "SiteSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsViews",
                table: "NewsViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsMedia",
                table: "NewsMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsLikes",
                table: "NewsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsComments",
                table: "NewsComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventViews",
                table: "EventViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMedia",
                table: "EventMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventLikes",
                table: "EventLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventComments",
                table: "EventComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomPageViews",
                table: "CustomPageViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomPages",
                table: "CustomPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomPageMedia",
                table: "CustomPageMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedIps",
                table: "BlockedIps");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Servers",
                newName: "servers");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

            migrationBuilder.RenameTable(
                name: "News",
                newName: "news");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "events");

            migrationBuilder.RenameTable(
                name: "TelegramSubscribers",
                newName: "telegram_subscribers");

            migrationBuilder.RenameTable(
                name: "SliderImages",
                newName: "slider_images");

            migrationBuilder.RenameTable(
                name: "SiteSettings",
                newName: "site_settings");

            migrationBuilder.RenameTable(
                name: "NewsViews",
                newName: "news_views");

            migrationBuilder.RenameTable(
                name: "NewsMedia",
                newName: "news_media");

            migrationBuilder.RenameTable(
                name: "NewsLikes",
                newName: "news_likes");

            migrationBuilder.RenameTable(
                name: "NewsComments",
                newName: "news_comments");

            migrationBuilder.RenameTable(
                name: "EventViews",
                newName: "event_views");

            migrationBuilder.RenameTable(
                name: "EventMedia",
                newName: "event_media");

            migrationBuilder.RenameTable(
                name: "EventLikes",
                newName: "event_likes");

            migrationBuilder.RenameTable(
                name: "EventComments",
                newName: "event_comments");

            migrationBuilder.RenameTable(
                name: "CustomPageViews",
                newName: "custom_page_views");

            migrationBuilder.RenameTable(
                name: "CustomPages",
                newName: "custom_pages");

            migrationBuilder.RenameTable(
                name: "CustomPageMedia",
                newName: "custom_page_media");

            migrationBuilder.RenameTable(
                name: "BlockedIps",
                newName: "blocked_ips");

            migrationBuilder.RenameIndex(
                name: "IX_vip_settings_server_id",
                table: "vip_settings",
                newName: "i_x_vip_settings_server_id");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SteamProfileUrl",
                table: "users",
                newName: "steam_profile_url");

            migrationBuilder.RenameColumn(
                name: "SteamId",
                table: "users",
                newName: "steam_id");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "LastIp",
                table: "users",
                newName: "last_ip");

            migrationBuilder.RenameColumn(
                name: "IsBlocked",
                table: "users",
                newName: "is_blocked");

            migrationBuilder.RenameColumn(
                name: "IsAdmin",
                table: "users",
                newName: "is_admin");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BlockedAt",
                table: "users",
                newName: "blocked_at");

            migrationBuilder.RenameColumn(
                name: "BlockReason",
                table: "users",
                newName: "block_reason");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "users",
                newName: "avatar_url");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "users",
                newName: "IX_users_username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.RenameIndex(
                name: "IX_user_vip_privileges_transaction_id",
                table: "user_vip_privileges",
                newName: "i_x_user_vip_privileges_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_vip_privileges_tariff_option_id",
                table: "user_vip_privileges",
                newName: "i_x_user_vip_privileges_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_vip_privileges_tariff_id",
                table: "user_vip_privileges",
                newName: "i_x_user_vip_privileges_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_vip_privileges_server_id",
                table: "user_vip_privileges",
                newName: "i_x_user_vip_privileges_server_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_admin_privileges_tariff_option_id",
                table: "user_admin_privileges",
                newName: "i_x_user_admin_privileges_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_admin_privileges_tariff_id",
                table: "user_admin_privileges",
                newName: "i_x_user_admin_privileges_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_admin_privileges_server_id",
                table: "user_admin_privileges",
                newName: "i_x_user_admin_privileges_server_id");

            migrationBuilder.RenameIndex(
                name: "IX_sourcebans_settings_server_id",
                table: "sourcebans_settings",
                newName: "i_x_sourcebans_settings_server_id");

            migrationBuilder.RenameColumn(
                name: "Port",
                table: "servers",
                newName: "port");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "servers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "servers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MaxPlayers",
                table: "servers",
                newName: "max_players");

            migrationBuilder.RenameColumn(
                name: "MapName",
                table: "servers",
                newName: "map_name");

            migrationBuilder.RenameColumn(
                name: "LastChecked",
                table: "servers",
                newName: "last_checked");

            migrationBuilder.RenameColumn(
                name: "IsOnline",
                table: "servers",
                newName: "is_online");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "servers",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "CurrentPlayers",
                table: "servers",
                newName: "current_players");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "notifications",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "notifications",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "notifications",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "notifications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "notifications",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RelatedEntityId",
                table: "notifications",
                newName: "related_entity_id");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "notifications",
                newName: "is_read");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "notifications",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "notifications",
                newName: "IX_notifications_user_id_is_read");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CreatedAt",
                table: "notifications",
                newName: "IX_notifications_created_at");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "news",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "news",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "news",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "news",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "news",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "news",
                newName: "view_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "news",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "LikeCount",
                table: "news",
                newName: "like_count");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "news",
                newName: "is_published");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "news",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "news",
                newName: "cover_image");

            migrationBuilder.RenameColumn(
                name: "CommentCount",
                table: "news",
                newName: "comment_count");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "news",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_News_Slug",
                table: "news",
                newName: "IX_news_slug");

            migrationBuilder.RenameIndex(
                name: "IX_News_AuthorId",
                table: "news",
                newName: "i_x_news_author_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "events",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "events",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "events",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "events",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "events",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "events",
                newName: "view_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "events",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "events",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "LikeCount",
                table: "events",
                newName: "like_count");

            migrationBuilder.RenameColumn(
                name: "IsStartNotificationSent",
                table: "events",
                newName: "is_start_notification_sent");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "events",
                newName: "is_published");

            migrationBuilder.RenameColumn(
                name: "IsEndNotificationSent",
                table: "events",
                newName: "is_end_notification_sent");

            migrationBuilder.RenameColumn(
                name: "IsCreatedNotificationSent",
                table: "events",
                newName: "is_created_notification_sent");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "events",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "events",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "events",
                newName: "cover_image");

            migrationBuilder.RenameColumn(
                name: "CommentCount",
                table: "events",
                newName: "comment_count");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "events",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_Events_Slug",
                table: "events",
                newName: "IX_events_slug");

            migrationBuilder.RenameIndex(
                name: "IX_Events_AuthorId",
                table: "events",
                newName: "i_x_events_author_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_vip_tariff_option_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_vip_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_vip_tariff_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_vip_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_tariff_option_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_tariff_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_tariff_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_server_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_server_id");

            migrationBuilder.RenameIndex(
                name: "IX_donation_transactions_privilege_id",
                table: "donation_transactions",
                newName: "i_x_donation_transactions_privilege_id");

            migrationBuilder.RenameColumn(
                name: "AdminTariffGroupId",
                table: "admin_tariffs",
                newName: "admin_tariff_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_admin_tariffs_AdminTariffGroupId",
                table: "admin_tariffs",
                newName: "i_x_admin_tariffs_admin_tariff_group_id");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "telegram_subscribers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "telegram_subscribers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "telegram_subscribers",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "SubscribedAt",
                table: "telegram_subscribers",
                newName: "subscribed_at");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "telegram_subscribers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "telegram_subscribers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "telegram_subscribers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "telegram_subscribers",
                newName: "chat_id");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramSubscribers_UserId",
                table: "telegram_subscribers",
                newName: "i_x_telegram_subscribers_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramSubscribers_ChatId",
                table: "telegram_subscribers",
                newName: "IX_telegram_subscribers_chat_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "slider_images",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "slider_images",
                newName: "order");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "slider_images",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Buttons",
                table: "slider_images",
                newName: "buttons");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "slider_images",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "slider_images",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "slider_images",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "site_settings",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "site_settings",
                newName: "key");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "site_settings",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "site_settings",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_settings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "site_settings",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "DataType",
                table: "site_settings",
                newName: "data_type");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "site_settings",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_SiteSettings_Key",
                table: "site_settings",
                newName: "IX_site_settings_key");

            migrationBuilder.RenameIndex(
                name: "IX_SiteSettings_Category",
                table: "site_settings",
                newName: "IX_site_settings_category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "news_views",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewDate",
                table: "news_views",
                newName: "view_date");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "news_views",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "news_views",
                newName: "news_id");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "news_views",
                newName: "ip_address");

            migrationBuilder.RenameIndex(
                name: "IX_NewsViews_UserId",
                table: "news_views",
                newName: "i_x_news_views_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsViews_NewsId_UserId_ViewDate",
                table: "news_views",
                newName: "IX_news_views_news_id_user_id_view_date");

            migrationBuilder.RenameIndex(
                name: "IX_NewsViews_NewsId_IpAddress_ViewDate",
                table: "news_views",
                newName: "IX_news_views_news_id_ip_address_view_date");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "news_media",
                newName: "order");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "news_media",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "news_media",
                newName: "news_id");

            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "news_media",
                newName: "media_url");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "news_media",
                newName: "media_type");

            migrationBuilder.RenameIndex(
                name: "IX_NewsMedia_NewsId",
                table: "news_media",
                newName: "i_x_news_media_news_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "news_likes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "news_likes",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "news_likes",
                newName: "news_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "news_likes",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_NewsLikes_UserId",
                table: "news_likes",
                newName: "i_x_news_likes_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsLikes_NewsId_UserId",
                table: "news_likes",
                newName: "IX_news_likes_news_id_user_id");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "news_comments",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "news_comments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "news_comments",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "news_comments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "news_comments",
                newName: "parent_comment_id");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "news_comments",
                newName: "news_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "news_comments",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_UserId",
                table: "news_comments",
                newName: "i_x_news_comments_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_ParentCommentId",
                table: "news_comments",
                newName: "i_x_news_comments_parent_comment_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_NewsId",
                table: "news_comments",
                newName: "i_x_news_comments_news_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "event_views",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewDate",
                table: "event_views",
                newName: "view_date");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "event_views",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "event_views",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "event_views",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventViews_UserId",
                table: "event_views",
                newName: "i_x_event_views_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventViews_EventId_UserId_ViewDate",
                table: "event_views",
                newName: "IX_event_views_event_id_user_id_view_date");

            migrationBuilder.RenameIndex(
                name: "IX_EventViews_EventId_IpAddress_ViewDate",
                table: "event_views",
                newName: "IX_event_views_event_id_ip_address_view_date");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "event_media",
                newName: "order");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "event_media",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "event_media",
                newName: "media_url");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "event_media",
                newName: "media_type");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "event_media",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventMedia_EventId",
                table: "event_media",
                newName: "i_x_event_media_event_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "event_likes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "event_likes",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "event_likes",
                newName: "event_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "event_likes",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_EventLikes_UserId",
                table: "event_likes",
                newName: "i_x_event_likes_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventLikes_EventId_UserId",
                table: "event_likes",
                newName: "IX_event_likes_event_id_user_id");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "event_comments",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "event_comments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "event_comments",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "event_comments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "event_comments",
                newName: "parent_comment_id");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "event_comments",
                newName: "event_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "event_comments",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_EventComments_UserId",
                table: "event_comments",
                newName: "i_x_event_comments_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventComments_ParentCommentId",
                table: "event_comments",
                newName: "i_x_event_comments_parent_comment_id");

            migrationBuilder.RenameIndex(
                name: "IX_EventComments_EventId",
                table: "event_comments",
                newName: "i_x_event_comments_event_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "custom_page_views",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewDate",
                table: "custom_page_views",
                newName: "view_date");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "custom_page_views",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "custom_page_views",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "CustomPageId",
                table: "custom_page_views",
                newName: "custom_page_id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPageViews_UserId",
                table: "custom_page_views",
                newName: "i_x_custom_page_views_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPageViews_CustomPageId_UserId_ViewDate",
                table: "custom_page_views",
                newName: "IX_custom_page_views_custom_page_id_user_id_view_date");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPageViews_CustomPageId_IpAddress_ViewDate",
                table: "custom_page_views",
                newName: "IX_custom_page_views_custom_page_id_ip_address_view_date");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "custom_pages",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "custom_pages",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "custom_pages",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "custom_pages",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "custom_pages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "custom_pages",
                newName: "view_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "custom_pages",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "custom_pages",
                newName: "is_published");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "custom_pages",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "custom_pages",
                newName: "cover_image");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "custom_pages",
                newName: "author_id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPages_Slug",
                table: "custom_pages",
                newName: "IX_custom_pages_slug");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPages_AuthorId",
                table: "custom_pages",
                newName: "i_x_custom_pages_author_id");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "custom_page_media",
                newName: "order");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "custom_page_media",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "custom_page_media",
                newName: "media_url");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "custom_page_media",
                newName: "media_type");

            migrationBuilder.RenameColumn(
                name: "CustomPageId",
                table: "custom_page_media",
                newName: "custom_page_id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomPageMedia_CustomPageId",
                table: "custom_page_media",
                newName: "i_x_custom_page_media_custom_page_id");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "blocked_ips",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "blocked_ips",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "blocked_ips",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "BlockedByUserId",
                table: "blocked_ips",
                newName: "blocked_by_user_id");

            migrationBuilder.RenameColumn(
                name: "BlockedAt",
                table: "blocked_ips",
                newName: "blocked_at");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedIps_IpAddress",
                table: "blocked_ips",
                newName: "IX_blocked_ips_ip_address");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedIps_BlockedByUserId",
                table: "blocked_ips",
                newName: "i_x_blocked_ips_blocked_by_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "wallet_number",
                table: "yoomoney_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "yoomoney_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "secret_key",
                table: "yoomoney_settings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "yoomoney_settings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "yoomoney_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "vip_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "vip_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "vip_tariffs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "vip_tariffs",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "vip_tariffs",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vip_tariffs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_tariffs",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "vip_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "vip_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "vip_tariff_options",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "duration_days",
                table: "vip_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_tariff_options",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "vip_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "vip_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "vip_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "vip_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "vip_settings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "vip_settings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "vip_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "database",
                table: "vip_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "steam_profile_url",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "last_ip",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_blocked",
                table: "users",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_admin",
                table: "users",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "blocked_at",
                table: "users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "block_reason",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "avatar_url",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "user_vip_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "user_vip_privileges",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "starts_at",
                table: "user_vip_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "user_vip_privileges",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "user_vip_privileges",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "user_vip_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "user_vip_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_vip_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "user_admin_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "user_admin_privileges",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "starts_at",
                table: "user_admin_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "sourcebans_admin_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "user_admin_privileges",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "immunity",
                table: "user_admin_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "user_admin_privileges",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "flags",
                table: "user_admin_privileges",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "user_admin_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "user_admin_privileges",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "admin_password",
                table: "user_admin_privileges",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_admin_privileges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "sourcebans_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "sourcebans_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "sourcebans_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "sourcebans_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "sourcebans_settings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "sourcebans_settings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "sourcebans_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "database",
                table: "sourcebans_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "sourcebans_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "sourcebans_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "smtp_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "smtp_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "smtp_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "smtp_settings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "smtp_settings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "smtp_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "from_name",
                table: "smtp_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "from_email",
                table: "smtp_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "enable_ssl",
                table: "smtp_settings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "smtp_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "servers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "max_players",
                table: "servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "map_name",
                table: "servers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_checked",
                table: "servers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_online",
                table: "servers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "servers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<int>(
                name: "current_players",
                table: "servers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "rcon_password",
                table: "servers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "password_reset_tokens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "used_at",
                table: "password_reset_tokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "password_reset_tokens",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_used",
                table: "password_reset_tokens",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "password_reset_tokens",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "password_reset_tokens",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "password_reset_tokens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "notifications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "notifications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "notifications",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "notifications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "notifications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "related_entity_id",
                table: "notifications",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_read",
                table: "notifications",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "notifications",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "news",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "summary",
                table: "news",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "news",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "news",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "news",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "view_count",
                table: "news",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "news",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "like_count",
                table: "news",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_published",
                table: "news",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "news",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "cover_image",
                table: "news",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "comment_count",
                table: "news",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "news",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "events",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "summary",
                table: "events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "events",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "events",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "view_count",
                table: "events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_date",
                table: "events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "like_count",
                table: "events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_start_notification_sent",
                table: "events",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_published",
                table: "events",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_end_notification_sent",
                table: "events",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_created_notification_sent",
                table: "events",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "events",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "cover_image",
                table: "events",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "comment_count",
                table: "events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "vip_tariff_option_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "vip_tariff_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "donation_transactions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "donation_transactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "donation_transactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "donation_transactions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "privilege_id",
                table: "donation_transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "pending_expires_at",
                table: "donation_transactions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_url",
                table: "donation_transactions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_method",
                table: "donation_transactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "label",
                table: "donation_transactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "donation_transactions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "donation_transactions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "completed_at",
                table: "donation_transactions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "cancelled_at",
                table: "donation_transactions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "admin_password",
                table: "donation_transactions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "donation_transactions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "donation_packages",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "donation_packages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "suggested_amounts",
                table: "donation_packages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "donation_packages",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "donation_packages",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "donation_packages",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "donation_packages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "admin_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "admin_tariffs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariffs",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "immunity",
                table: "admin_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "admin_tariffs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "flags",
                table: "admin_tariffs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "admin_tariffs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariffs",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "admin_tariff_group_id",
                table: "admin_tariffs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "admin_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "requires_password",
                table: "admin_tariff_options",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariff_options",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "duration_days",
                table: "admin_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariff_options",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariff_options",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "admin_tariff_groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariff_groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "admin_tariff_groups",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariff_groups",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "admin_tariff_groups",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariff_groups",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariff_groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "telegram_subscribers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "telegram_subscribers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "telegram_subscribers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "subscribed_at",
                table: "telegram_subscribers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "telegram_subscribers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "telegram_subscribers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "telegram_subscribers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "slider_images",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "slider_images",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "slider_images",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "buttons",
                table: "slider_images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "slider_images",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "slider_images",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "slider_images",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "site_settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "site_settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "site_settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "site_settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "site_settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "site_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "data_type",
                table: "site_settings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "site_settings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "news_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "view_date",
                table: "news_views",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "news_views",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "news_id",
                table: "news_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "news_views",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "news_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "news_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "news_id",
                table: "news_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "media_url",
                table: "news_media",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "media_type",
                table: "news_media",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "news_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "news_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "news_id",
                table: "news_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "news_likes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "news_comments",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "news_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "news_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "news_comments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "parent_comment_id",
                table: "news_comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "news_id",
                table: "news_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "news_comments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "event_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "view_date",
                table: "event_views",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "event_views",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "event_views",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "event_id",
                table: "event_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "event_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "event_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "media_url",
                table: "event_media",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "media_type",
                table: "event_media",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "event_id",
                table: "event_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "event_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "event_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "event_id",
                table: "event_likes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "event_likes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "event_comments",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "event_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "event_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "event_comments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "parent_comment_id",
                table: "event_comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "event_id",
                table: "event_comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "event_comments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "custom_page_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "view_date",
                table: "custom_page_views",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "custom_page_views",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "custom_page_views",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "custom_page_id",
                table: "custom_page_views",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "custom_pages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "summary",
                table: "custom_pages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "custom_pages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "custom_pages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "custom_pages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "view_count",
                table: "custom_pages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "custom_pages",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_published",
                table: "custom_pages",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "custom_pages",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "cover_image",
                table: "custom_pages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "custom_pages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "custom_page_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "custom_page_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "media_url",
                table: "custom_page_media",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "media_type",
                table: "custom_page_media",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "custom_page_id",
                table: "custom_page_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "reason",
                table: "blocked_ips",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "blocked_ips",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "blocked_ips",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<int>(
                name: "blocked_by_user_id",
                table: "blocked_ips",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "blocked_at",
                table: "blocked_ips",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_yoomoney_settings",
                table: "yoomoney_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_vip_tariffs",
                table: "vip_tariffs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_vip_tariff_options",
                table: "vip_tariff_options",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_vip_settings",
                table: "vip_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_user_vip_privileges",
                table: "user_vip_privileges",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_user_admin_privileges",
                table: "user_admin_privileges",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_sourcebans_settings",
                table: "sourcebans_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_smtp_settings",
                table: "smtp_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_servers",
                table: "servers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_password_reset_tokens",
                table: "password_reset_tokens",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_notifications",
                table: "notifications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_news",
                table: "news",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_events",
                table: "events",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_donation_transactions",
                table: "donation_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_donation_packages",
                table: "donation_packages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_admin_tariffs",
                table: "admin_tariffs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_admin_tariff_options",
                table: "admin_tariff_options",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_admin_tariff_groups",
                table: "admin_tariff_groups",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_telegram_subscribers",
                table: "telegram_subscribers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_slider_images",
                table: "slider_images",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_site_settings",
                table: "site_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_news_views",
                table: "news_views",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_news_media",
                table: "news_media",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_news_likes",
                table: "news_likes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_news_comments",
                table: "news_comments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_event_views",
                table: "event_views",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_event_media",
                table: "event_media",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_event_likes",
                table: "event_likes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_event_comments",
                table: "event_comments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_custom_page_views",
                table: "custom_page_views",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_custom_pages",
                table: "custom_pages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_custom_page_media",
                table: "custom_page_media",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_blocked_ips",
                table: "blocked_ips",
                column: "id");

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
                name: "i_x_vip_applications_server_id",
                table: "vip_applications",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_vip_applications_user_id_server_id_status",
                table: "vip_applications",
                columns: new[] { "user_id", "server_id", "status" });

            migrationBuilder.AddForeignKey(
                name: "f_k_admin_tariff_groups__servers_server_id",
                table: "admin_tariff_groups",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_admin_tariff_options_admin_tariffs_tariff_id",
                table: "admin_tariff_options",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_admin_tariffs__servers_server_id",
                table: "admin_tariffs",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_admin_tariffs_admin_tariff_groups_admin_tariff_group_id",
                table: "admin_tariffs",
                column: "admin_tariff_group_id",
                principalTable: "admin_tariff_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_blocked_ips__users_blocked_by_user_id",
                table: "blocked_ips",
                column: "blocked_by_user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_custom_page_media_custom_pages_custom_page_id",
                table: "custom_page_media",
                column: "custom_page_id",
                principalTable: "custom_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_custom_page_views__users_user_id",
                table: "custom_page_views",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_custom_page_views_custom_pages_custom_page_id",
                table: "custom_page_views",
                column: "custom_page_id",
                principalTable: "custom_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_custom_pages__users_author_id",
                table: "custom_pages",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions__servers_server_id",
                table: "donation_transactions",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions__users_user_id",
                table: "donation_transactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions_admin_tariffs_tariff_id",
                table: "donation_transactions",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions",
                column: "privilege_id",
                principalTable: "user_admin_privileges",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions_vip_tariff_options_vip_tariff_option_~",
                table: "donation_transactions",
                column: "vip_tariff_option_id",
                principalTable: "vip_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_donation_transactions_vip_tariffs_vip_tariff_id",
                table: "donation_transactions",
                column: "vip_tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_event_comments__users_user_id",
                table: "event_comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_comments_event_comments_parent_comment_id",
                table: "event_comments",
                column: "parent_comment_id",
                principalTable: "event_comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_comments_events_event_id",
                table: "event_comments",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_likes__users_user_id",
                table: "event_likes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_likes_events_event_id",
                table: "event_likes",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_media_events_event_id",
                table: "event_media",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_event_views__users_user_id",
                table: "event_views",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_event_views_events_event_id",
                table: "event_views",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_events__users_author_id",
                table: "events",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_news__users_author_id",
                table: "news",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_comments__users_user_id",
                table: "news_comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_comments_news_comments_parent_comment_id",
                table: "news_comments",
                column: "parent_comment_id",
                principalTable: "news_comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_comments_news_news_id",
                table: "news_comments",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_likes__users_user_id",
                table: "news_likes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_likes_news_news_id",
                table: "news_likes",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_media_news_news_id",
                table: "news_media",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_news_views__users_user_id",
                table: "news_views",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_news_views_news_news_id",
                table: "news_views",
                column: "news_id",
                principalTable: "news",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_notifications__users_user_id",
                table: "notifications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_password_reset_tokens__users_user_id",
                table: "password_reset_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_sourcebans_settings_servers_server_id",
                table: "sourcebans_settings",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_telegram_subscribers__users_user_id",
                table: "telegram_subscribers",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_user_admin_privileges_admin_tariffs_tariff_id",
                table: "user_admin_privileges",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_user_admin_privileges_servers_server_id",
                table: "user_admin_privileges",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_user_admin_privileges_users_user_id",
                table: "user_admin_privileges",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_user_vip_privileges_donation_transactions_transaction_id",
                table: "user_vip_privileges",
                column: "transaction_id",
                principalTable: "donation_transactions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_user_vip_privileges_servers_server_id",
                table: "user_vip_privileges",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_user_vip_privileges_users_user_id",
                table: "user_vip_privileges",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_user_vip_privileges_vip_tariff_options_tariff_option_id",
                table: "user_vip_privileges",
                column: "tariff_option_id",
                principalTable: "vip_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_user_vip_privileges_vip_tariffs_tariff_id",
                table: "user_vip_privileges",
                column: "tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_vip_settings_servers_server_id",
                table: "vip_settings",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_vip_tariff_options_vip_tariffs_tariff_id",
                table: "vip_tariff_options",
                column: "tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_vip_tariffs_servers_server_id",
                table: "vip_tariffs",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_admin_tariff_groups__servers_server_id",
                table: "admin_tariff_groups");

            migrationBuilder.DropForeignKey(
                name: "f_k_admin_tariff_options_admin_tariffs_tariff_id",
                table: "admin_tariff_options");

            migrationBuilder.DropForeignKey(
                name: "f_k_admin_tariffs__servers_server_id",
                table: "admin_tariffs");

            migrationBuilder.DropForeignKey(
                name: "f_k_admin_tariffs_admin_tariff_groups_admin_tariff_group_id",
                table: "admin_tariffs");

            migrationBuilder.DropForeignKey(
                name: "f_k_blocked_ips__users_blocked_by_user_id",
                table: "blocked_ips");

            migrationBuilder.DropForeignKey(
                name: "f_k_custom_page_media_custom_pages_custom_page_id",
                table: "custom_page_media");

            migrationBuilder.DropForeignKey(
                name: "f_k_custom_page_views__users_user_id",
                table: "custom_page_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_custom_page_views_custom_pages_custom_page_id",
                table: "custom_page_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_custom_pages__users_author_id",
                table: "custom_pages");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions__servers_server_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions__users_user_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions_admin_tariffs_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions_vip_tariff_options_vip_tariff_option_~",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_donation_transactions_vip_tariffs_vip_tariff_id",
                table: "donation_transactions");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_comments__users_user_id",
                table: "event_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_comments_event_comments_parent_comment_id",
                table: "event_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_comments_events_event_id",
                table: "event_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_likes__users_user_id",
                table: "event_likes");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_likes_events_event_id",
                table: "event_likes");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_media_events_event_id",
                table: "event_media");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_views__users_user_id",
                table: "event_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_event_views_events_event_id",
                table: "event_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_events__users_author_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "f_k_news__users_author_id",
                table: "news");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_comments__users_user_id",
                table: "news_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_comments_news_comments_parent_comment_id",
                table: "news_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_comments_news_news_id",
                table: "news_comments");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_likes__users_user_id",
                table: "news_likes");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_likes_news_news_id",
                table: "news_likes");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_media_news_news_id",
                table: "news_media");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_views__users_user_id",
                table: "news_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_news_views_news_news_id",
                table: "news_views");

            migrationBuilder.DropForeignKey(
                name: "f_k_notifications__users_user_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "f_k_password_reset_tokens__users_user_id",
                table: "password_reset_tokens");

            migrationBuilder.DropForeignKey(
                name: "f_k_sourcebans_settings_servers_server_id",
                table: "sourcebans_settings");

            migrationBuilder.DropForeignKey(
                name: "f_k_telegram_subscribers__users_user_id",
                table: "telegram_subscribers");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_admin_privileges_admin_tariffs_tariff_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_admin_privileges_servers_server_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_admin_privileges_users_user_id",
                table: "user_admin_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_vip_privileges_donation_transactions_transaction_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_vip_privileges_servers_server_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_vip_privileges_users_user_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_vip_privileges_vip_tariff_options_tariff_option_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_vip_privileges_vip_tariffs_tariff_id",
                table: "user_vip_privileges");

            migrationBuilder.DropForeignKey(
                name: "f_k_vip_settings_servers_server_id",
                table: "vip_settings");

            migrationBuilder.DropForeignKey(
                name: "f_k_vip_tariff_options_vip_tariffs_tariff_id",
                table: "vip_tariff_options");

            migrationBuilder.DropForeignKey(
                name: "f_k_vip_tariffs_servers_server_id",
                table: "vip_tariffs");

            migrationBuilder.DropTable(
                name: "nav_section_items");

            migrationBuilder.DropTable(
                name: "vip_applications");

            migrationBuilder.DropTable(
                name: "nav_sections");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_yoomoney_settings",
                table: "yoomoney_settings");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_vip_tariffs",
                table: "vip_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_vip_tariff_options",
                table: "vip_tariff_options");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_vip_settings",
                table: "vip_settings");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_user_vip_privileges",
                table: "user_vip_privileges");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_user_admin_privileges",
                table: "user_admin_privileges");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_sourcebans_settings",
                table: "sourcebans_settings");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_smtp_settings",
                table: "smtp_settings");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_servers",
                table: "servers");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_password_reset_tokens",
                table: "password_reset_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_notifications",
                table: "notifications");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_news",
                table: "news");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_events",
                table: "events");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_donation_transactions",
                table: "donation_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_donation_packages",
                table: "donation_packages");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_admin_tariffs",
                table: "admin_tariffs");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_admin_tariff_options",
                table: "admin_tariff_options");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_admin_tariff_groups",
                table: "admin_tariff_groups");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_telegram_subscribers",
                table: "telegram_subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_slider_images",
                table: "slider_images");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_site_settings",
                table: "site_settings");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_news_views",
                table: "news_views");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_news_media",
                table: "news_media");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_news_likes",
                table: "news_likes");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_news_comments",
                table: "news_comments");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_event_views",
                table: "event_views");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_event_media",
                table: "event_media");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_event_likes",
                table: "event_likes");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_event_comments",
                table: "event_comments");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_custom_pages",
                table: "custom_pages");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_custom_page_views",
                table: "custom_page_views");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_custom_page_media",
                table: "custom_page_media");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_blocked_ips",
                table: "blocked_ips");

            migrationBuilder.DropColumn(
                name: "rcon_password",
                table: "servers");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "servers",
                newName: "Servers");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "news",
                newName: "News");

            migrationBuilder.RenameTable(
                name: "events",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "telegram_subscribers",
                newName: "TelegramSubscribers");

            migrationBuilder.RenameTable(
                name: "slider_images",
                newName: "SliderImages");

            migrationBuilder.RenameTable(
                name: "site_settings",
                newName: "SiteSettings");

            migrationBuilder.RenameTable(
                name: "news_views",
                newName: "NewsViews");

            migrationBuilder.RenameTable(
                name: "news_media",
                newName: "NewsMedia");

            migrationBuilder.RenameTable(
                name: "news_likes",
                newName: "NewsLikes");

            migrationBuilder.RenameTable(
                name: "news_comments",
                newName: "NewsComments");

            migrationBuilder.RenameTable(
                name: "event_views",
                newName: "EventViews");

            migrationBuilder.RenameTable(
                name: "event_media",
                newName: "EventMedia");

            migrationBuilder.RenameTable(
                name: "event_likes",
                newName: "EventLikes");

            migrationBuilder.RenameTable(
                name: "event_comments",
                newName: "EventComments");

            migrationBuilder.RenameTable(
                name: "custom_pages",
                newName: "CustomPages");

            migrationBuilder.RenameTable(
                name: "custom_page_views",
                newName: "CustomPageViews");

            migrationBuilder.RenameTable(
                name: "custom_page_media",
                newName: "CustomPageMedia");

            migrationBuilder.RenameTable(
                name: "blocked_ips",
                newName: "BlockedIps");

            migrationBuilder.RenameIndex(
                name: "i_x_vip_settings_server_id",
                table: "vip_settings",
                newName: "IX_vip_settings_server_id");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "steam_profile_url",
                table: "Users",
                newName: "SteamProfileUrl");

            migrationBuilder.RenameColumn(
                name: "steam_id",
                table: "Users",
                newName: "SteamId");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "last_ip",
                table: "Users",
                newName: "LastIp");

            migrationBuilder.RenameColumn(
                name: "is_blocked",
                table: "Users",
                newName: "IsBlocked");

            migrationBuilder.RenameColumn(
                name: "is_admin",
                table: "Users",
                newName: "IsAdmin");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "blocked_at",
                table: "Users",
                newName: "BlockedAt");

            migrationBuilder.RenameColumn(
                name: "block_reason",
                table: "Users",
                newName: "BlockReason");

            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "Users",
                newName: "AvatarUrl");

            migrationBuilder.RenameIndex(
                name: "IX_users_username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "IX_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "i_x_user_vip_privileges_transaction_id",
                table: "user_vip_privileges",
                newName: "IX_user_vip_privileges_transaction_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_vip_privileges_tariff_option_id",
                table: "user_vip_privileges",
                newName: "IX_user_vip_privileges_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_vip_privileges_tariff_id",
                table: "user_vip_privileges",
                newName: "IX_user_vip_privileges_tariff_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_vip_privileges_server_id",
                table: "user_vip_privileges",
                newName: "IX_user_vip_privileges_server_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_admin_privileges_tariff_option_id",
                table: "user_admin_privileges",
                newName: "IX_user_admin_privileges_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_admin_privileges_tariff_id",
                table: "user_admin_privileges",
                newName: "IX_user_admin_privileges_tariff_id");

            migrationBuilder.RenameIndex(
                name: "i_x_user_admin_privileges_server_id",
                table: "user_admin_privileges",
                newName: "IX_user_admin_privileges_server_id");

            migrationBuilder.RenameIndex(
                name: "i_x_sourcebans_settings_server_id",
                table: "sourcebans_settings",
                newName: "IX_sourcebans_settings_server_id");

            migrationBuilder.RenameColumn(
                name: "port",
                table: "Servers",
                newName: "Port");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Servers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Servers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "max_players",
                table: "Servers",
                newName: "MaxPlayers");

            migrationBuilder.RenameColumn(
                name: "map_name",
                table: "Servers",
                newName: "MapName");

            migrationBuilder.RenameColumn(
                name: "last_checked",
                table: "Servers",
                newName: "LastChecked");

            migrationBuilder.RenameColumn(
                name: "is_online",
                table: "Servers",
                newName: "IsOnline");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "Servers",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "current_players",
                table: "Servers",
                newName: "CurrentPlayers");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Notifications",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Notifications",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Notifications",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Notifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "related_entity_id",
                table: "Notifications",
                newName: "RelatedEntityId");

            migrationBuilder.RenameColumn(
                name: "is_read",
                table: "Notifications",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Notifications",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_notifications_user_id_is_read",
                table: "Notifications",
                newName: "IX_Notifications_UserId_IsRead");

            migrationBuilder.RenameIndex(
                name: "IX_notifications_created_at",
                table: "Notifications",
                newName: "IX_Notifications_CreatedAt");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "News",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "summary",
                table: "News",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "News",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "News",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "News",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "News",
                newName: "ViewCount");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "News",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "like_count",
                table: "News",
                newName: "LikeCount");

            migrationBuilder.RenameColumn(
                name: "is_published",
                table: "News",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "News",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cover_image",
                table: "News",
                newName: "CoverImage");

            migrationBuilder.RenameColumn(
                name: "comment_count",
                table: "News",
                newName: "CommentCount");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "News",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_news_slug",
                table: "News",
                newName: "IX_News_Slug");

            migrationBuilder.RenameIndex(
                name: "i_x_news_author_id",
                table: "News",
                newName: "IX_News_AuthorId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Events",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "summary",
                table: "Events",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Events",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Events",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Events",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "Events",
                newName: "ViewCount");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Events",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Events",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "like_count",
                table: "Events",
                newName: "LikeCount");

            migrationBuilder.RenameColumn(
                name: "is_start_notification_sent",
                table: "Events",
                newName: "IsStartNotificationSent");

            migrationBuilder.RenameColumn(
                name: "is_published",
                table: "Events",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "is_end_notification_sent",
                table: "Events",
                newName: "IsEndNotificationSent");

            migrationBuilder.RenameColumn(
                name: "is_created_notification_sent",
                table: "Events",
                newName: "IsCreatedNotificationSent");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Events",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Events",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cover_image",
                table: "Events",
                newName: "CoverImage");

            migrationBuilder.RenameColumn(
                name: "comment_count",
                table: "Events",
                newName: "CommentCount");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "Events",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_events_slug",
                table: "Events",
                newName: "IX_Events_Slug");

            migrationBuilder.RenameIndex(
                name: "i_x_events_author_id",
                table: "Events",
                newName: "IX_Events_AuthorId");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_vip_tariff_option_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_vip_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_vip_tariff_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_vip_tariff_id");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_tariff_option_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_tariff_option_id");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_tariff_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_tariff_id");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_server_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_server_id");

            migrationBuilder.RenameIndex(
                name: "i_x_donation_transactions_privilege_id",
                table: "donation_transactions",
                newName: "IX_donation_transactions_privilege_id");

            migrationBuilder.RenameColumn(
                name: "admin_tariff_group_id",
                table: "admin_tariffs",
                newName: "AdminTariffGroupId");

            migrationBuilder.RenameIndex(
                name: "i_x_admin_tariffs_admin_tariff_group_id",
                table: "admin_tariffs",
                newName: "IX_admin_tariffs_AdminTariffGroupId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "TelegramSubscribers",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TelegramSubscribers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "TelegramSubscribers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "subscribed_at",
                table: "TelegramSubscribers",
                newName: "SubscribedAt");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "TelegramSubscribers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "TelegramSubscribers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "TelegramSubscribers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "TelegramSubscribers",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_telegram_subscribers_chat_id",
                table: "TelegramSubscribers",
                newName: "IX_TelegramSubscribers_ChatId");

            migrationBuilder.RenameIndex(
                name: "i_x_telegram_subscribers_user_id",
                table: "TelegramSubscribers",
                newName: "IX_TelegramSubscribers_UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "SliderImages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "order",
                table: "SliderImages",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "SliderImages",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "buttons",
                table: "SliderImages",
                newName: "Buttons");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SliderImages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "SliderImages",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "SliderImages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "SiteSettings",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "key",
                table: "SiteSettings",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "SiteSettings",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "SiteSettings",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SiteSettings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "SiteSettings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "data_type",
                table: "SiteSettings",
                newName: "DataType");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "SiteSettings",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_site_settings_key",
                table: "SiteSettings",
                newName: "IX_SiteSettings_Key");

            migrationBuilder.RenameIndex(
                name: "IX_site_settings_category",
                table: "SiteSettings",
                newName: "IX_SiteSettings_Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewsViews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_date",
                table: "NewsViews",
                newName: "ViewDate");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "NewsViews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "NewsViews",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "NewsViews",
                newName: "IpAddress");

            migrationBuilder.RenameIndex(
                name: "IX_news_views_news_id_user_id_view_date",
                table: "NewsViews",
                newName: "IX_NewsViews_NewsId_UserId_ViewDate");

            migrationBuilder.RenameIndex(
                name: "IX_news_views_news_id_ip_address_view_date",
                table: "NewsViews",
                newName: "IX_NewsViews_NewsId_IpAddress_ViewDate");

            migrationBuilder.RenameIndex(
                name: "i_x_news_views_user_id",
                table: "NewsViews",
                newName: "IX_NewsViews_UserId");

            migrationBuilder.RenameColumn(
                name: "order",
                table: "NewsMedia",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewsMedia",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "NewsMedia",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "media_url",
                table: "NewsMedia",
                newName: "MediaUrl");

            migrationBuilder.RenameColumn(
                name: "media_type",
                table: "NewsMedia",
                newName: "MediaType");

            migrationBuilder.RenameIndex(
                name: "i_x_news_media_news_id",
                table: "NewsMedia",
                newName: "IX_NewsMedia_NewsId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewsLikes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "NewsLikes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "NewsLikes",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "NewsLikes",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_news_likes_news_id_user_id",
                table: "NewsLikes",
                newName: "IX_NewsLikes_NewsId_UserId");

            migrationBuilder.RenameIndex(
                name: "i_x_news_likes_user_id",
                table: "NewsLikes",
                newName: "IX_NewsLikes_UserId");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "NewsComments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewsComments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "NewsComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "NewsComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "parent_comment_id",
                table: "NewsComments",
                newName: "ParentCommentId");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "NewsComments",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "NewsComments",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "i_x_news_comments_user_id",
                table: "NewsComments",
                newName: "IX_NewsComments_UserId");

            migrationBuilder.RenameIndex(
                name: "i_x_news_comments_parent_comment_id",
                table: "NewsComments",
                newName: "IX_NewsComments_ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "i_x_news_comments_news_id",
                table: "NewsComments",
                newName: "IX_NewsComments_NewsId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EventViews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_date",
                table: "EventViews",
                newName: "ViewDate");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "EventViews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "EventViews",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "EventViews",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_event_views_event_id_user_id_view_date",
                table: "EventViews",
                newName: "IX_EventViews_EventId_UserId_ViewDate");

            migrationBuilder.RenameIndex(
                name: "IX_event_views_event_id_ip_address_view_date",
                table: "EventViews",
                newName: "IX_EventViews_EventId_IpAddress_ViewDate");

            migrationBuilder.RenameIndex(
                name: "i_x_event_views_user_id",
                table: "EventViews",
                newName: "IX_EventViews_UserId");

            migrationBuilder.RenameColumn(
                name: "order",
                table: "EventMedia",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EventMedia",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "media_url",
                table: "EventMedia",
                newName: "MediaUrl");

            migrationBuilder.RenameColumn(
                name: "media_type",
                table: "EventMedia",
                newName: "MediaType");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "EventMedia",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "i_x_event_media_event_id",
                table: "EventMedia",
                newName: "IX_EventMedia_EventId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EventLikes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "EventLikes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "EventLikes",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "EventLikes",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_event_likes_event_id_user_id",
                table: "EventLikes",
                newName: "IX_EventLikes_EventId_UserId");

            migrationBuilder.RenameIndex(
                name: "i_x_event_likes_user_id",
                table: "EventLikes",
                newName: "IX_EventLikes_UserId");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "EventComments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EventComments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "EventComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "EventComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "parent_comment_id",
                table: "EventComments",
                newName: "ParentCommentId");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "EventComments",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "EventComments",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "i_x_event_comments_user_id",
                table: "EventComments",
                newName: "IX_EventComments_UserId");

            migrationBuilder.RenameIndex(
                name: "i_x_event_comments_parent_comment_id",
                table: "EventComments",
                newName: "IX_EventComments_ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "i_x_event_comments_event_id",
                table: "EventComments",
                newName: "IX_EventComments_EventId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "CustomPages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "summary",
                table: "CustomPages",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "CustomPages",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "CustomPages",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CustomPages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "CustomPages",
                newName: "ViewCount");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "CustomPages",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_published",
                table: "CustomPages",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "CustomPages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cover_image",
                table: "CustomPages",
                newName: "CoverImage");

            migrationBuilder.RenameColumn(
                name: "author_id",
                table: "CustomPages",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_custom_pages_slug",
                table: "CustomPages",
                newName: "IX_CustomPages_Slug");

            migrationBuilder.RenameIndex(
                name: "i_x_custom_pages_author_id",
                table: "CustomPages",
                newName: "IX_CustomPages_AuthorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CustomPageViews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_date",
                table: "CustomPageViews",
                newName: "ViewDate");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "CustomPageViews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "CustomPageViews",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "custom_page_id",
                table: "CustomPageViews",
                newName: "CustomPageId");

            migrationBuilder.RenameIndex(
                name: "IX_custom_page_views_custom_page_id_user_id_view_date",
                table: "CustomPageViews",
                newName: "IX_CustomPageViews_CustomPageId_UserId_ViewDate");

            migrationBuilder.RenameIndex(
                name: "IX_custom_page_views_custom_page_id_ip_address_view_date",
                table: "CustomPageViews",
                newName: "IX_CustomPageViews_CustomPageId_IpAddress_ViewDate");

            migrationBuilder.RenameIndex(
                name: "i_x_custom_page_views_user_id",
                table: "CustomPageViews",
                newName: "IX_CustomPageViews_UserId");

            migrationBuilder.RenameColumn(
                name: "order",
                table: "CustomPageMedia",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CustomPageMedia",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "media_url",
                table: "CustomPageMedia",
                newName: "MediaUrl");

            migrationBuilder.RenameColumn(
                name: "media_type",
                table: "CustomPageMedia",
                newName: "MediaType");

            migrationBuilder.RenameColumn(
                name: "custom_page_id",
                table: "CustomPageMedia",
                newName: "CustomPageId");

            migrationBuilder.RenameIndex(
                name: "i_x_custom_page_media_custom_page_id",
                table: "CustomPageMedia",
                newName: "IX_CustomPageMedia_CustomPageId");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "BlockedIps",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlockedIps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "BlockedIps",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "blocked_by_user_id",
                table: "BlockedIps",
                newName: "BlockedByUserId");

            migrationBuilder.RenameColumn(
                name: "blocked_at",
                table: "BlockedIps",
                newName: "BlockedAt");

            migrationBuilder.RenameIndex(
                name: "IX_blocked_ips_ip_address",
                table: "BlockedIps",
                newName: "IX_BlockedIps_IpAddress");

            migrationBuilder.RenameIndex(
                name: "i_x_blocked_ips_blocked_by_user_id",
                table: "BlockedIps",
                newName: "IX_BlockedIps_BlockedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "wallet_number",
                table: "yoomoney_settings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "yoomoney_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "secret_key",
                table: "yoomoney_settings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "yoomoney_settings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "yoomoney_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "vip_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "vip_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "vip_tariffs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "vip_tariffs",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "vip_tariffs",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vip_tariffs",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_tariffs",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "vip_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "vip_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "vip_tariff_options",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "duration_days",
                table: "vip_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_tariff_options",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "vip_settings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "vip_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "vip_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "vip_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "vip_settings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "vip_settings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "vip_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "database",
                table: "vip_settings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "vip_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "vip_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SteamProfileUrl",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SteamId",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastIp",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBlocked",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockedAt",
                table: "Users",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BlockReason",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "user_vip_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "user_vip_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_vip_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_vip_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "user_vip_privileges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "user_vip_privileges",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "starts_at",
                table: "user_vip_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "user_vip_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "user_vip_privileges",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "user_vip_privileges",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "user_vip_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "user_vip_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_vip_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "user_admin_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "transaction_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "user_admin_privileges",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "starts_at",
                table: "user_admin_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "sourcebans_admin_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "user_admin_privileges",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "immunity",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "user_admin_privileges",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "flags",
                table: "user_admin_privileges",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "user_admin_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "user_admin_privileges",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "admin_password",
                table: "user_admin_privileges",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_admin_privileges",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "sourcebans_settings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "sourcebans_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "sourcebans_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "sourcebans_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "sourcebans_settings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "sourcebans_settings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "sourcebans_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "database",
                table: "sourcebans_settings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "sourcebans_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "sourcebans_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "smtp_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "smtp_settings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "smtp_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "smtp_settings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_configured",
                table: "smtp_settings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "host",
                table: "smtp_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "from_name",
                table: "smtp_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "from_email",
                table: "smtp_settings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "enable_ssl",
                table: "smtp_settings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "smtp_settings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Port",
                table: "Servers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Servers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Servers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "MaxPlayers",
                table: "Servers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MapName",
                table: "Servers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastChecked",
                table: "Servers",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOnline",
                table: "Servers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Servers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentPlayers",
                table: "Servers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "password_reset_tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "used_at",
                table: "password_reset_tokens",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "password_reset_tokens",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "is_used",
                table: "password_reset_tokens",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "password_reset_tokens",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "password_reset_tokens",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "password_reset_tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "RelatedEntityId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "News",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "News",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "News",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "News",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "News",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "News",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "News",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "News",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Events",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Events",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Events",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Events",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Events",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsStartNotificationSent",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEndNotificationSent",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCreatedNotificationSent",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Events",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "Events",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "vip_tariff_option_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "vip_tariff_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "donation_transactions",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "donation_transactions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_option_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "donation_transactions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "donation_transactions",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "privilege_id",
                table: "donation_transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "pending_expires_at",
                table: "donation_transactions",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_url",
                table: "donation_transactions",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_method",
                table: "donation_transactions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "label",
                table: "donation_transactions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "donation_transactions",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "donation_transactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "completed_at",
                table: "donation_transactions",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "cancelled_at",
                table: "donation_transactions",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "admin_password",
                table: "donation_transactions",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "donation_transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "donation_packages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "donation_packages",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "suggested_amounts",
                table: "donation_packages",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "donation_packages",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "donation_packages",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "donation_packages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "donation_packages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "admin_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "admin_tariffs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariffs",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "immunity",
                table: "admin_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "group_name",
                table: "admin_tariffs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "flags",
                table: "admin_tariffs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "admin_tariffs",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariffs",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariffs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "AdminTariffGroupId",
                table: "admin_tariffs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tariff_id",
                table: "admin_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "requires_password",
                table: "admin_tariff_options",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariff_options",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "duration_days",
                table: "admin_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariff_options",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariff_options",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "server_id",
                table: "admin_tariff_groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "admin_tariff_groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "admin_tariff_groups",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "admin_tariff_groups",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "admin_tariff_groups",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "admin_tariff_groups",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "admin_tariff_groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "TelegramSubscribers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TelegramSubscribers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TelegramSubscribers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubscribedAt",
                table: "TelegramSubscribers",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "TelegramSubscribers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TelegramSubscribers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "TelegramSubscribers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SliderImages",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "SliderImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SliderImages",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Buttons",
                table: "SliderImages",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SliderImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "SliderImages",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SliderImages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SiteSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "SiteSettings",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SiteSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "SiteSettings",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SiteSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SiteSettings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "SiteSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SiteSettings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NewsViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ViewDate",
                table: "NewsViews",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "NewsViews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "NewsViews",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "NewsMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NewsMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "NewsMedia",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "NewsMedia",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NewsLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "NewsLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NewsLikes",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NewsComments",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NewsComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "NewsComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsComments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "NewsComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NewsComments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ViewDate",
                table: "EventViews",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EventViews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "EventViews",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "EventMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "EventMedia",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "EventMedia",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EventLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventLikes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "EventLikes",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "EventComments",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EventComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "EventComments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "EventComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "EventComments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CustomPages",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "CustomPages",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "CustomPages",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CustomPages",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomPages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "CustomPages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomPages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "CustomPages",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomPages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "CustomPages",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "CustomPages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomPageViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ViewDate",
                table: "CustomPageViews",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CustomPageViews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "CustomPageViews",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomPageId",
                table: "CustomPageViews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "CustomPageMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomPageMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "CustomPageMedia",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "CustomPageMedia",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CustomPageId",
                table: "CustomPageMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "BlockedIps",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlockedIps",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "BlockedIps",
                type: "varchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45);

            migrationBuilder.AlterColumn<int>(
                name: "BlockedByUserId",
                table: "BlockedIps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockedAt",
                table: "BlockedIps",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_yoomoney_settings",
                table: "yoomoney_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vip_tariffs",
                table: "vip_tariffs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vip_tariff_options",
                table: "vip_tariff_options",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vip_settings",
                table: "vip_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_vip_privileges",
                table: "user_vip_privileges",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_admin_privileges",
                table: "user_admin_privileges",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sourcebans_settings",
                table: "sourcebans_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_smtp_settings",
                table: "smtp_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servers",
                table: "Servers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_password_reset_tokens",
                table: "password_reset_tokens",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_donation_transactions",
                table: "donation_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_donation_packages",
                table: "donation_packages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin_tariffs",
                table: "admin_tariffs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin_tariff_options",
                table: "admin_tariff_options",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin_tariff_groups",
                table: "admin_tariff_groups",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramSubscribers",
                table: "TelegramSubscribers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderImages",
                table: "SliderImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteSettings",
                table: "SiteSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsViews",
                table: "NewsViews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsMedia",
                table: "NewsMedia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsLikes",
                table: "NewsLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsComments",
                table: "NewsComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventViews",
                table: "EventViews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMedia",
                table: "EventMedia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventLikes",
                table: "EventLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventComments",
                table: "EventComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomPages",
                table: "CustomPages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomPageViews",
                table: "CustomPageViews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomPageMedia",
                table: "CustomPageMedia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedIps",
                table: "BlockedIps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tariff_groups_Servers_server_id",
                table: "admin_tariff_groups",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tariff_options_admin_tariffs_tariff_id",
                table: "admin_tariff_options",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tariffs_Servers_server_id",
                table: "admin_tariffs",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId",
                table: "admin_tariffs",
                column: "AdminTariffGroupId",
                principalTable: "admin_tariff_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedIps_Users_BlockedByUserId",
                table: "BlockedIps",
                column: "BlockedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPageMedia_CustomPages_CustomPageId",
                table: "CustomPageMedia",
                column: "CustomPageId",
                principalTable: "CustomPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPages_Users_AuthorId",
                table: "CustomPages",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPageViews_CustomPages_CustomPageId",
                table: "CustomPageViews",
                column: "CustomPageId",
                principalTable: "CustomPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPageViews_Users_UserId",
                table: "CustomPageViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_Servers_server_id",
                table: "donation_transactions",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_Users_user_id",
                table: "donation_transactions",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_admin_tariff_options_tariff_option_id",
                table: "donation_transactions",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_admin_tariffs_tariff_id",
                table: "donation_transactions",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_donation_transactions_user_admin_privileges_privilege_id",
                table: "donation_transactions",
                column: "privilege_id",
                principalTable: "user_admin_privileges",
                principalColumn: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EventComments_EventComments_ParentCommentId",
                table: "EventComments",
                column: "ParentCommentId",
                principalTable: "EventComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventComments_Events_EventId",
                table: "EventComments",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventComments_Users_UserId",
                table: "EventComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLikes_Events_EventId",
                table: "EventLikes",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLikes_Users_UserId",
                table: "EventLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventMedia_Events_EventId",
                table: "EventMedia",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_AuthorId",
                table: "Events",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventViews_Events_EventId",
                table: "EventViews",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventViews_Users_UserId",
                table: "EventViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Users_AuthorId",
                table: "News",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_NewsComments_ParentCommentId",
                table: "NewsComments",
                column: "ParentCommentId",
                principalTable: "NewsComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_Users_UserId",
                table: "NewsComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLikes_News_NewsId",
                table: "NewsLikes",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLikes_Users_UserId",
                table: "NewsLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsMedia_News_NewsId",
                table: "NewsMedia",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsViews_News_NewsId",
                table: "NewsViews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsViews_Users_UserId",
                table: "NewsViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_password_reset_tokens_Users_user_id",
                table: "password_reset_tokens",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sourcebans_settings_Servers_server_id",
                table: "sourcebans_settings",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramSubscribers_Users_UserId",
                table: "TelegramSubscribers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_Servers_server_id",
                table: "user_admin_privileges",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_Users_user_id",
                table: "user_admin_privileges",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_admin_tariff_options_tariff_option_id",
                table: "user_admin_privileges",
                column: "tariff_option_id",
                principalTable: "admin_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_admin_privileges_admin_tariffs_tariff_id",
                table: "user_admin_privileges",
                column: "tariff_id",
                principalTable: "admin_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_vip_privileges_Servers_server_id",
                table: "user_vip_privileges",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_vip_privileges_Users_user_id",
                table: "user_vip_privileges",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_vip_privileges_donation_transactions_transaction_id",
                table: "user_vip_privileges",
                column: "transaction_id",
                principalTable: "donation_transactions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_vip_privileges_vip_tariff_options_tariff_option_id",
                table: "user_vip_privileges",
                column: "tariff_option_id",
                principalTable: "vip_tariff_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_vip_privileges_vip_tariffs_tariff_id",
                table: "user_vip_privileges",
                column: "tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vip_settings_Servers_server_id",
                table: "vip_settings",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vip_tariff_options_vip_tariffs_tariff_id",
                table: "vip_tariff_options",
                column: "tariff_id",
                principalTable: "vip_tariffs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vip_tariffs_Servers_server_id",
                table: "vip_tariffs",
                column: "server_id",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
