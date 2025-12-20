+-----------------------+
| Tables_in_sibgamer    |
+-----------------------+
| __efmigrationshistory |
| admin_tariff_groups   |
| admin_tariff_options  |
| admin_tariffs         |
| blockedips            |
| custompagemedia       |
| custompages           |
| custompageviews       |
| donation_packages     |
| donation_transactions |
| eventcomments         |
| eventlikes            |
| eventmedia            |
| events                |
| eventviews            |
| news                  |
| newscomments          |
| newslikes             |
| newsmedia             |
| newsviews             |
| notifications         |
| password_reset_tokens |
| servers               |
| sitesettings          |
| sliderimages          |
| smtp_settings         |
| sourcebans_settings   |
| telegramsubscribers   |
| user_admin_privileges |
| user_vip_privileges   |
| users                 |
| vip_settings          |
| vip_tariff_options    |
| vip_tariffs           |
| yoomoney_settings     |
+-----------------------+

+-----------------------+---------------------------+---------------+-------------+------------+----------------------------+
| TABLE_NAME            | COLUMN_NAME               | COLUMN_TYPE   | IS_NULLABLE | COLUMN_KEY | COLUMN_DEFAULT             |
+-----------------------+---------------------------+---------------+-------------+------------+----------------------------+
| __efmigrationshistory | MigrationId               | varchar(150)  | NO          | PRI        | NULL                       |
| __efmigrationshistory | ProductVersion            | varchar(32)   | NO          |            | NULL                       |
| admin_tariff_groups   | id                        | int           | NO          | PRI        | NULL                       |
| admin_tariff_groups   | server_id                 | int           | NO          | MUL        | NULL                       |
| admin_tariff_groups   | name                      | varchar(100)  | NO          |            | NULL                       |
| admin_tariff_groups   | description               | varchar(1000) | NO          |            | NULL                       |
| admin_tariff_groups   | order                     | int           | NO          |            | NULL                       |
| admin_tariff_groups   | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| admin_tariff_groups   | created_at                | datetime(6)   | NO          |            | NULL                       |
| admin_tariff_options  | id                        | int           | NO          | PRI        | NULL                       |
| admin_tariff_options  | tariff_id                 | int           | NO          | MUL        | NULL                       |
| admin_tariff_options  | duration_days             | int           | NO          |            | NULL                       |
| admin_tariff_options  | price                     | decimal(10,2) | NO          |            | NULL                       |
| admin_tariff_options  | order                     | int           | NO          |            | NULL                       |
| admin_tariff_options  | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| admin_tariff_options  | created_at                | datetime(6)   | NO          |            | NULL                       |
| admin_tariff_options  | requires_password         | tinyint(1)    | NO          |            | 1                          |
| admin_tariffs         | id                        | int           | NO          | PRI        | NULL                       |
| admin_tariffs         | server_id                 | int           | NO          | MUL        | NULL                       |
| admin_tariffs         | name                      | varchar(100)  | NO          |   
         | NULL                       |
| admin_tariffs         | description               | varchar(1000) | NO          |            | NULL                       |
| admin_tariffs         | flags                     | varchar(100)  | YES         |            | NULL                       |
| admin_tariffs         | group_name                | varchar(100)  | YES         |            | NULL                       |
| admin_tariffs         | immunity                  | int           | NO          |            | NULL                       |
| admin_tariffs         | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| admin_tariffs         | order                     | int           | NO          |            | NULL                       |
| admin_tariffs         | created_at                | datetime(6)   | NO          |            | NULL                       |
| admin_tariffs         | AdminTariffGroupId        | int           | YES         | MUL        | NULL                       |
| blockedips            | Id                        | int           | NO          | PRI        | NULL                       |
| blockedips            | IpAddress                 | varchar(45)   | NO          | UNI        | NULL                       |
| blockedips            | Reason                    | longtext      | YES         |   
         | NULL                       |
| blockedips            | BlockedAt                 | datetime(6)   | NO          |            | NULL                       |
| blockedips            | BlockedByUserId           | int           | YES         | MUL        | NULL                       |
| custompagemedia       | Id                        | int           | NO          | PRI        | NULL                       |
| custompagemedia       | CustomPageId              | int           | NO          | MUL        | NULL                       |
| custompagemedia       | MediaUrl                  | longtext      | NO          |            | NULL                       |
| custompagemedia       | MediaType                 | varchar(20)   | NO          |   
         | NULL                       |
