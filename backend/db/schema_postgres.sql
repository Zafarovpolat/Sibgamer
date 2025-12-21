-- ===========================================
-- SibGamer PostgreSQL Schema
-- ===========================================

-- Users
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    avatar_url TEXT,
    is_admin BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    steam_id VARCHAR(50),
    steam_profile_url TEXT,
    last_ip TEXT,
    is_blocked BOOLEAN NOT NULL DEFAULT FALSE,
    block_reason TEXT,
    blocked_at TIMESTAMP
);

-- Servers
CREATE TABLE IF NOT EXISTS servers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    ip_address TEXT NOT NULL,
    port INTEGER NOT NULL,
    map_name TEXT NOT NULL DEFAULT '',
    current_players INTEGER NOT NULL DEFAULT 0,
    max_players INTEGER NOT NULL DEFAULT 0,
    is_online BOOLEAN NOT NULL DEFAULT FALSE,
    last_checked TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    rcon_password VARCHAR(255)
);

-- News
CREATE TABLE IF NOT EXISTS news (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    content TEXT NOT NULL,
    summary VARCHAR(500),
    slug VARCHAR(200) NOT NULL UNIQUE,
    cover_image TEXT,
    author_id INTEGER NOT NULL REFERENCES users(id),
    is_published BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    view_count INTEGER NOT NULL DEFAULT 0,
    like_count INTEGER NOT NULL DEFAULT 0,
    comment_count INTEGER NOT NULL DEFAULT 0
);

