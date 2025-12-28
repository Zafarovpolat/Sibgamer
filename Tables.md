# SibGamer Database Schema

> **Database:** PostgreSQL 15 (Neon DB)  
> **ORM:** Entity Framework Core 9.0  
> **Naming:** snake_case (PostgreSQL convention)  
> **Tables:** 36

---

## 📊 Tables Overview

| Category | Tables |
|----------|--------|
| **Auth & Users** | `users`, `blocked_ips`, `password_reset_tokens` |
| **Content** | `news`, `events`, `custom_pages` + media/comments/likes/views |
| **Navigation** | `nav_sections`, `nav_section_items` |
| **Donations** | `donation_packages`, `donation_transactions` |
| **VIP/Admin** | `vip_tariffs`, `vip_tariff_options`, `admin_tariffs`, `admin_tariff_options`, `admin_tariff_groups`, `user_vip_privileges`, `user_admin_privileges`, `vip_applications` |
| **Servers** | `servers`, `vip_settings`, `sourcebans_settings` |
| **Settings** | `site_settings`, `smtp_settings`, `yoomoney_settings` |
| **UI** | `slider_images` |
| **Notifications** | `notifications`, `telegram_subscribers` |

---

## 🔐 Auth & Users

### users
| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| id | integer | NO | PK, auto-increment |
| username | varchar(50) | NO | Unique |
| email | text | NO | Unique |
| password_hash | text | NO | BCrypt hash |
| avatar_url | text | YES | Profile image |
| steam_id | varchar(50) | YES | Steam64 ID |
| steam_profile_url | text | YES | Steam profile link |
| last_ip | text | YES | Last login IP |
| is_admin | boolean | NO | Admin flag |
| is_blocked | boolean | NO | Block status |
| blocked_at | timestamp | YES | When blocked |
| block_reason | text | YES | Reason for block |
| created_at | timestamp | NO | Registration date |

### blocked_ips
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| ip_address | varchar(45) | Blocked IP (IPv4/IPv6) |
| reason | text | Block reason |
| blocked_at | timestamp | When blocked |
| blocked_by_user_id | integer | FK → users.id |

### password_reset_tokens
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| token | varchar(500) | Reset token |
| user_id | integer | FK → users.id |
| expires_at | timestamp | Token expiration |
| is_used | boolean | Whether used |
| created_at | timestamp | Created date |
| used_at | timestamp | When used |

---

## 📰 Content

### news
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| title | varchar(200) | Article title |
| content | text | Rich HTML content |
| summary | varchar(500) | Short description |
| slug | varchar(200) | URL slug (unique) |
| cover_image | text | Cover image URL |
| author_id | integer | FK → users.id |
| is_published | boolean | Publication status |
| created_at | timestamp | Created date |
| updated_at | timestamp | Last modified |
| view_count | integer | Total views |
| like_count | integer | Total likes |
| comment_count | integer | Total comments |

### news_comments, news_likes, news_views, news_media
Supporting tables for news (same pattern for events, custom_pages).

### events
Same as news, plus:
| Column | Type | Description |
|--------|------|-------------|
| start_date | timestamp | Event start |
| end_date | timestamp | Event end |
| is_start_notification_sent | boolean | Notification flag |
| is_end_notification_sent | boolean | Notification flag |
| is_created_notification_sent | boolean | Notification flag |

### custom_pages
Same structure as news (content pages).

---

## 🧭 Navigation

### nav_sections
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| name | varchar(100) | Section name |
| icon | varchar(50) | Icon class |
| order | integer | Display order |
| is_published | boolean | Visibility |
| type | integer | 0=link, 1=dropdown, 2=page |
| url | varchar(500) | Direct URL |
| is_external | boolean | External link flag |
| open_in_new_tab | boolean | New tab flag |
| created_at | timestamp | Created |
| updated_at | timestamp | Updated |

### nav_section_items
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| section_id | integer | FK → nav_sections.id |
| name | varchar(100) | Item name |
| icon | varchar(50) | Icon class |
| order | integer | Display order |
| is_published | boolean | Visibility |
| type | integer | 0=link, 1=page |
| url | varchar(500) | URL or null |
| custom_page_id | integer | FK → custom_pages.id |
| open_in_new_tab | boolean | New tab flag |
| created_at | timestamp | Created |

---

## 💰 Donations

### donation_packages
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| title | varchar(200) | Package name |
| description | varchar(1000) | Description |
| suggested_amounts | varchar(500) | JSON array of amounts |
| is_active | boolean | Active status |
| created_at | timestamp | Created |
| updated_at | timestamp | Updated |

