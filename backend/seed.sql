-- ============================================
-- SibGamer Seed Data
-- Выполнить после применения миграции
-- Безопасно для повторного запуска
-- ============================================

-- 1. Создание администратора (если не существует)
-- Пароль: Admin123! (BCrypt хеш)
INSERT INTO users (username, email, password_hash, is_admin, is_blocked, created_at)
VALUES (
    'admin',
    'admin@sibgamer.ru',
    '$2a$11$K7qT8Y.5X9vZd3mE4rN6OuHJz1K4Lp8M3nQ5oR7sT9uV0wX2yZ4a6',
    true,
    false,
    NOW()
) ON CONFLICT (email) DO UPDATE SET is_admin = true;

-- 2. Тестовый сервер
INSERT INTO servers (name, ip_address, port, map_name, current_players, max_players, is_online, last_checked)
VALUES (
    'SibGamer Public #1',
    '185.185.185.185',
    27015,
    'de_dust2',
    0,
    32,
    false,
    NOW()
) ON CONFLICT DO NOTHING;

-- 3. Слайдер на главной
INSERT INTO slider_images (image_url, title, description, "order", created_at)
SELECT '/uploads/slider/welcome.jpg', 'Добро пожаловать на SibGamer!', 'Лучшие Counter-Strike сервера в Сибири', 1, NOW()
WHERE NOT EXISTS (SELECT 1 FROM slider_images WHERE "order" = 1);

INSERT INTO slider_images (image_url, title, description, "order", created_at)
SELECT '/uploads/slider/vip.jpg', 'VIP привилегии', 'Получите уникальные возможности на наших серверах', 2, NOW()
WHERE NOT EXISTS (SELECT 1 FROM slider_images WHERE "order" = 2);

-- 4. Базовые настройки сайта
INSERT INTO site_settings (key, value, category, description, data_type, created_at, updated_at)
VALUES 
('site_name', 'SibGamer', 'general', 'Название сайта', 'string', NOW(), NOW()),
('site_description', 'Игровой портал Counter-Strike сообщества', 'general', 'Описание сайта', 'string', NOW(), NOW()),
('contact_email', 'admin@sibgamer.ru', 'general', 'Email для связи', 'string', NOW(), NOW()),
('vk_url', 'https://vk.com/sibgamer', 'social', 'Ссылка на VK группу', 'string', NOW(), NOW()),
('discord_url', 'https://discord.gg/sibgamer', 'social', 'Ссылка на Discord', 'string', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- 5. VIP тариф (проверяем существование)
INSERT INTO vip_tariffs (server_id, name, description, group_name, is_active, "order", created_at)
SELECT 1, 'VIP Standard', 'Базовый VIP пакет с доступом к скинам и резервному слоту', 'vip', true, 1, NOW()
WHERE EXISTS (SELECT 1 FROM servers WHERE id = 1)
AND NOT EXISTS (SELECT 1 FROM vip_tariffs WHERE name = 'VIP Standard');

-- VIP опции
INSERT INTO vip_tariff_options (tariff_id, duration_days, price, "order", is_active, created_at)
SELECT id, 7, 49.00, 1, true, NOW() FROM vip_tariffs WHERE name = 'VIP Standard'
AND NOT EXISTS (SELECT 1 FROM vip_tariff_options WHERE tariff_id = (SELECT id FROM vip_tariffs WHERE name = 'VIP Standard') AND duration_days = 7);

INSERT INTO vip_tariff_options (tariff_id, duration_days, price, "order", is_active, created_at)
SELECT id, 30, 149.00, 2, true, NOW() FROM vip_tariffs WHERE name = 'VIP Standard'
AND NOT EXISTS (SELECT 1 FROM vip_tariff_options WHERE tariff_id = (SELECT id FROM vip_tariffs WHERE name = 'VIP Standard') AND duration_days = 30);

INSERT INTO vip_tariff_options (tariff_id, duration_days, price, "order", is_active, created_at)
SELECT id, 90, 349.00, 3, true, NOW() FROM vip_tariffs WHERE name = 'VIP Standard'
AND NOT EXISTS (SELECT 1 FROM vip_tariff_options WHERE tariff_id = (SELECT id FROM vip_tariffs WHERE name = 'VIP Standard') AND duration_days = 90);

-- 6. Admin тариф
INSERT INTO admin_tariffs (server_id, name, description, flags, immunity, is_active, "order", created_at)
SELECT 1, 'Admin Basic', 'Базовый Admin с правами kick/ban', 'abcd', 50, true, 1, NOW()
WHERE EXISTS (SELECT 1 FROM servers WHERE id = 1)
AND NOT EXISTS (SELECT 1 FROM admin_tariffs WHERE name = 'Admin Basic');

-- Admin опции
INSERT INTO admin_tariff_options (tariff_id, duration_days, price, "order", is_active, requires_password, created_at)
SELECT id, 30, 299.00, 1, true, true, NOW() FROM admin_tariffs WHERE name = 'Admin Basic'
AND NOT EXISTS (SELECT 1 FROM admin_tariff_options WHERE tariff_id = (SELECT id FROM admin_tariffs WHERE name = 'Admin Basic') AND duration_days = 30);

INSERT INTO admin_tariff_options (tariff_id, duration_days, price, "order", is_active, requires_password, created_at)
SELECT id, 90, 699.00, 2, true, true, NOW() FROM admin_tariffs WHERE name = 'Admin Basic'
AND NOT EXISTS (SELECT 1 FROM admin_tariff_options WHERE tariff_id = (SELECT id FROM admin_tariffs WHERE name = 'Admin Basic') AND duration_days = 90);

-- 7. Навигация - базовые разделы
INSERT INTO nav_sections (name, icon, "order", is_published, type, url, is_external, open_in_new_tab, created_at, updated_at)
SELECT 'Главная', 'Home', 1, true, 0, '/', false, false, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM nav_sections WHERE name = 'Главная');