-- News Comments
CREATE TABLE IF NOT EXISTS news_comments (
    id SERIAL PRIMARY KEY,
    news_id INTEGER NOT NULL REFERENCES news(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id),
    content VARCHAR(2000) NOT NULL,
    parent_comment_id INTEGER REFERENCES news_comments(id),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- News Likes
CREATE TABLE IF NOT EXISTS news_likes (
    id SERIAL PRIMARY KEY,
    news_id INTEGER NOT NULL REFERENCES news(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(news_id, user_id)
);

-- News Media
CREATE TABLE IF NOT EXISTS news_media (
    id SERIAL PRIMARY KEY,
    news_id INTEGER NOT NULL REFERENCES news(id) ON DELETE CASCADE,
    media_url TEXT NOT NULL,
    media_type VARCHAR(20) NOT NULL,
    "order" INTEGER NOT NULL DEFAULT 0
);

-- News Views
CREATE TABLE IF NOT EXISTS news_views (
    id SERIAL PRIMARY KEY,
    news_id INTEGER NOT NULL REFERENCES news(id) ON DELETE CASCADE,
    user_id INTEGER REFERENCES users(id),
    ip_address VARCHAR(45),
    view_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Events
CREATE TABLE IF NOT EXISTS events (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    content TEXT NOT NULL,
    summary VARCHAR(500),
    slug VARCHAR(200) NOT NULL UNIQUE,
    cover_image TEXT,
    author_id INTEGER NOT NULL REFERENCES users(id),
    is_published BOOLEAN NOT NULL DEFAULT FALSE,
    start_date TIMESTAMP NOT NULL,
    end_date TIMESTAMP NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    view_count INTEGER NOT NULL DEFAULT 0,
    like_count INTEGER NOT NULL DEFAULT 0,
    comment_count INTEGER NOT NULL DEFAULT 0,
    is_start_notification_sent BOOLEAN NOT NULL DEFAULT FALSE,
    is_end_notification_sent BOOLEAN NOT NULL DEFAULT FALSE,
    is_created_notification_sent BOOLEAN NOT NULL DEFAULT FALSE
);

-- Event Comments
CREATE TABLE IF NOT EXISTS event_comments (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id),
    content VARCHAR(2000) NOT NULL,
    parent_comment_id INTEGER REFERENCES event_comments(id),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Event Likes
CREATE TABLE IF NOT EXISTS event_likes (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(event_id, user_id)
);

-- Event Media
CREATE TABLE IF NOT EXISTS event_media (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    media_url TEXT NOT NULL,
    media_type VARCHAR(20) NOT NULL,
    "order" INTEGER NOT NULL DEFAULT 0
);

-- Event Views
CREATE TABLE IF NOT EXISTS event_views (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    user_id INTEGER REFERENCES users(id),
    ip_address VARCHAR(45),
    view_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Custom Pages
CREATE TABLE IF NOT EXISTS custom_pages (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    content TEXT NOT NULL,
    summary VARCHAR(500),
    slug VARCHAR(200) NOT NULL UNIQUE,
    cover_image TEXT,
    author_id INTEGER NOT NULL REFERENCES users(id),
    is_published BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    view_count INTEGER NOT NULL DEFAULT 0
);

-- Custom Page Media
CREATE TABLE IF NOT EXISTS custom_page_media (
    id SERIAL PRIMARY KEY,
    custom_page_id INTEGER NOT NULL REFERENCES custom_pages(id) ON DELETE CASCADE,
    media_url TEXT NOT NULL,
    media_type VARCHAR(20) NOT NULL,
    "order" INTEGER NOT NULL DEFAULT 0
);

-- Custom Page Views
CREATE TABLE IF NOT EXISTS custom_page_views (
    id SERIAL PRIMARY KEY,
    custom_page_id INTEGER NOT NULL REFERENCES custom_pages(id) ON DELETE CASCADE,
    user_id INTEGER REFERENCES users(id),
    ip_address VARCHAR(45),
    view_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Site Settings
CREATE TABLE IF NOT EXISTS site_settings (
    id SERIAL PRIMARY KEY,
    key VARCHAR(255) NOT NULL UNIQUE,
    value TEXT NOT NULL,
    category VARCHAR(255) NOT NULL,
    description TEXT NOT NULL DEFAULT '',
    data_type TEXT NOT NULL DEFAULT 'string',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Slider Images
CREATE TABLE IF NOT EXISTS slider_images (
    id SERIAL PRIMARY KEY,
    image_url TEXT NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(500) NOT NULL DEFAULT '',
    "order" INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    buttons TEXT
);

-- Notifications
CREATE TABLE IF NOT EXISTS notifications (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    title VARCHAR(200) NOT NULL,
    message VARCHAR(1000) NOT NULL,
    type VARCHAR(50) NOT NULL,
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    related_entity_id INTEGER,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_notifications_user_created ON notifications(user_id, created_at DESC);

-- Telegram Subscribers
CREATE TABLE IF NOT EXISTS telegram_subscribers (
    id SERIAL PRIMARY KEY,
    chat_id BIGINT NOT NULL UNIQUE,
    user_id INTEGER REFERENCES users(id),
    username TEXT,
    first_name TEXT,
    last_name TEXT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    subscribed_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Blocked IPs
CREATE TABLE IF NOT EXISTS blocked_ips (
    id SERIAL PRIMARY KEY,
    ip_address VARCHAR(45) NOT NULL UNIQUE,
    reason TEXT,
    blocked_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    blocked_by_user_id INTEGER REFERENCES users(id)
);

-- Password Reset Tokens
CREATE TABLE IF NOT EXISTS password_reset_tokens (
    id SERIAL PRIMARY KEY,
    token VARCHAR(500) NOT NULL,
    user_id INTEGER NOT NULL REFERENCES users(id),
    expires_at TIMESTAMP NOT NULL,
    is_used BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    used_at TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_password_reset_token ON password_reset_tokens(token);

-- SMTP Settings
CREATE TABLE IF NOT EXISTS smtp_settings (
    id SERIAL PRIMARY KEY,
    host VARCHAR(255) NOT NULL,
    port INTEGER NOT NULL,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(500) NOT NULL,
    enable_ssl BOOLEAN NOT NULL DEFAULT TRUE,
    from_email VARCHAR(255) NOT NULL,
    from_name VARCHAR(255) NOT NULL,
    is_configured BOOLEAN NOT NULL DEFAULT FALSE,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- YooMoney Settings
CREATE TABLE IF NOT EXISTS yoomoney_settings (
    id SERIAL PRIMARY KEY,
    wallet_number VARCHAR(100) NOT NULL,
    secret_key VARCHAR(500) NOT NULL,
    is_configured BOOLEAN NOT NULL DEFAULT FALSE,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- VIP Tariffs
CREATE TABLE IF NOT EXISTS vip_tariffs (
    id SERIAL PRIMARY KEY,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    name VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NOT NULL DEFAULT '',
    group_name VARCHAR(64) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    "order" INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- VIP Tariff Options
CREATE TABLE IF NOT EXISTS vip_tariff_options (
    id SERIAL PRIMARY KEY,
    tariff_id INTEGER NOT NULL REFERENCES vip_tariffs(id) ON DELETE CASCADE,
    duration_days INTEGER NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    "order" INTEGER NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Admin Tariff Groups
CREATE TABLE IF NOT EXISTS admin_tariff_groups (
    id SERIAL PRIMARY KEY,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    name VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NOT NULL DEFAULT '',
    "order" INTEGER NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Admin Tariffs
CREATE TABLE IF NOT EXISTS admin_tariffs (
    id SERIAL PRIMARY KEY,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    name VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NOT NULL DEFAULT '',
    flags VARCHAR(100),
    group_name VARCHAR(100),
    immunity INTEGER NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    "order" INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    admin_tariff_group_id INTEGER REFERENCES admin_tariff_groups(id)
);

-- Admin Tariff Options
CREATE TABLE IF NOT EXISTS admin_tariff_options (
    id SERIAL PRIMARY KEY,
    tariff_id INTEGER NOT NULL REFERENCES admin_tariffs(id) ON DELETE CASCADE,
    duration_days INTEGER NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    "order" INTEGER NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    requires_password BOOLEAN NOT NULL DEFAULT TRUE
);

-- Donation Packages
CREATE TABLE IF NOT EXISTS donation_packages (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    description VARCHAR(1000) NOT NULL DEFAULT '',
    suggested_amounts VARCHAR(500),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Donation Transactions
CREATE TABLE IF NOT EXISTS donation_transactions (
    id SERIAL PRIMARY KEY,
    transaction_id VARCHAR(255) NOT NULL UNIQUE,
    user_id INTEGER REFERENCES users(id),
    steam_id VARCHAR(50),
    amount DECIMAL(10,2) NOT NULL,
    type VARCHAR(20) NOT NULL,
    tariff_id INTEGER,
    server_id INTEGER REFERENCES servers(id),
    status VARCHAR(20) NOT NULL DEFAULT 'pending',
    payment_method VARCHAR(50),
    label VARCHAR(255),
    expires_at TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP,
    cancelled_at TIMESTAMP,
    payment_url VARCHAR(1000),
    pending_expires_at TIMESTAMP,
    tariff_option_id INTEGER,
    admin_password VARCHAR(100),
    privilege_id INTEGER,
    vip_tariff_id INTEGER,
    vip_tariff_option_id INTEGER
);

CREATE INDEX IF NOT EXISTS idx_donation_steam ON donation_transactions(steam_id);

-- User VIP Privileges
CREATE TABLE IF NOT EXISTS user_vip_privileges (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id),
    steam_id VARCHAR(50) NOT NULL,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    tariff_id INTEGER NOT NULL REFERENCES vip_tariffs(id),
    tariff_option_id INTEGER REFERENCES vip_tariff_options(id),
    group_name VARCHAR(64) NOT NULL,
    starts_at TIMESTAMP NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    transaction_id INTEGER,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_vip_steam ON user_vip_privileges(steam_id);
CREATE INDEX IF NOT EXISTS idx_vip_expires ON user_vip_privileges(expires_at);

-- User Admin Privileges
CREATE TABLE IF NOT EXISTS user_admin_privileges (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id),
    steam_id VARCHAR(50) NOT NULL,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    tariff_id INTEGER NOT NULL REFERENCES admin_tariffs(id),
    tariff_option_id INTEGER REFERENCES admin_tariff_options(id),
    transaction_id INTEGER,
    flags VARCHAR(100),
    group_name VARCHAR(100),
    immunity INTEGER NOT NULL DEFAULT 0,
    starts_at TIMESTAMP NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    sourcebans_admin_id INTEGER,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    admin_password VARCHAR(100)
);

CREATE INDEX IF NOT EXISTS idx_admin_steam ON user_admin_privileges(steam_id);
CREATE INDEX IF NOT EXISTS idx_admin_expires ON user_admin_privileges(expires_at);

-- VIP Settings (connection to VIP database)
CREATE TABLE IF NOT EXISTS vip_settings (
    id SERIAL PRIMARY KEY,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    host VARCHAR(255) NOT NULL,
    port INTEGER NOT NULL DEFAULT 3306,
    database VARCHAR(100) NOT NULL,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(500) NOT NULL,
    is_configured BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- SourceBans Settings
CREATE TABLE IF NOT EXISTS sourcebans_settings (
    id SERIAL PRIMARY KEY,
    server_id INTEGER NOT NULL REFERENCES servers(id),
    host VARCHAR(255) NOT NULL,
    port INTEGER NOT NULL DEFAULT 3306,
    database VARCHAR(100) NOT NULL,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(500) NOT NULL,
    is_configured BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- EF Migrations History (для совместимости)
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" VARCHAR(150) NOT NULL PRIMARY KEY,
    "ProductVersion" VARCHAR(32) NOT NULL
);

-- ===========================================
-- Initial Data
-- ===========================================

-- Default site settings
INSERT INTO site_settings (key, value, category, description, data_type) VALUES
    ('site_name', 'SibGamer', 'general', 'Название сайта', 'string'),
    ('site_description', 'Игровое сообщество Counter-Strike', 'general', 'Описание сайта', 'string'),
    ('telegram_bot_token', '', 'telegram', 'Токен Telegram бота', 'string'),
    ('telegram_channel_id', '', 'telegram', 'ID канала Telegram', 'string')
ON CONFLICT (key) DO NOTHING;

-- Default SMTP settings
INSERT INTO smtp_settings (host, port, username, password, enable_ssl, from_email, from_name, is_configured)
VALUES ('smtp.example.com', 587, '', '', true, 'noreply@example.com', 'SibGamer', false)
ON CONFLICT DO NOTHING;

-- Default YooMoney settings
INSERT INTO yoomoney_settings (wallet_number, secret_key, is_configured)
VALUES ('', '', false)
ON CONFLICT DO NOTHING;