| custompagemedia       | Order                     | int           | NO          |            | NULL                       |
| custompages           | Id                        | int           | NO          | PRI        | NULL                       |
| custompages           | Title                     | varchar(200)  | NO          |            | NULL                       |
| custompages           | Content                   | longtext      | NO          |            | NULL                       |
| custompages           | Summary                   | varchar(500)  | YES         |            | NULL                       |
| custompages           | Slug                      | varchar(200)  | NO          | UNI        | NULL                       |
| custompages           | CoverImage                | longtext      | YES         |   
         | NULL                       |
| custompages           | AuthorId                  | int           | NO          | MUL        | NULL                       |
| custompages           | IsPublished               | tinyint(1)    | NO          |            | NULL                       |
| custompages           | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| custompages           | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| custompages           | ViewCount                 | int           | NO          |            | NULL                       |
| custompageviews       | Id                        | int           | NO          | PRI        | NULL                       |
| custompageviews       | CustomPageId              | int           | NO          | MUL        | NULL                       |
| custompageviews       | UserId                    | int           | YES         | MUL        | NULL                       |
| custompageviews       | IpAddress                 | varchar(45)   | YES         |            | NULL                       |
| custompageviews       | ViewDate                  | datetime(6)   | NO          |            | NULL                       |
| donation_packages     | id                        | int           | NO          | PRI        | NULL                       |
| donation_packages     | title                     | varchar(200)  | NO          |            | NULL                       |
| donation_packages     | description               | varchar(1000) | NO          |            | NULL                       |
| donation_packages     | suggested_amounts         | varchar(500)  | YES         |            | NULL                       |
| donation_packages     | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| donation_packages     | created_at                | datetime(6)   | NO          |            | NULL                       |
| donation_packages     | updated_at                | datetime(6)   | NO          |            | NULL                       |
| donation_transactions | id                        | int           | NO          | PRI        | NULL                       |
| donation_transactions | transaction_id            | varchar(255)  | NO          | UNI        | NULL                       |
| donation_transactions | user_id                   | int           | YES         | MUL        | NULL                       |
| donation_transactions | steam_id                  | varchar(50)   | YES         | MUL        | NULL                       |
| donation_transactions | amount                    | decimal(10,2) | NO          |            | NULL                       |
| donation_transactions | type                      | varchar(20)   | NO          |            | NULL                       |
| donation_transactions | tariff_id                 | int           | YES         | MUL        | NULL                       |
| donation_transactions | server_id                 | int           | YES         | MUL        | NULL                       |
| donation_transactions | status                    | varchar(20)   | NO          |            | NULL                       |
| donation_transactions | payment_method            | varchar(50)   | YES         |            | NULL                       |
| donation_transactions | label                     | varchar(255)  | YES         |   
         | NULL                       |
| donation_transactions | expires_at                | datetime(6)   | YES         |            | NULL                       |
| donation_transactions | created_at                | datetime(6)   | NO          |            | NULL                       |
| donation_transactions | completed_at              | datetime(6)   | YES         |            | NULL                       |
| donation_transactions | cancelled_at              | datetime(6)   | YES         |            | NULL                       |
| donation_transactions | payment_url               | varchar(1000) | YES         |   
         | NULL                       |