INSERT INTO nav_sections (name, icon, "order", is_published, type, url, is_external, open_in_new_tab, created_at, updated_at)
SELECT 'Новости', 'Newspaper', 2, true, 0, '/news', false, false, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM nav_sections WHERE name = 'Новости');

INSERT INTO nav_sections (name, icon, "order", is_published, type, url, is_external, open_in_new_tab, created_at, updated_at)
SELECT 'Мероприятия', 'Calendar', 3, true, 0, '/events', false, false, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM nav_sections WHERE name = 'Мероприятия');

INSERT INTO nav_sections (name, icon, "order", is_published, type, url, is_external, open_in_new_tab, created_at, updated_at)
SELECT 'Донат', 'Gift', 4, true, 0, '/donate', false, false, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM nav_sections WHERE name = 'Донат');

INSERT INTO nav_sections (name, icon, "order", is_published, type, url, is_external, open_in_new_tab, created_at, updated_at)
SELECT 'Информация', 'Info', 5, true, 1, NULL, false, false, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM nav_sections WHERE name = 'Информация');

-- 8. Приветственная новость
INSERT INTO news (title, content, summary, slug, author_id, is_published, created_at, updated_at, view_count, like_count, comment_count)
SELECT 
    'Добро пожаловать на SibGamer!',
    '<p>Мы рады приветствовать вас на нашем игровом портале! Здесь вы найдёте всё необходимое для комфортной игры на наших серверах.</p><p>Следите за новостями и не забудьте подписаться на наш Telegram бот для получения уведомлений!</p>',
    'Мы рады приветствовать вас на нашем игровом портале!',
    'dobro-pozhalovat-na-sibgamer',
    1,
    true,
    NOW(),
    NOW(),
    0, 0, 0
WHERE NOT EXISTS (SELECT 1 FROM news WHERE slug = 'dobro-pozhalovat-na-sibgamer');

-- ============================================
-- Готово! 
-- Админ: admin@sibgamer.ru / Admin123!
-- ============================================