### donation_transactions
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| transaction_id | varchar(255) | YooMoney transaction ID |
| user_id | integer | FK → users.id |
| steam_id | varchar(50) | Steam ID for privilege |
| amount | decimal(10,2) | Payment amount |
| type | varchar(20) | `vip`, `admin`, `donation` |
| tariff_id | integer | FK → admin_tariffs |
| tariff_option_id | integer | FK → admin_tariff_options |
| vip_tariff_id | integer | FK → vip_tariffs |
| vip_tariff_option_id | integer | FK → vip_tariff_options |
| privilege_id | integer | FK → user_admin_privileges |
| server_id | integer | FK → servers |
| admin_password | varchar(100) | Password for admin |
| status | varchar(20) | `pending`, `completed`, `cancelled` |
| payment_url | varchar(1000) | YooMoney payment link |
| pending_expires_at | timestamp | Pending expiration |
| payment_method | varchar(50) | Payment method |
| label | varchar(255) | YooMoney label |
| expires_at | timestamp | Privilege expiration |
| created_at | timestamp | Created |
| completed_at | timestamp | When completed |
| cancelled_at | timestamp | When cancelled |

---

## 🎮 VIP/Admin Privileges

### vip_tariffs
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| server_id | integer | FK → servers.id |
| name | varchar(100) | Tariff name |
| description | varchar(1000) | Description |
| group_name | varchar(64) | VIP group in game |
| is_active | boolean | Active status |
| order | integer | Display order |
| created_at | timestamp | Created |

### vip_tariff_options
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| tariff_id | integer | FK → vip_tariffs.id |
| duration_days | integer | Duration in days |
| price | decimal(10,2) | Price |
| order | integer | Display order |
| is_active | boolean | Active status |
| created_at | timestamp | Created |

### admin_tariff_groups
Groups admin tariffs together.

### admin_tariffs
Similar to vip_tariffs, with flags, immunity.

### admin_tariff_options
Similar to vip_tariff_options, with requires_password.

### user_vip_privileges
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| user_id | integer | FK → users.id |
| steam_id | varchar(50) | Steam ID |
| server_id | integer | FK → servers.id |
| tariff_id | integer | FK → vip_tariffs |
| tariff_option_id | integer | FK → vip_tariff_options |
| group_name | varchar(64) | VIP group |
| starts_at | timestamp | Start date |
| expires_at | timestamp | Expiration |
| is_active | boolean | Active status |
| transaction_id | integer | FK → donation_transactions |
| created_at | timestamp | Created |
| updated_at | timestamp | Updated |

### user_admin_privileges
Similar to user_vip_privileges, with flags, immunity, sourcebans_admin_id.

### vip_applications
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| user_id | integer | FK → users.id |
| username | varchar(150) | Username |
| steam_id | varchar(50) | Steam ID |
| server_id | integer | FK → servers.id |
| hours_per_week | integer | Playing hours |
| reason | text | Application reason |
| status | varchar(20) | `pending`, `approved`, `rejected` |
| admin_id | integer | Processing admin |
| admin_comment | text | Admin response |
| vip_group | varchar(128) | Assigned group |
| duration_days | integer | Granted duration |
| created_at | timestamp | Created |
| updated_at | timestamp | Updated |
| processed_at | timestamp | When processed |

---

## 🖥️ Servers & Integrations

### servers
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| name | varchar(100) | Server name |
| ip_address | text | IP address |
| port | integer | Port |
| rcon_password | varchar(100) | RCON password |
| map_name | text | Current map |
| current_players | integer | Online players |
| max_players | integer | Max slots |
| is_online | boolean | Online status |
| last_checked | timestamp | Last query time |

### vip_settings
MySQL connection settings for VIP Core database per server.

### sourcebans_settings
MySQL connection settings for SourceBans++ database per server.

---

## ⚙️ Settings

### site_settings
Key-value storage for site configuration.

### smtp_settings
SMTP email configuration.

### yoomoney_settings
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| wallet_number | varchar(100) | YooMoney wallet |
| secret_key | varchar(500) | Webhook secret |
| is_configured | boolean | Setup complete |
| updated_at | timestamp | Last update |

---

## 🎨 UI

### slider_images
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| image_url | text | Image URL |
| title | varchar(100) | Slide title |
| description | varchar(500) | Slide description |
| order | integer | Display order |
| buttons | text | JSON buttons array |
| created_at | timestamp | Created |

---

## 🔔 Notifications

### notifications
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| user_id | integer | FK → users.id |
| title | varchar(200) | Title |
| message | varchar(1000) | Message body |
| type | varchar(50) | Notification type |
| is_read | boolean | Read status |
| related_entity_id | integer | Related content ID |
| created_at | timestamp | Created |

### telegram_subscribers
| Column | Type | Description |
|--------|------|-------------|
| id | integer | PK |
| chat_id | bigint | Telegram chat ID |
| user_id | integer | FK → users.id |
| username | text | Telegram username |
| first_name | text | First name |
| last_name | text | Last name |
| is_active | boolean | Subscription status |
| subscribed_at | timestamp | When subscribed |

---

*Last updated: December 28, 2025*