| donation_transactions | pending_expires_at        | datetime(6)   | YES         |            | NULL                       |
| donation_transactions | tariff_option_id          | int           | YES         | MUL        | NULL                       |
| donation_transactions | admin_password            | varchar(100)  | YES         |            | NULL                       |
| donation_transactions | privilege_id              | int           | YES         | MUL        | NULL                       |
| donation_transactions | vip_tariff_id             | int           | YES         | MUL        | NULL                       |
| donation_transactions | vip_tariff_option_id      | int           | YES         | MUL        | NULL                       |
| eventcomments         | Id                        | int           | NO          | PRI        | NULL                       |
| eventcomments         | EventId                   | int           | NO          | MUL        | NULL                       |
| eventcomments         | UserId                    | int           | NO          | MUL        | NULL                       |
| eventcomments         | Content                   | varchar(2000) | NO          |            | NULL                       |
| eventcomments         | ParentCommentId           | int           | YES         | MUL        | NULL                       |
| eventcomments         | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| eventcomments         | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| eventlikes            | Id                        | int           | NO          | PRI        | NULL                       |
| eventlikes            | EventId                   | int           | NO          | MUL        | NULL                       |
| eventlikes            | UserId                    | int           | NO          | MUL        | NULL                       |
| eventlikes            | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| eventmedia            | Id                        | int           | NO          | PRI        | NULL                       |
| eventmedia            | EventId                   | int           | NO          | MUL        | NULL                       |
| eventmedia            | MediaUrl                  | longtext      | NO          |            | NULL                       |
| eventmedia            | MediaType                 | varchar(20)   | NO          |            | NULL                       |
| eventmedia            | Order                     | int           | NO          |            | NULL                       |
| events                | Id                        | int           | NO          | PRI        | NULL                       |
| events                | Title                     | varchar(200)  | NO          |            | NULL                       |
| events                | Content                   | longtext      | NO          |            | NULL                       |
| events                | Summary                   | varchar(500)  | YES         |            | NULL                       |
| events                | Slug                      | varchar(200)  | NO          | UNI        | NULL                       |
| events                | CoverImage                | longtext      | YES         |            | NULL                       |
| events                | AuthorId                  | int           | NO          | MUL        | NULL                       |
| events                | IsPublished               | tinyint(1)    | NO          |            | NULL                       |
| events                | StartDate                 | datetime(6)   | NO          |   
         | NULL                       |
| events                | EndDate                   | datetime(6)   | NO          |            | NULL                       |
| events                | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| events                | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| events                | ViewCount                 | int           | NO          |            | NULL                       |
| events                | LikeCount                 | int           | NO          |            | NULL                       |
| events                | CommentCount              | int           | NO          |            | NULL                       |
| events                | IsEndNotificationSent     | tinyint(1)    | NO          |            | 0                          |
| events                | IsStartNotificationSent   | tinyint(1)    | NO          |            | 0                          |
| events                | IsCreatedNotificationSent | tinyint(1)    | NO          |   
         | 0                          |
| eventviews            | Id                        | int           | NO          | PRI        | NULL                       |
| eventviews            | EventId                   | int           | NO          | MUL        | NULL                       |
| eventviews            | UserId                    | int           | YES         | MUL        | NULL                       |
| eventviews            | IpAddress                 | varchar(45)   | YES         |            | NULL                       |
| eventviews            | ViewDate                  | datetime(6)   | NO          |            | NULL                       |
| news                  | Id                        | int           | NO          | PRI        | NULL                       |
| news                  | Title                     | varchar(200)  | NO          |            | NULL                       |
| news                  | Content                   | longtext      | NO          |            | NULL                       |
| news                  | Summary                   | varchar(500)  | YES         |            | NULL                       |
| news                  | Slug                      | varchar(200)  | NO          | UNI        | NULL                       |
| news                  | CoverImage                | longtext      | YES         |            | NULL                       |
| news                  | AuthorId                  | int           | NO          | MUL        | NULL                       |
| news                  | IsPublished               | tinyint(1)    | NO          |            | NULL                       |
| news                  | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| news                  | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| news                  | ViewCount                 | int           | NO          |            | NULL                       |
| news                  | LikeCount                 | int           | NO          |            | NULL                       |
| news                  | CommentCount              | int           | NO          |            | NULL                       |
| newscomments          | Id                        | int           | NO          | PRI        | NULL                       |
| newscomments          | NewsId                    | int           | NO          | MUL        | NULL                       |
| newscomments          | UserId                    | int           | NO          | MUL        | NULL                       |
| newscomments          | Content                   | varchar(2000) | NO          |            | NULL                       |
| newscomments          | ParentCommentId           | int           | YES         | MUL        | NULL                       |
| newscomments          | CreatedAt                 | datetime(6)   | NO          |   
         | NULL                       |
