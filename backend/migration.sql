CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "Servers" (
        "Id" int NOT NULL,
        "Name" varchar(100) NOT NULL,
        "IpAddress" longtext NOT NULL,
        "Port" int NOT NULL,
        "MapName" longtext NOT NULL,
        "CurrentPlayers" int NOT NULL,
        "MaxPlayers" int NOT NULL,
        "IsOnline" tinyint(1) NOT NULL,
        "LastChecked" datetime(6) NOT NULL,
        CONSTRAINT "PK_Servers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "SiteSettings" (
        "Id" int NOT NULL,
        "Key" varchar(255) NOT NULL,
        "Value" longtext NOT NULL,
        "Category" varchar(255) NOT NULL,
        "Description" longtext NOT NULL,
        "DataType" longtext NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_SiteSettings" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "SliderImages" (
        "Id" int NOT NULL,
        "ImageUrl" longtext NOT NULL,
        "Title" varchar(100) NOT NULL,
        "Description" varchar(500) NOT NULL,
        "Order" int NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_SliderImages" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "Users" (
        "Id" int NOT NULL,
        "Username" varchar(50) NOT NULL,
        "Email" varchar(255) NOT NULL,
        "PasswordHash" longtext NOT NULL,
        "AvatarUrl" longtext,
        "IsAdmin" tinyint(1) NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "News" (
        "Id" int NOT NULL,
        "Title" varchar(200) NOT NULL,
        "Content" longtext NOT NULL,
        "Summary" varchar(500),
        "Slug" varchar(200) NOT NULL,
        "CoverImage" longtext,
        "AuthorId" int NOT NULL,
        "IsPublished" tinyint(1) NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        "ViewCount" int NOT NULL,
        "LikeCount" int NOT NULL,
        "CommentCount" int NOT NULL,
        CONSTRAINT "PK_News" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_News_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "NewsComments" (
        "Id" int NOT NULL,
        "NewsId" int NOT NULL,
        "UserId" int NOT NULL,
        "Content" varchar(2000) NOT NULL,
        "ParentCommentId" int,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_NewsComments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_NewsComments_NewsComments_ParentCommentId" FOREIGN KEY ("ParentCommentId") REFERENCES "NewsComments" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_NewsComments_News_NewsId" FOREIGN KEY ("NewsId") REFERENCES "News" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_NewsComments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "NewsLikes" (
        "Id" int NOT NULL,
        "NewsId" int NOT NULL,
        "UserId" int NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_NewsLikes" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_NewsLikes_News_NewsId" FOREIGN KEY ("NewsId") REFERENCES "News" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_NewsLikes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE TABLE "NewsMedia" (
        "Id" int NOT NULL,
        "NewsId" int NOT NULL,
        "MediaUrl" longtext NOT NULL,
        "MediaType" varchar(20) NOT NULL,
        "Order" int NOT NULL,
        CONSTRAINT "PK_NewsMedia" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_NewsMedia_News_NewsId" FOREIGN KEY ("NewsId") REFERENCES "News" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_News_AuthorId" ON "News" ("AuthorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE UNIQUE INDEX "IX_News_Slug" ON "News" ("Slug");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_NewsComments_NewsId" ON "NewsComments" ("NewsId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_NewsComments_ParentCommentId" ON "NewsComments" ("ParentCommentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_NewsComments_UserId" ON "NewsComments" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE UNIQUE INDEX "IX_NewsLikes_NewsId_UserId" ON "NewsLikes" ("NewsId", "UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_NewsLikes_UserId" ON "NewsLikes" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_NewsMedia_NewsId" ON "NewsMedia" ("NewsId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE INDEX "IX_SiteSettings_Category" ON "SiteSettings" ("Category");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE UNIQUE INDEX "IX_SiteSettings_Key" ON "SiteSettings" ("Key");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024105710_InitialMySqlMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251024105710_InitialMySqlMigration', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE TABLE "Events" (
        "Id" int NOT NULL,
        "Title" varchar(200) NOT NULL,
        "Content" longtext NOT NULL,
        "Summary" varchar(500),
        "Slug" varchar(200) NOT NULL,
        "CoverImage" longtext,
        "AuthorId" int NOT NULL,
        "IsPublished" tinyint(1) NOT NULL,
        "StartDate" datetime(6) NOT NULL,
        "EndDate" datetime(6) NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        "ViewCount" int NOT NULL,
        "LikeCount" int NOT NULL,
        "CommentCount" int NOT NULL,
        CONSTRAINT "PK_Events" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Events_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE TABLE "EventComments" (
        "Id" int NOT NULL,
        "EventId" int NOT NULL,
        "UserId" int NOT NULL,
        "Content" varchar(2000) NOT NULL,
        "ParentCommentId" int,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_EventComments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_EventComments_EventComments_ParentCommentId" FOREIGN KEY ("ParentCommentId") REFERENCES "EventComments" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_EventComments_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_EventComments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE TABLE "EventLikes" (
        "Id" int NOT NULL,
        "EventId" int NOT NULL,
        "UserId" int NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_EventLikes" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_EventLikes_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_EventLikes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE TABLE "EventMedia" (
        "Id" int NOT NULL,
        "EventId" int NOT NULL,
        "MediaUrl" longtext NOT NULL,
        "MediaType" varchar(20) NOT NULL,
        "Order" int NOT NULL,
        CONSTRAINT "PK_EventMedia" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_EventMedia_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_EventComments_EventId" ON "EventComments" ("EventId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_EventComments_ParentCommentId" ON "EventComments" ("ParentCommentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_EventComments_UserId" ON "EventComments" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE UNIQUE INDEX "IX_EventLikes_EventId_UserId" ON "EventLikes" ("EventId", "UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_EventLikes_UserId" ON "EventLikes" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_EventMedia_EventId" ON "EventMedia" ("EventId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE INDEX "IX_Events_AuthorId" ON "Events" ("AuthorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    CREATE UNIQUE INDEX "IX_Events_Slug" ON "Events" ("Slug");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024112832_AddEvents') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251024112832_AddEvents', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024225237_AddSteamFields') THEN
    ALTER TABLE "Users" ADD "SteamId" varchar(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024225237_AddSteamFields') THEN
    ALTER TABLE "Users" ADD "SteamProfileUrl" longtext;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024225237_AddSteamFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251024225237_AddSteamFields', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE TABLE "EventViews" (
        "Id" int NOT NULL,
        "EventId" int NOT NULL,
        "UserId" int,
        "IpAddress" varchar(45),
        "ViewDate" datetime(6) NOT NULL,
        CONSTRAINT "PK_EventViews" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_EventViews_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_EventViews_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE TABLE "NewsViews" (
        "Id" int NOT NULL,
        "NewsId" int NOT NULL,
        "UserId" int,
        "IpAddress" varchar(45),
        "ViewDate" datetime(6) NOT NULL,
        CONSTRAINT "PK_NewsViews" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_NewsViews_News_NewsId" FOREIGN KEY ("NewsId") REFERENCES "News" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_NewsViews_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_EventViews_EventId_IpAddress_ViewDate" ON "EventViews" ("EventId", "IpAddress", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_EventViews_EventId_UserId_ViewDate" ON "EventViews" ("EventId", "UserId", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_EventViews_UserId" ON "EventViews" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_NewsViews_NewsId_IpAddress_ViewDate" ON "NewsViews" ("NewsId", "IpAddress", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_NewsViews_NewsId_UserId_ViewDate" ON "NewsViews" ("NewsId", "UserId", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    CREATE INDEX "IX_NewsViews_UserId" ON "NewsViews" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024230913_AddViewTracking') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251024230913_AddViewTracking', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024235205_AddEmailSystem') THEN
    CREATE TABLE password_reset_tokens (
        id int NOT NULL,
        token varchar(500) NOT NULL,
        user_id int NOT NULL,
        expires_at datetime(6) NOT NULL,
        is_used tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        used_at datetime(6),
        CONSTRAINT "PK_password_reset_tokens" PRIMARY KEY (id),
        CONSTRAINT "FK_password_reset_tokens_Users_user_id" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024235205_AddEmailSystem') THEN
    CREATE TABLE smtp_settings (
        id int NOT NULL,
        host varchar(255) NOT NULL,
        port int NOT NULL,
        username varchar(255) NOT NULL,
        password varchar(500) NOT NULL,
        enable_ssl tinyint(1) NOT NULL,
        from_email varchar(255) NOT NULL,
        from_name varchar(255) NOT NULL,
        is_configured tinyint(1) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_smtp_settings" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024235205_AddEmailSystem') THEN
    CREATE INDEX "IX_password_reset_tokens_token" ON password_reset_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024235205_AddEmailSystem') THEN
    CREATE INDEX "IX_password_reset_tokens_user_id_is_used" ON password_reset_tokens (user_id, is_used);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251024235205_AddEmailSystem') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251024235205_AddEmailSystem', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE admin_tariffs (
        id int NOT NULL,
        server_id int NOT NULL,
        name varchar(100) NOT NULL,
        description varchar(1000) NOT NULL,
        duration_days int NOT NULL,
        price decimal(10,2) NOT NULL,
        flags varchar(100),
        group_name varchar(100),
        immunity int NOT NULL,
        is_active tinyint(1) NOT NULL,
        "order" int NOT NULL,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_admin_tariffs" PRIMARY KEY (id),
        CONSTRAINT "FK_admin_tariffs_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE donation_packages (
        id int NOT NULL,
        title varchar(200) NOT NULL,
        description varchar(1000) NOT NULL,
        suggested_amounts varchar(500),
        is_active tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_donation_packages" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE sourcebans_settings (
        id int NOT NULL,
        host varchar(255) NOT NULL,
        port int NOT NULL,
        database varchar(100) NOT NULL,
        username varchar(100) NOT NULL,
        password varchar(500) NOT NULL,
        is_configured tinyint(1) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_sourcebans_settings" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE yoomoney_settings (
        id int NOT NULL,
        wallet_number varchar(100) NOT NULL,
        secret_key varchar(500) NOT NULL,
        is_configured tinyint(1) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_yoomoney_settings" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE donation_transactions (
        id int NOT NULL,
        transaction_id varchar(255) NOT NULL,
        user_id int,
        steam_id varchar(50),
        amount decimal(10,2) NOT NULL,
        type varchar(20) NOT NULL,
        tariff_id int,
        server_id int,
        status varchar(20) NOT NULL,
        payment_method varchar(50),
        label varchar(255),
        expires_at datetime(6),
        created_at datetime(6) NOT NULL,
        completed_at datetime(6),
        CONSTRAINT "PK_donation_transactions" PRIMARY KEY (id),
        CONSTRAINT "FK_donation_transactions_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id"),
        CONSTRAINT "FK_donation_transactions_Users_user_id" FOREIGN KEY (user_id) REFERENCES "Users" ("Id"),
        CONSTRAINT "FK_donation_transactions_admin_tariffs_tariff_id" FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE TABLE user_admin_privileges (
        id int NOT NULL,
        user_id int NOT NULL,
        steam_id varchar(50) NOT NULL,
        server_id int NOT NULL,
        tariff_id int NOT NULL,
        transaction_id int NOT NULL,
        flags varchar(100),
        group_name varchar(100),
        immunity int NOT NULL,
        starts_at datetime(6) NOT NULL,
        expires_at datetime(6) NOT NULL,
        is_active tinyint(1) NOT NULL,
        sourcebans_admin_id int,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_user_admin_privileges" PRIMARY KEY (id),
        CONSTRAINT "FK_user_admin_privileges_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_user_admin_privileges_Users_user_id" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_user_admin_privileges_admin_tariffs_tariff_id" FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id) ON DELETE CASCADE,
        CONSTRAINT "FK_user_admin_privileges_donation_transactions_transaction_id" FOREIGN KEY (transaction_id) REFERENCES donation_transactions (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_admin_tariffs_server_id_is_active" ON admin_tariffs (server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_donation_transactions_server_id" ON donation_transactions (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_donation_transactions_steam_id" ON donation_transactions (steam_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_donation_transactions_tariff_id" ON donation_transactions (tariff_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE UNIQUE INDEX "IX_donation_transactions_transaction_id" ON donation_transactions (transaction_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_donation_transactions_user_id_status" ON donation_transactions (user_id, status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_expires_at" ON user_admin_privileges (expires_at);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_server_id" ON user_admin_privileges (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_steam_id_server_id_is_active" ON user_admin_privileges (steam_id, server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_tariff_id" ON user_admin_privileges (tariff_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_transaction_id" ON user_admin_privileges (transaction_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_user_id_server_id_is_active" ON user_admin_privileges (user_id, server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025082943_AddDonationSystem') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025082943_AddDonationSystem', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025085648_UpdateSourceBansForMultiServer') THEN
    ALTER TABLE sourcebans_settings ADD created_at datetime(6) NOT NULL DEFAULT TIMESTAMP '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025085648_UpdateSourceBansForMultiServer') THEN
    ALTER TABLE sourcebans_settings ADD server_id int NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025085648_UpdateSourceBansForMultiServer') THEN
    CREATE INDEX "IX_sourcebans_settings_server_id" ON sourcebans_settings (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025085648_UpdateSourceBansForMultiServer') THEN
    ALTER TABLE sourcebans_settings ADD CONSTRAINT "FK_sourcebans_settings_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025085648_UpdateSourceBansForMultiServer') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025085648_UpdateSourceBansForMultiServer', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025091415_AddPendingPaymentTracking') THEN
    ALTER TABLE donation_transactions ADD cancelled_at datetime(6);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025091415_AddPendingPaymentTracking') THEN
    ALTER TABLE donation_transactions ADD payment_url varchar(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025091415_AddPendingPaymentTracking') THEN
    ALTER TABLE donation_transactions ADD pending_expires_at datetime(6);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025091415_AddPendingPaymentTracking') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025091415_AddPendingPaymentTracking', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE admin_tariffs DROP COLUMN duration_days;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE admin_tariffs DROP COLUMN price;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE user_admin_privileges ADD tariff_option_id int NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE donation_transactions ADD tariff_option_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE admin_tariffs ADD "AdminTariffGroupId" int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE TABLE admin_tariff_groups (
        id int NOT NULL,
        server_id int NOT NULL,
        name varchar(100) NOT NULL,
        description varchar(1000) NOT NULL,
        "order" int NOT NULL,
        is_active tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_admin_tariff_groups" PRIMARY KEY (id),
        CONSTRAINT "FK_admin_tariff_groups_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE TABLE admin_tariff_options (
        id int NOT NULL,
        tariff_id int NOT NULL,
        duration_days int NOT NULL,
        price decimal(10,2) NOT NULL,
        "order" int NOT NULL,
        is_active tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_admin_tariff_options" PRIMARY KEY (id),
        CONSTRAINT "FK_admin_tariff_options_admin_tariffs_tariff_id" FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE INDEX "IX_user_admin_privileges_tariff_option_id" ON user_admin_privileges (tariff_option_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE INDEX "IX_donation_transactions_tariff_option_id" ON donation_transactions (tariff_option_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE INDEX "IX_admin_tariffs_AdminTariffGroupId" ON admin_tariffs ("AdminTariffGroupId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE INDEX "IX_admin_tariff_groups_server_id_is_active" ON admin_tariff_groups (server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    CREATE INDEX "IX_admin_tariff_options_tariff_id_is_active" ON admin_tariff_options (tariff_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE admin_tariffs ADD CONSTRAINT "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId" FOREIGN KEY ("AdminTariffGroupId") REFERENCES admin_tariff_groups (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_admin_tariff_options_tariff_option_id" FOREIGN KEY (tariff_option_id) REFERENCES admin_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT "FK_user_admin_privileges_admin_tariff_options_tariff_option_id" FOREIGN KEY (tariff_option_id) REFERENCES admin_tariff_options (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025104302_AddTariffOptionsSystem') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025104302_AddTariffOptionsSystem', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025110034_AddAdminPasswordFields') THEN
    ALTER TABLE user_admin_privileges ADD admin_password varchar(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025110034_AddAdminPasswordFields') THEN
    ALTER TABLE donation_transactions ADD admin_password varchar(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025110034_AddAdminPasswordFields') THEN
    ALTER TABLE admin_tariff_options ADD requires_password tinyint(1) NOT NULL DEFAULT TRUE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025110034_AddAdminPasswordFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025110034_AddAdminPasswordFields', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025112405_AddNotifications') THEN
    CREATE TABLE "Notifications" (
        "Id" int NOT NULL,
        "UserId" int NOT NULL,
        "Title" varchar(200) NOT NULL,
        "Message" varchar(1000) NOT NULL,
        "Type" varchar(50) NOT NULL,
        "IsRead" tinyint(1) NOT NULL,
        "RelatedEntityId" int,
        "CreatedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025112405_AddNotifications') THEN
    CREATE INDEX "IX_Notifications_CreatedAt" ON "Notifications" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025112405_AddNotifications') THEN
    CREATE INDEX "IX_Notifications_UserId_IsRead" ON "Notifications" ("UserId", "IsRead");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025112405_AddNotifications') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025112405_AddNotifications', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_admin_tariff_options_tariff_option_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_donation_transactions_transaction_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN transaction_id DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN tariff_option_id DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges ADD updated_at datetime(6) NOT NULL DEFAULT TIMESTAMP '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT "FK_user_admin_privileges_admin_tariff_options_tariff_option_id" FOREIGN KEY (tariff_option_id) REFERENCES admin_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT "FK_user_admin_privileges_donation_transactions_transaction_id" FOREIGN KEY (transaction_id) REFERENCES donation_transactions (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025122707_AddUpdatedAtToUserAdminPrivileges', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN
    ALTER TABLE donation_transactions ADD privilege_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN
    CREATE INDEX "IX_donation_transactions_privilege_id" ON donation_transactions (privilege_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_user_admin_privileges_privilege_id" FOREIGN KEY (privilege_id) REFERENCES user_admin_privileges (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251025203310_AddPrivilegeIdToDonationTransaction', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_user_admin_privileges_privilege_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    DROP INDEX "IX_donation_transactions_privilege_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE TABLE vip_settings (
        id int NOT NULL,
        server_id int NOT NULL,
        host varchar(255) NOT NULL,
        port int NOT NULL,
        database varchar(100) NOT NULL,
        username varchar(100) NOT NULL,
        password varchar(500) NOT NULL,
        is_configured tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_vip_settings" PRIMARY KEY (id),
        CONSTRAINT "FK_vip_settings_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE TABLE vip_tariffs (
        id int NOT NULL,
        server_id int NOT NULL,
        name varchar(100) NOT NULL,
        description varchar(1000) NOT NULL,
        group_name varchar(64) NOT NULL,
        is_active tinyint(1) NOT NULL,
        "order" int NOT NULL,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_vip_tariffs" PRIMARY KEY (id),
        CONSTRAINT "FK_vip_tariffs_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE TABLE vip_tariff_options (
        id int NOT NULL,
        tariff_id int NOT NULL,
        duration_days int NOT NULL,
        price decimal(10,2) NOT NULL,
        "order" int NOT NULL,
        is_active tinyint(1) NOT NULL,
        created_at datetime(6) NOT NULL,
        CONSTRAINT "PK_vip_tariff_options" PRIMARY KEY (id),
        CONSTRAINT "FK_vip_tariff_options_vip_tariffs_tariff_id" FOREIGN KEY (tariff_id) REFERENCES vip_tariffs (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE TABLE user_vip_privileges (
        id int NOT NULL,
        user_id int NOT NULL,
        steam_id varchar(50) NOT NULL,
        server_id int NOT NULL,
        tariff_id int NOT NULL,
        tariff_option_id int,
        group_name varchar(64) NOT NULL,
        starts_at datetime(6) NOT NULL,
        expires_at datetime(6) NOT NULL,
        is_active tinyint(1) NOT NULL,
        transaction_id int,
        created_at datetime(6) NOT NULL,
        updated_at datetime(6) NOT NULL,
        CONSTRAINT "PK_user_vip_privileges" PRIMARY KEY (id),
        CONSTRAINT "FK_user_vip_privileges_Servers_server_id" FOREIGN KEY (server_id) REFERENCES "Servers" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_user_vip_privileges_Users_user_id" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_user_vip_privileges_donation_transactions_transaction_id" FOREIGN KEY (transaction_id) REFERENCES donation_transactions (id),
        CONSTRAINT "FK_user_vip_privileges_vip_tariff_options_tariff_option_id" FOREIGN KEY (tariff_option_id) REFERENCES vip_tariff_options (id),
        CONSTRAINT "FK_user_vip_privileges_vip_tariffs_tariff_id" FOREIGN KEY (tariff_id) REFERENCES vip_tariffs (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_donation_transactions_privilege_id" ON donation_transactions (privilege_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_user_admin_privileges_privilege_id" FOREIGN KEY (privilege_id) REFERENCES user_admin_privileges (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_expires_at" ON user_vip_privileges (expires_at);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_server_id" ON user_vip_privileges (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_steam_id_server_id_is_active" ON user_vip_privileges (steam_id, server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_tariff_id" ON user_vip_privileges (tariff_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_tariff_option_id" ON user_vip_privileges (tariff_option_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_transaction_id" ON user_vip_privileges (transaction_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_user_vip_privileges_user_id_server_id_is_active" ON user_vip_privileges (user_id, server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_vip_settings_server_id" ON vip_settings (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_vip_tariff_options_tariff_id_is_active" ON vip_tariff_options (tariff_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    CREATE INDEX "IX_vip_tariffs_server_id_is_active" ON vip_tariffs (server_id, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251031230413_AddVipSystem') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251031230413_AddVipSystem', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105143712_AddBlockedIpAndUserLastIp') THEN
    ALTER TABLE "Users" ADD "LastIp" longtext;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105143712_AddBlockedIpAndUserLastIp') THEN
    CREATE TABLE "BlockedIps" (
        "Id" int NOT NULL,
        "IpAddress" varchar(45) NOT NULL,
        "Reason" longtext,
        "BlockedAt" datetime(6) NOT NULL,
        "BlockedByUserId" int,
        CONSTRAINT "PK_BlockedIps" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_BlockedIps_Users_BlockedByUserId" FOREIGN KEY ("BlockedByUserId") REFERENCES "Users" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105143712_AddBlockedIpAndUserLastIp') THEN
    CREATE INDEX "IX_BlockedIps_BlockedByUserId" ON "BlockedIps" ("BlockedByUserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105143712_AddBlockedIpAndUserLastIp') THEN
    CREATE UNIQUE INDEX "IX_BlockedIps_IpAddress" ON "BlockedIps" ("IpAddress");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105143712_AddBlockedIpAndUserLastIp') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251105143712_AddBlockedIpAndUserLastIp', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105150159_AddUserBlockingFields') THEN
    ALTER TABLE "Users" ADD "BlockReason" longtext;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105150159_AddUserBlockingFields') THEN
    ALTER TABLE "Users" ADD "BlockedAt" datetime(6);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105150159_AddUserBlockingFields') THEN
    ALTER TABLE "Users" ADD "IsBlocked" tinyint(1) NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251105150159_AddUserBlockingFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251105150159_AddUserBlockingFields', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123113942_AddButtonsToSliderImage') THEN
    ALTER TABLE "SliderImages" ADD "Buttons" longtext;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123113942_AddButtonsToSliderImage') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123113942_AddButtonsToSliderImage', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123125101_AddTelegramSubscriber') THEN
    CREATE TABLE "TelegramSubscribers" (
        "Id" int NOT NULL,
        "ChatId" bigint NOT NULL,
        "UserId" int,
        "Username" longtext,
        "FirstName" longtext,
        "LastName" longtext,
        "IsActive" tinyint(1) NOT NULL,
        "SubscribedAt" datetime(6) NOT NULL,
        CONSTRAINT "PK_TelegramSubscribers" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_TelegramSubscribers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123125101_AddTelegramSubscriber') THEN
    CREATE UNIQUE INDEX "IX_TelegramSubscribers_ChatId" ON "TelegramSubscribers" ("ChatId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123125101_AddTelegramSubscriber') THEN
    CREATE INDEX "IX_TelegramSubscribers_UserId" ON "TelegramSubscribers" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123125101_AddTelegramSubscriber') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123125101_AddTelegramSubscriber', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123135210_AddEventNotificationFields') THEN
    ALTER TABLE "Events" ADD "IsEndNotificationSent" tinyint(1) NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123135210_AddEventNotificationFields') THEN
    ALTER TABLE "Events" ADD "IsStartNotificationSent" tinyint(1) NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123135210_AddEventNotificationFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123135210_AddEventNotificationFields', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    CREATE TABLE "CustomPages" (
        "Id" int NOT NULL,
        "Title" varchar(200) NOT NULL,
        "Content" longtext NOT NULL,
        "Summary" varchar(500),
        "Slug" varchar(200) NOT NULL,
        "CoverImage" longtext,
        "AuthorId" int NOT NULL,
        "IsPublished" tinyint(1) NOT NULL,
        "CreatedAt" datetime(6) NOT NULL,
        "UpdatedAt" datetime(6) NOT NULL,
        "ViewCount" int NOT NULL,
        CONSTRAINT "PK_CustomPages" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_CustomPages_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    CREATE TABLE "CustomPageMedia" (
        "Id" int NOT NULL,
        "CustomPageId" int NOT NULL,
        "MediaUrl" longtext NOT NULL,
        "MediaType" varchar(20) NOT NULL,
        "Order" int NOT NULL,
        CONSTRAINT "PK_CustomPageMedia" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_CustomPageMedia_CustomPages_CustomPageId" FOREIGN KEY ("CustomPageId") REFERENCES "CustomPages" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    CREATE INDEX "IX_CustomPageMedia_CustomPageId" ON "CustomPageMedia" ("CustomPageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    CREATE INDEX "IX_CustomPages_AuthorId" ON "CustomPages" ("AuthorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    CREATE UNIQUE INDEX "IX_CustomPages_Slug" ON "CustomPages" ("Slug");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142328_AddCustomPages') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123142328_AddCustomPages', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142737_AddCustomPageViews') THEN
    CREATE TABLE "CustomPageViews" (
        "Id" int NOT NULL,
        "CustomPageId" int NOT NULL,
        "UserId" int,
        "IpAddress" varchar(45),
        "ViewDate" datetime(6) NOT NULL,
        CONSTRAINT "PK_CustomPageViews" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_CustomPageViews_CustomPages_CustomPageId" FOREIGN KEY ("CustomPageId") REFERENCES "CustomPages" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_CustomPageViews_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142737_AddCustomPageViews') THEN
    CREATE INDEX "IX_CustomPageViews_CustomPageId_IpAddress_ViewDate" ON "CustomPageViews" ("CustomPageId", "IpAddress", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142737_AddCustomPageViews') THEN
    CREATE INDEX "IX_CustomPageViews_CustomPageId_UserId_ViewDate" ON "CustomPageViews" ("CustomPageId", "UserId", "ViewDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142737_AddCustomPageViews') THEN
    CREATE INDEX "IX_CustomPageViews_UserId" ON "CustomPageViews" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123142737_AddCustomPageViews') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123142737_AddCustomPageViews', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    ALTER TABLE donation_transactions ADD vip_tariff_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    ALTER TABLE donation_transactions ADD vip_tariff_option_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    CREATE INDEX "IX_donation_transactions_vip_tariff_id" ON donation_transactions (vip_tariff_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    CREATE INDEX "IX_donation_transactions_vip_tariff_option_id" ON donation_transactions (vip_tariff_option_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id" FOREIGN KEY (vip_tariff_option_id) REFERENCES vip_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_vip_tariffs_vip_tariff_id" FOREIGN KEY (vip_tariff_id) REFERENCES vip_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127171334_AddVipFieldsToDonationTransactions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251127171334_AddVipFieldsToDonationTransactions', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    ALTER TABLE "Events" ADD "IsCreatedNotificationSent" tinyint(1) NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    ALTER TABLE donation_transactions ADD vip_tariff_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    ALTER TABLE donation_transactions ADD vip_tariff_option_id int;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    CREATE INDEX "IX_donation_transactions_vip_tariff_id" ON donation_transactions (vip_tariff_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    CREATE INDEX "IX_donation_transactions_vip_tariff_option_id" ON donation_transactions (vip_tariff_option_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id" FOREIGN KEY (vip_tariff_option_id) REFERENCES vip_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "FK_donation_transactions_vip_tariffs_vip_tariff_id" FOREIGN KEY (vip_tariff_id) REFERENCES vip_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127193135_AddEventCreatedNotificationSent') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251127193135_AddEventCreatedNotificationSent', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251127215010_ConvertEventDatesToUtc') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251127215010_ConvertEventDatesToUtc', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups DROP CONSTRAINT "FK_admin_tariff_groups_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options DROP CONSTRAINT "FK_admin_tariff_options_admin_tariffs_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs DROP CONSTRAINT "FK_admin_tariffs_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs DROP CONSTRAINT "FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "BlockedIps" DROP CONSTRAINT "FK_BlockedIps_Users_BlockedByUserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageMedia" DROP CONSTRAINT "FK_CustomPageMedia_CustomPages_CustomPageId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPages" DROP CONSTRAINT "FK_CustomPages_Users_AuthorId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageViews" DROP CONSTRAINT "FK_CustomPageViews_CustomPages_CustomPageId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageViews" DROP CONSTRAINT "FK_CustomPageViews_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_Users_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_admin_tariff_options_tariff_option_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_admin_tariffs_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_user_admin_privileges_privilege_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_vip_tariff_options_vip_tariff_option_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "FK_donation_transactions_vip_tariffs_vip_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventComments" DROP CONSTRAINT "FK_EventComments_EventComments_ParentCommentId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventComments" DROP CONSTRAINT "FK_EventComments_Events_EventId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventComments" DROP CONSTRAINT "FK_EventComments_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventLikes" DROP CONSTRAINT "FK_EventLikes_Events_EventId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventLikes" DROP CONSTRAINT "FK_EventLikes_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventMedia" DROP CONSTRAINT "FK_EventMedia_Events_EventId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Events" DROP CONSTRAINT "FK_Events_Users_AuthorId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventViews" DROP CONSTRAINT "FK_EventViews_Events_EventId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventViews" DROP CONSTRAINT "FK_EventViews_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "News" DROP CONSTRAINT "FK_News_Users_AuthorId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsComments" DROP CONSTRAINT "FK_NewsComments_NewsComments_ParentCommentId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsComments" DROP CONSTRAINT "FK_NewsComments_News_NewsId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsComments" DROP CONSTRAINT "FK_NewsComments_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsLikes" DROP CONSTRAINT "FK_NewsLikes_News_NewsId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsLikes" DROP CONSTRAINT "FK_NewsLikes_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsMedia" DROP CONSTRAINT "FK_NewsMedia_News_NewsId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsViews" DROP CONSTRAINT "FK_NewsViews_News_NewsId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsViews" DROP CONSTRAINT "FK_NewsViews_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Notifications" DROP CONSTRAINT "FK_Notifications_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens DROP CONSTRAINT "FK_password_reset_tokens_Users_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings DROP CONSTRAINT "FK_sourcebans_settings_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "TelegramSubscribers" DROP CONSTRAINT "FK_TelegramSubscribers_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_Users_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_admin_tariff_options_tariff_option_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "FK_user_admin_privileges_admin_tariffs_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "FK_user_vip_privileges_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "FK_user_vip_privileges_Users_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "FK_user_vip_privileges_donation_transactions_transaction_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "FK_user_vip_privileges_vip_tariff_options_tariff_option_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "FK_user_vip_privileges_vip_tariffs_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings DROP CONSTRAINT "FK_vip_settings_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options DROP CONSTRAINT "FK_vip_tariff_options_vip_tariffs_tariff_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs DROP CONSTRAINT "FK_vip_tariffs_Servers_server_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings DROP CONSTRAINT "PK_yoomoney_settings";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs DROP CONSTRAINT "PK_vip_tariffs";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options DROP CONSTRAINT "PK_vip_tariff_options";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings DROP CONSTRAINT "PK_vip_settings";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Users" DROP CONSTRAINT "PK_Users";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges DROP CONSTRAINT "PK_user_vip_privileges";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges DROP CONSTRAINT "PK_user_admin_privileges";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings DROP CONSTRAINT "PK_sourcebans_settings";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings DROP CONSTRAINT "PK_smtp_settings";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Servers" DROP CONSTRAINT "PK_Servers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens DROP CONSTRAINT "PK_password_reset_tokens";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Notifications" DROP CONSTRAINT "PK_Notifications";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "News" DROP CONSTRAINT "PK_News";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Events" DROP CONSTRAINT "PK_Events";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions DROP CONSTRAINT "PK_donation_transactions";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages DROP CONSTRAINT "PK_donation_packages";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs DROP CONSTRAINT "PK_admin_tariffs";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options DROP CONSTRAINT "PK_admin_tariff_options";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups DROP CONSTRAINT "PK_admin_tariff_groups";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "TelegramSubscribers" DROP CONSTRAINT "PK_TelegramSubscribers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "SliderImages" DROP CONSTRAINT "PK_SliderImages";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "SiteSettings" DROP CONSTRAINT "PK_SiteSettings";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsViews" DROP CONSTRAINT "PK_NewsViews";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsMedia" DROP CONSTRAINT "PK_NewsMedia";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsLikes" DROP CONSTRAINT "PK_NewsLikes";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsComments" DROP CONSTRAINT "PK_NewsComments";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventViews" DROP CONSTRAINT "PK_EventViews";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventMedia" DROP CONSTRAINT "PK_EventMedia";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventLikes" DROP CONSTRAINT "PK_EventLikes";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventComments" DROP CONSTRAINT "PK_EventComments";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageViews" DROP CONSTRAINT "PK_CustomPageViews";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPages" DROP CONSTRAINT "PK_CustomPages";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageMedia" DROP CONSTRAINT "PK_CustomPageMedia";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "BlockedIps" DROP CONSTRAINT "PK_BlockedIps";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Users" RENAME TO users;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Servers" RENAME TO servers;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Notifications" RENAME TO notifications;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "News" RENAME TO news;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "Events" RENAME TO events;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "TelegramSubscribers" RENAME TO telegram_subscribers;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "SliderImages" RENAME TO slider_images;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "SiteSettings" RENAME TO site_settings;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsViews" RENAME TO news_views;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsMedia" RENAME TO news_media;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsLikes" RENAME TO news_likes;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "NewsComments" RENAME TO news_comments;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventViews" RENAME TO event_views;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventMedia" RENAME TO event_media;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventLikes" RENAME TO event_likes;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "EventComments" RENAME TO event_comments;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageViews" RENAME TO custom_page_views;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPages" RENAME TO custom_pages;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "CustomPageMedia" RENAME TO custom_page_media;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE "BlockedIps" RENAME TO blocked_ips;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_vip_settings_server_id" RENAME TO i_x_vip_settings_server_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "Username" TO username;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "Email" TO email;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "SteamProfileUrl" TO steam_profile_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "SteamId" TO steam_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "PasswordHash" TO password_hash;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "LastIp" TO last_ip;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "IsBlocked" TO is_blocked;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "IsAdmin" TO is_admin;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "BlockedAt" TO blocked_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "BlockReason" TO block_reason;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users RENAME COLUMN "AvatarUrl" TO avatar_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Users_Username" RENAME TO "IX_users_username";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Users_Email" RENAME TO "IX_users_email";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_vip_privileges_transaction_id" RENAME TO i_x_user_vip_privileges_transaction_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_vip_privileges_tariff_option_id" RENAME TO i_x_user_vip_privileges_tariff_option_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_vip_privileges_tariff_id" RENAME TO i_x_user_vip_privileges_tariff_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_vip_privileges_server_id" RENAME TO i_x_user_vip_privileges_server_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_admin_privileges_tariff_option_id" RENAME TO i_x_user_admin_privileges_tariff_option_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_admin_privileges_tariff_id" RENAME TO i_x_user_admin_privileges_tariff_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_user_admin_privileges_server_id" RENAME TO i_x_user_admin_privileges_server_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_sourcebans_settings_server_id" RENAME TO i_x_sourcebans_settings_server_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "Port" TO port;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "Name" TO name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "MaxPlayers" TO max_players;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "MapName" TO map_name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "LastChecked" TO last_checked;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "IsOnline" TO is_online;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "IpAddress" TO ip_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers RENAME COLUMN "CurrentPlayers" TO current_players;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "Type" TO type;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "Title" TO title;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "Message" TO message;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "RelatedEntityId" TO related_entity_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "IsRead" TO is_read;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Notifications_UserId_IsRead" RENAME TO "IX_notifications_user_id_is_read";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Notifications_CreatedAt" RENAME TO "IX_notifications_created_at";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "Title" TO title;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "Summary" TO summary;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "Slug" TO slug;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "Content" TO content;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "ViewCount" TO view_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "LikeCount" TO like_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "IsPublished" TO is_published;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "CoverImage" TO cover_image;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "CommentCount" TO comment_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news RENAME COLUMN "AuthorId" TO author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_News_Slug" RENAME TO "IX_news_slug";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_News_AuthorId" RENAME TO i_x_news_author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "Title" TO title;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "Summary" TO summary;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "Slug" TO slug;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "Content" TO content;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "ViewCount" TO view_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "StartDate" TO start_date;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "LikeCount" TO like_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "IsStartNotificationSent" TO is_start_notification_sent;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "IsPublished" TO is_published;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "IsEndNotificationSent" TO is_end_notification_sent;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "IsCreatedNotificationSent" TO is_created_notification_sent;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "EndDate" TO end_date;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "CoverImage" TO cover_image;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "CommentCount" TO comment_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events RENAME COLUMN "AuthorId" TO author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Events_Slug" RENAME TO "IX_events_slug";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_Events_AuthorId" RENAME TO i_x_events_author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_vip_tariff_option_id" RENAME TO i_x_donation_transactions_vip_tariff_option_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_vip_tariff_id" RENAME TO i_x_donation_transactions_vip_tariff_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_tariff_option_id" RENAME TO i_x_donation_transactions_tariff_option_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_tariff_id" RENAME TO i_x_donation_transactions_tariff_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_server_id" RENAME TO i_x_donation_transactions_server_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_donation_transactions_privilege_id" RENAME TO i_x_donation_transactions_privilege_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs RENAME COLUMN "AdminTariffGroupId" TO admin_tariff_group_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_admin_tariffs_AdminTariffGroupId" RENAME TO i_x_admin_tariffs_admin_tariff_group_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "Username" TO username;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "SubscribedAt" TO subscribed_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "LastName" TO last_name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "IsActive" TO is_active;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "FirstName" TO first_name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers RENAME COLUMN "ChatId" TO chat_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_TelegramSubscribers_UserId" RENAME TO i_x_telegram_subscribers_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_TelegramSubscribers_ChatId" RENAME TO "IX_telegram_subscribers_chat_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "Title" TO title;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "Order" TO "order";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "Description" TO description;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "Buttons" TO buttons;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "ImageUrl" TO image_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "Value" TO value;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "Key" TO key;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "Description" TO description;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "Category" TO category;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "DataType" TO data_type;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_SiteSettings_Key" RENAME TO "IX_site_settings_key";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_SiteSettings_Category" RENAME TO "IX_site_settings_category";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views RENAME COLUMN "ViewDate" TO view_date;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views RENAME COLUMN "NewsId" TO news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views RENAME COLUMN "IpAddress" TO ip_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsViews_UserId" RENAME TO i_x_news_views_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsViews_NewsId_UserId_ViewDate" RENAME TO "IX_news_views_news_id_user_id_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsViews_NewsId_IpAddress_ViewDate" RENAME TO "IX_news_views_news_id_ip_address_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media RENAME COLUMN "Order" TO "order";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media RENAME COLUMN "NewsId" TO news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media RENAME COLUMN "MediaUrl" TO media_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media RENAME COLUMN "MediaType" TO media_type;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsMedia_NewsId" RENAME TO i_x_news_media_news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes RENAME COLUMN "NewsId" TO news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsLikes_UserId" RENAME TO i_x_news_likes_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsLikes_NewsId_UserId" RENAME TO "IX_news_likes_news_id_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "Content" TO content;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "ParentCommentId" TO parent_comment_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "NewsId" TO news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsComments_UserId" RENAME TO i_x_news_comments_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsComments_ParentCommentId" RENAME TO i_x_news_comments_parent_comment_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_NewsComments_NewsId" RENAME TO i_x_news_comments_news_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views RENAME COLUMN "ViewDate" TO view_date;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views RENAME COLUMN "IpAddress" TO ip_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views RENAME COLUMN "EventId" TO event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventViews_UserId" RENAME TO i_x_event_views_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventViews_EventId_UserId_ViewDate" RENAME TO "IX_event_views_event_id_user_id_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventViews_EventId_IpAddress_ViewDate" RENAME TO "IX_event_views_event_id_ip_address_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media RENAME COLUMN "Order" TO "order";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media RENAME COLUMN "MediaUrl" TO media_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media RENAME COLUMN "MediaType" TO media_type;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media RENAME COLUMN "EventId" TO event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventMedia_EventId" RENAME TO i_x_event_media_event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes RENAME COLUMN "EventId" TO event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventLikes_UserId" RENAME TO i_x_event_likes_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventLikes_EventId_UserId" RENAME TO "IX_event_likes_event_id_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "Content" TO content;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "ParentCommentId" TO parent_comment_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "EventId" TO event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventComments_UserId" RENAME TO i_x_event_comments_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventComments_ParentCommentId" RENAME TO i_x_event_comments_parent_comment_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_EventComments_EventId" RENAME TO i_x_event_comments_event_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views RENAME COLUMN "ViewDate" TO view_date;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views RENAME COLUMN "IpAddress" TO ip_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views RENAME COLUMN "CustomPageId" TO custom_page_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPageViews_UserId" RENAME TO i_x_custom_page_views_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPageViews_CustomPageId_UserId_ViewDate" RENAME TO "IX_custom_page_views_custom_page_id_user_id_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPageViews_CustomPageId_IpAddress_ViewDate" RENAME TO "IX_custom_page_views_custom_page_id_ip_address_view_date";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "Title" TO title;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "Summary" TO summary;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "Slug" TO slug;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "Content" TO content;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "ViewCount" TO view_count;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "IsPublished" TO is_published;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "CoverImage" TO cover_image;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages RENAME COLUMN "AuthorId" TO author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPages_Slug" RENAME TO "IX_custom_pages_slug";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPages_AuthorId" RENAME TO i_x_custom_pages_author_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media RENAME COLUMN "Order" TO "order";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media RENAME COLUMN "MediaUrl" TO media_url;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media RENAME COLUMN "MediaType" TO media_type;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media RENAME COLUMN "CustomPageId" TO custom_page_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_CustomPageMedia_CustomPageId" RENAME TO i_x_custom_page_media_custom_page_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips RENAME COLUMN "Reason" TO reason;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips RENAME COLUMN "IpAddress" TO ip_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips RENAME COLUMN "BlockedByUserId" TO blocked_by_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips RENAME COLUMN "BlockedAt" TO blocked_at;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_BlockedIps_IpAddress" RENAME TO "IX_blocked_ips_ip_address";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER INDEX "IX_BlockedIps_BlockedByUserId" RENAME TO i_x_blocked_ips_blocked_by_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ALTER COLUMN wallet_number TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ALTER COLUMN secret_key TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ALTER COLUMN is_configured TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ALTER COLUMN id TYPE integer;
    ALTER TABLE yoomoney_settings ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE yoomoney_settings ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN group_name TYPE character varying(64);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN description TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ALTER COLUMN id TYPE integer;
    ALTER TABLE vip_tariffs ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE vip_tariffs ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN tariff_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN duration_days TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ALTER COLUMN id TYPE integer;
    ALTER TABLE vip_tariff_options ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE vip_tariff_options ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN username TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN port TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN password TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN is_configured TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN host TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN database TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ALTER COLUMN id TYPE integer;
    ALTER TABLE vip_settings ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE vip_settings ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN username TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN email TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN id TYPE integer;
    ALTER TABLE users ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE users ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN steam_profile_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN steam_id TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN password_hash TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN last_ip TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN is_blocked TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN is_admin TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN blocked_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN block_reason TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ALTER COLUMN avatar_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN transaction_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN tariff_option_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN tariff_id TYPE integer;
    ALTER TABLE user_vip_privileges ALTER COLUMN tariff_id DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN steam_id TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN starts_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN group_name TYPE character varying(64);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN expires_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ALTER COLUMN id TYPE integer;
    ALTER TABLE user_vip_privileges ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE user_vip_privileges ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN transaction_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN tariff_option_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN tariff_id TYPE integer;
    ALTER TABLE user_admin_privileges ALTER COLUMN tariff_id DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN steam_id TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN starts_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN sourcebans_admin_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN immunity TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN group_name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN flags TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN expires_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN admin_password TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ALTER COLUMN id TYPE integer;
    ALTER TABLE user_admin_privileges ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE user_admin_privileges ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN username TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN port TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN password TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN is_configured TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN host TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN database TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ALTER COLUMN id TYPE integer;
    ALTER TABLE sourcebans_settings ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE sourcebans_settings ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN username TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN port TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN password TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN is_configured TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN host TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN from_name TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN from_email TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN enable_ssl TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ALTER COLUMN id TYPE integer;
    ALTER TABLE smtp_settings ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE smtp_settings ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN port TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN id TYPE integer;
    ALTER TABLE servers ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE servers ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN max_players TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN map_name TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN last_checked TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN is_online TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN ip_address TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ALTER COLUMN current_players TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ADD rcon_password character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN used_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN token TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN is_used TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN expires_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ALTER COLUMN id TYPE integer;
    ALTER TABLE password_reset_tokens ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE password_reset_tokens ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN type TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN title TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN message TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN id TYPE integer;
    ALTER TABLE notifications ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE notifications ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN related_entity_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN is_read TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN title TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN summary TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN slug TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN content TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN id TYPE integer;
    ALTER TABLE news ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE news ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN view_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN like_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN is_published TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN cover_image TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN comment_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ALTER COLUMN author_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN title TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN summary TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN slug TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN content TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN id TYPE integer;
    ALTER TABLE events ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE events ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN view_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN start_date TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN like_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN is_start_notification_sent TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN is_published TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN is_end_notification_sent TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN is_created_notification_sent TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN end_date TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN cover_image TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN comment_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ALTER COLUMN author_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN vip_tariff_option_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN vip_tariff_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN type TYPE character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN transaction_id TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN tariff_option_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN tariff_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN steam_id TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN status TYPE character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN privilege_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN pending_expires_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN payment_url TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN payment_method TYPE character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN label TYPE character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN expires_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN completed_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN cancelled_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN admin_password TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ALTER COLUMN id TYPE integer;
    ALTER TABLE donation_transactions ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE donation_transactions ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN title TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN suggested_amounts TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN description TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ALTER COLUMN id TYPE integer;
    ALTER TABLE donation_packages ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE donation_packages ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN immunity TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN group_name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN flags TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN description TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN id TYPE integer;
    ALTER TABLE admin_tariffs ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE admin_tariffs ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ALTER COLUMN admin_tariff_group_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN tariff_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN requires_password TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN duration_days TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ALTER COLUMN id TYPE integer;
    ALTER TABLE admin_tariff_options ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE admin_tariff_options ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN server_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN name TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN description TYPE character varying(1000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ALTER COLUMN id TYPE integer;
    ALTER TABLE admin_tariff_groups ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE admin_tariff_groups ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN username TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN id TYPE integer;
    ALTER TABLE telegram_subscribers ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE telegram_subscribers ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN subscribed_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN last_name TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN is_active TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ALTER COLUMN first_name TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN title TYPE character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN description TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN buttons TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN id TYPE integer;
    ALTER TABLE slider_images ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE slider_images ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN image_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN value TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN key TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN description TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN category TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN id TYPE integer;
    ALTER TABLE site_settings ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE site_settings ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN data_type TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ALTER COLUMN id TYPE integer;
    ALTER TABLE news_views ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE news_views ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ALTER COLUMN view_date TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ALTER COLUMN news_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ALTER COLUMN ip_address TYPE character varying(45);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ALTER COLUMN id TYPE integer;
    ALTER TABLE news_media ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE news_media ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ALTER COLUMN news_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ALTER COLUMN media_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ALTER COLUMN media_type TYPE character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ALTER COLUMN id TYPE integer;
    ALTER TABLE news_likes ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE news_likes ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ALTER COLUMN news_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN content TYPE character varying(2000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN id TYPE integer;
    ALTER TABLE news_comments ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE news_comments ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN parent_comment_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN news_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ALTER COLUMN id TYPE integer;
    ALTER TABLE event_views ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE event_views ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ALTER COLUMN view_date TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ALTER COLUMN ip_address TYPE character varying(45);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ALTER COLUMN event_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ALTER COLUMN id TYPE integer;
    ALTER TABLE event_media ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE event_media ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ALTER COLUMN media_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ALTER COLUMN media_type TYPE character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ALTER COLUMN event_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ALTER COLUMN id TYPE integer;
    ALTER TABLE event_likes ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE event_likes ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ALTER COLUMN event_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN content TYPE character varying(2000);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN id TYPE integer;
    ALTER TABLE event_comments ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE event_comments ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN parent_comment_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN event_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ALTER COLUMN id TYPE integer;
    ALTER TABLE custom_page_views ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE custom_page_views ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ALTER COLUMN view_date TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ALTER COLUMN user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ALTER COLUMN ip_address TYPE character varying(45);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ALTER COLUMN custom_page_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN title TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN summary TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN slug TYPE character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN content TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN id TYPE integer;
    ALTER TABLE custom_pages ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE custom_pages ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN view_count TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN updated_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN is_published TYPE boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN created_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN cover_image TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ALTER COLUMN author_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ALTER COLUMN "order" TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ALTER COLUMN id TYPE integer;
    ALTER TABLE custom_page_media ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE custom_page_media ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ALTER COLUMN media_url TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ALTER COLUMN media_type TYPE character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ALTER COLUMN custom_page_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ALTER COLUMN reason TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ALTER COLUMN id TYPE integer;
    ALTER TABLE blocked_ips ALTER COLUMN id DROP DEFAULT;
    ALTER TABLE blocked_ips ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ALTER COLUMN ip_address TYPE character varying(45);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ALTER COLUMN blocked_by_user_id TYPE integer;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ALTER COLUMN blocked_at TYPE timestamp without time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE yoomoney_settings ADD CONSTRAINT p_k_yoomoney_settings PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ADD CONSTRAINT p_k_vip_tariffs PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ADD CONSTRAINT p_k_vip_tariff_options PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ADD CONSTRAINT p_k_vip_settings PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE users ADD CONSTRAINT p_k_users PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT p_k_user_vip_privileges PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT p_k_user_admin_privileges PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ADD CONSTRAINT p_k_sourcebans_settings PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE smtp_settings ADD CONSTRAINT p_k_smtp_settings PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE servers ADD CONSTRAINT p_k_servers PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ADD CONSTRAINT p_k_password_reset_tokens PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ADD CONSTRAINT p_k_notifications PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ADD CONSTRAINT p_k_news PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ADD CONSTRAINT p_k_events PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT p_k_donation_transactions PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_packages ADD CONSTRAINT p_k_donation_packages PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ADD CONSTRAINT p_k_admin_tariffs PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ADD CONSTRAINT p_k_admin_tariff_options PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ADD CONSTRAINT p_k_admin_tariff_groups PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ADD CONSTRAINT p_k_telegram_subscribers PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE slider_images ADD CONSTRAINT p_k_slider_images PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE site_settings ADD CONSTRAINT p_k_site_settings PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ADD CONSTRAINT p_k_news_views PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ADD CONSTRAINT p_k_news_media PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ADD CONSTRAINT p_k_news_likes PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ADD CONSTRAINT p_k_news_comments PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ADD CONSTRAINT p_k_event_views PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ADD CONSTRAINT p_k_event_media PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ADD CONSTRAINT p_k_event_likes PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ADD CONSTRAINT p_k_event_comments PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ADD CONSTRAINT p_k_custom_page_views PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ADD CONSTRAINT p_k_custom_pages PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ADD CONSTRAINT p_k_custom_page_media PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ADD CONSTRAINT p_k_blocked_ips PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE TABLE nav_sections (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        name character varying(100) NOT NULL,
        icon character varying(50),
        "order" integer NOT NULL,
        is_published boolean NOT NULL,
        type integer NOT NULL,
        url character varying(500),
        is_external boolean NOT NULL,
        open_in_new_tab boolean NOT NULL,
        created_at timestamp without time zone NOT NULL,
        updated_at timestamp without time zone NOT NULL,
        CONSTRAINT p_k_nav_sections PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE TABLE vip_applications (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        user_id integer NOT NULL,
        username character varying(150) NOT NULL,
        steam_id character varying(50) NOT NULL,
        server_id integer NOT NULL,
        hours_per_week integer,
        reason text NOT NULL,
        status character varying(20) NOT NULL,
        admin_id integer,
        admin_comment text,
        vip_group character varying(128),
        duration_days integer,
        created_at timestamp without time zone NOT NULL,
        updated_at timestamp without time zone NOT NULL,
        processed_at timestamp without time zone,
        CONSTRAINT p_k_vip_applications PRIMARY KEY (id),
        CONSTRAINT f_k_vip_applications_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE,
        CONSTRAINT f_k_vip_applications_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE TABLE nav_section_items (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        section_id integer NOT NULL,
        name character varying(100) NOT NULL,
        icon character varying(50),
        "order" integer NOT NULL,
        is_published boolean NOT NULL,
        type integer NOT NULL,
        url character varying(500),
        custom_page_id integer,
        open_in_new_tab boolean NOT NULL,
        created_at timestamp without time zone NOT NULL,
        CONSTRAINT p_k_nav_section_items PRIMARY KEY (id),
        CONSTRAINT f_k_nav_section_items_custom_pages_custom_page_id FOREIGN KEY (custom_page_id) REFERENCES custom_pages (id) ON DELETE SET NULL,
        CONSTRAINT f_k_nav_section_items_nav_sections_section_id FOREIGN KEY (section_id) REFERENCES nav_sections (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX i_x_nav_section_items_custom_page_id ON nav_section_items (custom_page_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX "IX_nav_section_items_section_id_order" ON nav_section_items (section_id, "order");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX "IX_nav_sections_is_published" ON nav_sections (is_published);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX "IX_nav_sections_order" ON nav_sections ("order");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX i_x_vip_applications_server_id ON vip_applications (server_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    CREATE INDEX "IX_vip_applications_user_id_server_id_status" ON vip_applications (user_id, server_id, status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_groups ADD CONSTRAINT f_k_admin_tariff_groups__servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariff_options ADD CONSTRAINT f_k_admin_tariff_options_admin_tariffs_tariff_id FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ADD CONSTRAINT f_k_admin_tariffs__servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE admin_tariffs ADD CONSTRAINT f_k_admin_tariffs_admin_tariff_groups_admin_tariff_group_id FOREIGN KEY (admin_tariff_group_id) REFERENCES admin_tariff_groups (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE blocked_ips ADD CONSTRAINT f_k_blocked_ips__users_blocked_by_user_id FOREIGN KEY (blocked_by_user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_media ADD CONSTRAINT f_k_custom_page_media_custom_pages_custom_page_id FOREIGN KEY (custom_page_id) REFERENCES custom_pages (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ADD CONSTRAINT f_k_custom_page_views__users_user_id FOREIGN KEY (user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_page_views ADD CONSTRAINT f_k_custom_page_views_custom_pages_custom_page_id FOREIGN KEY (custom_page_id) REFERENCES custom_pages (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE custom_pages ADD CONSTRAINT f_k_custom_pages__users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions__servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions__users_user_id FOREIGN KEY (user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions_admin_tariff_options_tariff_option_id FOREIGN KEY (tariff_option_id) REFERENCES admin_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions_admin_tariffs_tariff_id FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions_user_admin_privileges_privilege_id FOREIGN KEY (privilege_id) REFERENCES user_admin_privileges (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT "f_k_donation_transactions_vip_tariff_options_vip_tariff_option_~" FOREIGN KEY (vip_tariff_option_id) REFERENCES vip_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE donation_transactions ADD CONSTRAINT f_k_donation_transactions_vip_tariffs_vip_tariff_id FOREIGN KEY (vip_tariff_id) REFERENCES vip_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ADD CONSTRAINT f_k_event_comments__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ADD CONSTRAINT f_k_event_comments_event_comments_parent_comment_id FOREIGN KEY (parent_comment_id) REFERENCES event_comments (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_comments ADD CONSTRAINT f_k_event_comments_events_event_id FOREIGN KEY (event_id) REFERENCES events (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ADD CONSTRAINT f_k_event_likes__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_likes ADD CONSTRAINT f_k_event_likes_events_event_id FOREIGN KEY (event_id) REFERENCES events (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_media ADD CONSTRAINT f_k_event_media_events_event_id FOREIGN KEY (event_id) REFERENCES events (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ADD CONSTRAINT f_k_event_views__users_user_id FOREIGN KEY (user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE event_views ADD CONSTRAINT f_k_event_views_events_event_id FOREIGN KEY (event_id) REFERENCES events (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE events ADD CONSTRAINT f_k_events__users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news ADD CONSTRAINT f_k_news__users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ADD CONSTRAINT f_k_news_comments__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ADD CONSTRAINT f_k_news_comments_news_comments_parent_comment_id FOREIGN KEY (parent_comment_id) REFERENCES news_comments (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_comments ADD CONSTRAINT f_k_news_comments_news_news_id FOREIGN KEY (news_id) REFERENCES news (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ADD CONSTRAINT f_k_news_likes__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_likes ADD CONSTRAINT f_k_news_likes_news_news_id FOREIGN KEY (news_id) REFERENCES news (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_media ADD CONSTRAINT f_k_news_media_news_news_id FOREIGN KEY (news_id) REFERENCES news (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ADD CONSTRAINT f_k_news_views__users_user_id FOREIGN KEY (user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE news_views ADD CONSTRAINT f_k_news_views_news_news_id FOREIGN KEY (news_id) REFERENCES news (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE notifications ADD CONSTRAINT f_k_notifications__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE password_reset_tokens ADD CONSTRAINT f_k_password_reset_tokens__users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE sourcebans_settings ADD CONSTRAINT f_k_sourcebans_settings_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE telegram_subscribers ADD CONSTRAINT f_k_telegram_subscribers__users_user_id FOREIGN KEY (user_id) REFERENCES users (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT f_k_user_admin_privileges_admin_tariff_options_tariff_option_id FOREIGN KEY (tariff_option_id) REFERENCES admin_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT f_k_user_admin_privileges_admin_tariffs_tariff_id FOREIGN KEY (tariff_id) REFERENCES admin_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT f_k_user_admin_privileges_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_admin_privileges ADD CONSTRAINT f_k_user_admin_privileges_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT f_k_user_vip_privileges_donation_transactions_transaction_id FOREIGN KEY (transaction_id) REFERENCES donation_transactions (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT f_k_user_vip_privileges_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT f_k_user_vip_privileges_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT f_k_user_vip_privileges_vip_tariff_options_tariff_option_id FOREIGN KEY (tariff_option_id) REFERENCES vip_tariff_options (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE user_vip_privileges ADD CONSTRAINT f_k_user_vip_privileges_vip_tariffs_tariff_id FOREIGN KEY (tariff_id) REFERENCES vip_tariffs (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_settings ADD CONSTRAINT f_k_vip_settings_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariff_options ADD CONSTRAINT f_k_vip_tariff_options_vip_tariffs_tariff_id FOREIGN KEY (tariff_id) REFERENCES vip_tariffs (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    ALTER TABLE vip_tariffs ADD CONSTRAINT f_k_vip_tariffs_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251227170509_AddNavSections') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251227170509_AddNavSections', '9.0.0');
    END IF;
END $EF$;
COMMIT;