| newscomments          | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| newslikes             | Id                        | int           | NO          | PRI        | NULL                       |
| newslikes             | NewsId                    | int           | NO          | MUL        | NULL                       |
| newslikes             | UserId                    | int           | NO          | MUL        | NULL                       |
| newslikes             | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| newsmedia             | Id                        | int           | NO          | PRI        | NULL                       |
| newsmedia             | NewsId                    | int           | NO          | MUL        | NULL                       |
| newsmedia             | MediaUrl                  | longtext      | NO          |            | NULL                       |
| newsmedia             | MediaType                 | varchar(20)   | NO          |            | NULL                       |
| newsmedia             | Order                     | int           | NO          |            | NULL                       |
| newsviews             | Id                        | int           | NO          | PRI        | NULL                       |
| newsviews             | NewsId                    | int           | NO          | MUL        | NULL                       |
| newsviews             | UserId                    | int           | YES         | MUL        | NULL                       |
| newsviews             | IpAddress                 | varchar(45)   | YES         |   
         | NULL                       |
| newsviews             | ViewDate                  | datetime(6)   | NO          |            | NULL                       |
| notifications         | Id                        | int           | NO          | PRI        | NULL                       |
| notifications         | UserId                    | int           | NO          | MUL        | NULL                       |
| notifications         | Title                     | varchar(200)  | NO          |            | NULL                       |
| notifications         | Message                   | varchar(1000) | NO          |            | NULL                       |
| notifications         | Type                      | varchar(50)   | NO          |            | NULL                       |
| notifications         | IsRead                    | tinyint(1)    | NO          |            | NULL                       |
| notifications         | RelatedEntityId           | int           | YES         |            | NULL                       |
| notifications         | CreatedAt                 | datetime(6)   | NO          | MUL        | NULL                       |
| password_reset_tokens | id                        | int           | NO          | PRI        | NULL                       |
| password_reset_tokens | token                     | varchar(500)  | NO          | MUL        | NULL                       |
| password_reset_tokens | user_id                   | int           | NO          | MUL        | NULL                       |
| password_reset_tokens | expires_at                | datetime(6)   | NO          |            | NULL                       |
| password_reset_tokens | is_used                   | tinyint(1)    | NO          |            | NULL                       |
| password_reset_tokens | created_at                | datetime(6)   | NO          |            | NULL                       |
| password_reset_tokens | used_at                   | datetime(6)   | YES         |   
         | NULL                       |
| servers               | Id                        | int           | NO          | PRI        | NULL                       |
| servers               | Name                      | varchar(100)  | NO          |            | NULL                       |
| servers               | IpAddress                 | longtext      | NO          |            | NULL                       |
| servers               | Port                      | int           | NO          |            | NULL                       |
| servers               | MapName                   | longtext      | NO          |            | NULL                       |
| servers               | CurrentPlayers            | int           | NO          |            | NULL                       |
| servers               | MaxPlayers                | int           | NO          |            | NULL                       |
| servers               | IsOnline                  | tinyint(1)    | NO          |            | NULL                       |
| servers               | LastChecked               | datetime(6)   | NO          |            | NULL                       |
| servers               | rcon_password             | varchar(255)  | YES         |            | NULL                       |
| sitesettings          | Id                        | int           | NO          | PRI        | NULL                       |
| sitesettings          | Key                       | varchar(255)  | NO          | UNI        | NULL                       |
| sitesettings          | Value                     | longtext      | NO          |            | NULL                       |
| sitesettings          | Category                  | varchar(255)  | NO          | MUL        | NULL                       |
| sitesettings          | Description               | longtext      | NO          |            | NULL                       |
| sitesettings          | DataType                  | longtext      | NO          |            | NULL                       |
| sitesettings          | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| sitesettings          | UpdatedAt                 | datetime(6)   | NO          |            | NULL                       |
| sliderimages          | Id                        | int           | NO          | PRI        | NULL                       |
| sliderimages          | ImageUrl                  | longtext      | NO          |            | NULL                       |
| sliderimages          | Title                     | varchar(100)  | NO          |            | NULL                       |
| sliderimages          | Description               | varchar(500)  | NO          |            | NULL                       |
| sliderimages          | Order                     | int           | NO          |            | NULL                       |
| sliderimages          | CreatedAt                 | datetime(6)   | NO          |            | NULL                       |
| sliderimages          | Buttons                   | longtext      | YES         |            | NULL                       |
| smtp_settings         | id                        | int           | NO          | PRI        | NULL                       |
| smtp_settings         | host                      | varchar(255)  | NO          |            | NULL                       |
| smtp_settings         | port                      | int           | NO          |            | NULL                       |
| smtp_settings         | username                  | varchar(255)  | NO          |            | NULL                       |
| smtp_settings         | password                  | varchar(500)  | NO          |            | NULL                       |
| smtp_settings         | enable_ssl                | tinyint(1)    | NO          |            | NULL                       |
| smtp_settings         | from_email                | varchar(255)  | NO          |            | NULL                       |
| smtp_settings         | from_name                 | varchar(255)  | NO          |            | NULL                       |
| smtp_settings         | is_configured             | tinyint(1)    | NO          |            | NULL                       |
| smtp_settings         | updated_at                | datetime(6)   | NO          |            | NULL                       |
| sourcebans_settings   | id                        | int           | NO          | PRI        | NULL                       |
| sourcebans_settings   | host                      | varchar(255)  | NO          |            | NULL                       |
| sourcebans_settings   | port                      | int           | NO          |            | NULL                       |
| sourcebans_settings   | database                  | varchar(100)  | NO          |   
         | NULL                       |
| sourcebans_settings   | username                  | varchar(100)  | NO          |            | NULL                       |
| sourcebans_settings   | password                  | varchar(500)  | NO          |            | NULL                       |
| sourcebans_settings   | is_configured             | tinyint(1)    | NO          |   
         | NULL                       |
| sourcebans_settings   | updated_at                | datetime(6)   | NO          |            | NULL                       |
| sourcebans_settings   | created_at                | datetime(6)   | NO          |            | 0001-01-01 00:00:00.000000 |
| sourcebans_settings   | server_id                 | int           | NO          | MUL        | 0                          |
| telegramsubscribers   | Id                        | int           | NO          | PRI        | NULL                       |
| telegramsubscribers   | ChatId                    | bigint        | NO          | UNI        | NULL                       |
| telegramsubscribers   | UserId                    | int           | YES         | MUL        | NULL                       |
| telegramsubscribers   | Username                  | longtext      | YES         |            | NULL                       |
| telegramsubscribers   | FirstName                 | longtext      | YES         |            | NULL                       |
| telegramsubscribers   | LastName                  | longtext      | YES         |   
         | NULL                       |
| telegramsubscribers   | IsActive                  | tinyint(1)    | NO          |            | NULL                       |
| telegramsubscribers   | SubscribedAt              | datetime(6)   | NO          |            | NULL                       |
| user_admin_privileges | id                        | int           | NO          | PRI        | NULL                       |
| user_admin_privileges | user_id                   | int           | NO          | MUL        | NULL                       |
| user_admin_privileges | steam_id                  | varchar(50)   | NO          | MUL        | NULL                       |
| user_admin_privileges | server_id                 | int           | NO          | MUL        | NULL                       |
| user_admin_privileges | tariff_id                 | int           | NO          | MUL        | NULL                       |
| user_admin_privileges | transaction_id            | int           | YES         | MUL        | NULL                       |
| user_admin_privileges | flags                     | varchar(100)  | YES         |            | NULL                       |
| user_admin_privileges | group_name                | varchar(100)  | YES         |            | NULL                       |
| user_admin_privileges | immunity                  | int           | NO          |            | NULL                       |
| user_admin_privileges | starts_at                 | datetime(6)   | NO          |            | NULL                       |
| user_admin_privileges | expires_at                | datetime(6)   | NO          | MUL        | NULL                       |
| user_admin_privileges | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| user_admin_privileges | sourcebans_admin_id       | int           | YES         |   
         | NULL                       |
| user_admin_privileges | created_at                | datetime(6)   | NO          |            | NULL                       |
| user_admin_privileges | tariff_option_id          | int           | YES         | MUL        | NULL                       |
| user_admin_privileges | admin_password            | varchar(100)  | YES         |            | NULL                       |
| user_admin_privileges | updated_at                | datetime(6)   | NO          |            | 0001-01-01 00:00:00.000000 |
| user_vip_privileges   | id                        | int           | NO          | PRI        | NULL                       |
| user_vip_privileges   | user_id                   | int           | NO          | MUL        | NULL                       |
| user_vip_privileges   | steam_id                  | varchar(50)   | NO          | MUL        | NULL                       |
| user_vip_privileges   | server_id                 | int           | NO          | MUL        | NULL                       |
| user_vip_privileges   | tariff_id                 | int           | NO          | MUL        | NULL                       |
| user_vip_privileges   | tariff_option_id          | int           | YES         | MUL        | NULL                       |
| user_vip_privileges   | group_name                | varchar(64)   | NO          |            | NULL                       |
| user_vip_privileges   | starts_at                 | datetime(6)   | NO          |            | NULL                       |
| user_vip_privileges   | expires_at                | datetime(6)   | NO          | MUL        | NULL                       |
| user_vip_privileges   | is_active                 | tinyint(1)    | NO          |   
         | NULL                       |
| user_vip_privileges   | transaction_id            | int           | YES         | MUL        | NULL                       |
| user_vip_privileges   | created_at                | datetime(6)   | NO          |            | NULL                       |
| user_vip_privileges   | updated_at                | datetime(6)   | NO          |            | NULL                       |
| users                 | Id                        | int           | NO          | PRI        | NULL                       |
| users                 | Username                  | varchar(50)   | NO          | UNI        | NULL                       |
| users                 | Email                     | varchar(255)  | NO          | UNI        | NULL                       |
| users                 | PasswordHash              | longtext      | NO          |            | NULL                       |
| users                 | AvatarUrl                 | longtext      | YES         |            | NULL                       |
| users                 | IsAdmin                   | tinyint(1)    | NO          |            | NULL                       |
| users                 | CreatedAt                 | datetime(6)   | NO          |   
         | NULL                       |
| users                 | SteamId                   | varchar(50)   | YES         |            | NULL                       |
| users                 | SteamProfileUrl           | longtext      | YES         |            | NULL                       |
| users                 | LastIp                    | longtext      | YES         |   
         | NULL                       |
| users                 | BlockReason               | longtext      | YES         |            | NULL                       |
| users                 | BlockedAt                 | datetime(6)   | YES         |            | NULL                       |
| users                 | IsBlocked                 | tinyint(1)    | NO          |            | 0                          |
| vip_settings          | id                        | int           | NO          | PRI        | NULL                       |
| vip_settings          | server_id                 | int           | NO          | MUL        | NULL                       |
| vip_settings          | host                      | varchar(255)  | NO          |            | NULL                       |
| vip_settings          | port                      | int           | NO          |            | NULL                       |
| vip_settings          | database                  | varchar(100)  | NO          |            | NULL                       |
| vip_settings          | username                  | varchar(100)  | NO          |            | NULL                       |
| vip_settings          | password                  | varchar(500)  | NO          |            | NULL                       |
| vip_settings          | is_configured             | tinyint(1)    | NO          |            | NULL                       |
| vip_settings          | created_at                | datetime(6)   | NO          |            | NULL                       |
| vip_settings          | updated_at                | datetime(6)   | NO          |            | NULL                       |
| vip_tariff_options    | id                        | int           | NO          | PRI        | NULL                       |
| vip_tariff_options    | tariff_id                 | int           | NO          | MUL        | NULL                       |
| vip_tariff_options    | duration_days             | int           | NO          |            | NULL                       |
| vip_tariff_options    | price                     | decimal(10,2) | NO          |            | NULL                       |
| vip_tariff_options    | order                     | int           | NO          |            | NULL                       |
| vip_tariff_options    | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| vip_tariff_options    | created_at                | datetime(6)   | NO          |            | NULL                       |
| vip_tariffs           | id                        | int           | NO          | PRI        | NULL                       |
| vip_tariffs           | server_id                 | int           | NO          | MUL        | NULL                       |
| vip_tariffs           | name                      | varchar(100)  | NO          |            | NULL                       |
| vip_tariffs           | description               | varchar(1000) | NO          |            | NULL                       |
| vip_tariffs           | group_name                | varchar(64)   | NO          |            | NULL                       |
| vip_tariffs           | is_active                 | tinyint(1)    | NO          |            | NULL                       |
| vip_tariffs           | order                     | int           | NO          |            | NULL                       |
| vip_tariffs           | created_at                | datetime(6)   | NO          |            | NULL                       |
| yoomoney_settings     | id                        | int           | NO          | PRI        | NULL                       |
| yoomoney_settings     | wallet_number             | varchar(100)  | NO          |            | NULL                       |
| yoomoney_settings     | secret_key                | varchar(500)  | NO          |            | NULL                       |
| yoomoney_settings     | is_configured             | tinyint(1)    | NO          |            | NULL                       |
| yoomoney_settings     | updated_at                | datetime(6)   | NO          |   
         | NULL                       |
+-----------------------+---------------------------+---------------+-------------+------------+----------------------------+