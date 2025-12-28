# SibGamer - Полный обзор проекта

> **Последнее обновление:** 28 декабря 2025  
> **Версия:** 5.0  
> **Статус:** ✅ Развёрнуто на Render + Neon DB

---

## 🌐 Деплой

| Сервис | URL | Статус |
|--------|-----|--------|
| **GitHub** | [github.com/Zafarovpolat/sibgamer](https://github.com/Zafarovpolat/sibgamer) | ✅ |
| **Frontend** | [sibgamer-front.onrender.com](https://sibgamer-front.onrender.com) | ✅ |
| **Backend** | [sibgamer.onrender.com](https://sibgamer.onrender.com) | ✅ |
| **Database** | [Neon DB](https://console.neon.tech) (PostgreSQL 15) | ✅ |
| **Telegram Bot** | [@sibgamer_notify_bot](https://t.me/sibgamer_notify_bot) | ✅ |

---

## 📋 Общее описание проекта

**SibGamer** — игровой портал сообщества Counter-Strike серверов с полноценной системой контента, донатов, VIP/Admin привилегий и Telegram-уведомлений.

### 🔧 Технологический стек

| Компонент | Технология | Версия |
|-----------|------------|--------|
| **Backend Framework** | ASP.NET Core | 9.0 |
| **Frontend Framework** | React + Vite | 19.1.1 + 7.1.7 |
| **Язык Frontend** | TypeScript | 5.9.3 |
| **База данных** | PostgreSQL (Neon DB) | 15 |
| **ORM** | Entity Framework Core | 9.0.0 |
| **Редактор контента** | Tiptap | 3.7.2 |
| **Стилизация** | TailwindCSS | 3.4.18 |
| **Состояние** | Zustand | 5.0.8 |
| **Запросы** | TanStack Query | 5.90.5 |
| **Telegram Bot** | Telegram.Bot | 22.3.0 |
| **Аутентификация** | JWT Bearer | 9.0.0 |
| **Хеширование** | BCrypt.Net-Next | 4.0.3 |
| **API Docs** | Swashbuckle (Swagger) | 7.2.0 |

---

## 🏗️ Архитектура проекта

### Структура каталогов

```
SibGamer/
├── backend/                           # ASP.NET Core 9.0 API
│   ├── BackgroundServices/            # 7 фоновых сервисов
│   │   ├── TelegramBotBackgroundService.cs      # Telegram бот (/start, /stop)
│   │   ├── PrivilegeExpirationService.cs        # Истечение VIP/Admin
│   │   ├── EventNotificationBackgroundService.cs # Уведомления о событиях
│   │   ├── ServerMonitoringService.cs           # Мониторинг серверов
│   │   ├── VipSyncBackgroundService.cs          # Синхронизация VIP с SourceBans
│   │   ├── AdminSyncBackgroundService.cs        # Синхронизация Admin с SourceBans
│   │   └── PendingPaymentCancellationService.cs # Отмена неоплаченных транзакций
│   │
│   ├── Controllers/                   # 16 публичных контроллеров
│   │   ├── AuthController.cs          # Регистрация, логин, сброс пароля
│   │   ├── NewsController.cs          # CRUD новостей, комментарии, лайки
│   │   ├── EventsController.cs        # CRUD мероприятий
│   │   ├── DonationController.cs      # Донаты, тарифы, покупки
│   │   ├── ProfileController.cs       # Профиль пользователя
│   │   ├── NotificationsController.cs # Уведомления пользователей
│   │   ├── CustomPagesController.cs   # Кастомные страницы (public)
│   │   ├── NavSectionsController.cs   # Навигация (public)
│   │   ├── ServersController.cs       # Информация о серверах
│   │   ├── SettingsController.cs      # Настройки сайта
│   │   ├── SliderController.cs        # Слайдер страницы
│   │   ├── UploadController.cs        # Загрузка файлов
│   │   ├── YooMoneyWebhookController.cs # Webhook платежей
│   │   ├── VipSyncController.cs       # API синхронизации VIP
│   │   ├── AdminSyncController.cs     # API синхронизации Admin
│   │   └── SystemController.cs        # Системные endpoints
│   │
│   ├── Controllers/Admin/             # 11 admin контроллеров
│   │   ├── AdminDonationController.cs # Донаты, тарифы, SourceBans
│   │   ├── AdminUsersController.cs    # Пользователи, блокировки
│   │   ├── AdminEventsController.cs   # Мероприятия
│   │   ├── AdminEmailController.cs    # SMTP настройки
│   │   ├── AdminServersController.cs  # Игровые сервера
│   │   ├── AdminSettingsController.cs # Общие настройки
│   │   ├── AdminSliderController.cs   # Управление слайдером
│   │   ├── AdminTelegramController.cs # Telegram настройки
│   │   ├── AdminNavSectionsController.cs # Управление навигацией
│   │   ├── AdminNotificationsController.cs # Массовые уведомления
│   │   └── CustomPagesController.cs   # Управление страницами
│   │
│   ├── Services/                      # 13 бизнес-сервисов
│   ├── Models/                        # 17 моделей данных
│   ├── Data/                          # DbContext + DTOs
│   ├── Migrations/                    # 1 EF Core миграция (InitialCreate)
│   └── wwwroot/                       # Статические файлы (uploads)
│
└── frontend/                          # React SPA
    └── src/
        ├── components/                # 28 React компонентов
        ├── pages/                     # 12 public + 15 admin страниц
        ├── lib/                       # API клиенты (navApi.ts и др.)
        └── store/                     # Zustand store
```

---

## 📊 База данных (36 таблиц)

### Основные категории

| Категория | Таблицы |
|-----------|---------|
| **Auth & Users** | users, blocked_ips, password_reset_tokens |
| **Content** | news, events, custom_pages + media/comments/likes/views |
| **Navigation** | nav_sections, nav_section_items |
| **Donations** | donation_packages, donation_transactions |
| **VIP/Admin** | vip_tariffs, admin_tariffs, user_vip_privileges, user_admin_privileges |
| **Servers** | servers, vip_settings, sourcebans_settings |
| **Settings** | site_settings, smtp_settings, yoomoney_settings |

> Подробная структура: [Tables.md](./Tables.md)

---

## ✅ Реализованный функционал

### Аутентификация и пользователи
- [x] Регистрация и авторизация (JWT)
- [x] Восстановление пароля через email
- [x] Привязка Steam аккаунта
- [x] Личный кабинет с историей покупок
- [x] Система ролей (user, admin)
- [x] Блокировка пользователей и IP

### Контент
- [x] Новости с WYSIWYG редактором (Tiptap)
- [x] Комментарии с вложенностью
- [x] Лайки контента
- [x] Подсчёт уникальных просмотров
- [x] Мероприятия с датами начала/окончания
- [x] Кастомные страницы
- [x] Слайдер на главной

### Навигация (NEW)
- [x] Управление разделами меню из админки
- [x] Поддержка типов: ссылка, выпадающий список, страница
- [x] Drag-and-drop сортировка
- [x] Привязка к кастомным страницам

### Донаты и привилегии
- [x] VIP тарифы с опциями (сроки, цены)
- [x] Admin тарифы с группами доступа
- [x] YooMoney интеграция (webhook)
- [x] Автоматическая выдача привилегий
- [x] Синхронизация с SourceBans++
- [x] Автоматическое истечение привилегий

### Telegram бот
- [x] Подписка через /start
- [x] Уведомления о новостях и мероприятиях
- [x] Уведомления об истечении привилегий
- [x] Массовые уведомления из админки

### Админ-панель (15 страниц)
- [x] Dashboard, News, Events, Pages, Slider
- [x] Servers, Users, VIP Applications
- [x] Donation Tariffs, Monitoring, Settings
- [x] SMTP, Telegram, Notifications
- [x] **Nav Sections** (NEW)

---

## 🔐 Environment Variables

### Backend (Render)
```env
# Database (Neon DB)
ConnectionStrings__DefaultConnection=Host=ep-xxx.neon.tech;Database=neondb;Username=xxx;Password=xxx;SslMode=Require

# JWT
Jwt__Key=YOUR_SECRET_KEY_AT_LEAST_32_CHARS
Jwt__Issuer=SibGamer
Jwt__Audience=SibGamerUsers

# URLs
FrontendUrl=https://sibgamer-front.onrender.com
ImageBaseUrl=https://sibgamer.onrender.com

# Telegram (optional)
TelegramBotToken=xxx:xxx
```

### Frontend (Render)
```env
VITE_API_URL=https://sibgamer.onrender.com/api
VITE_BASE_URL=https://sibgamer-front.onrender.com
VITE_IMAGE_BASE_URL=https://sibgamer.onrender.com
VITE_SERVER_TZ_OFFSET=180
```

---

## 📊 Статистика кода

| Метрика | Значение |
|---------|----------|
| **Backend контроллеров** | 27 (16 public + 11 admin) |
| **Backend сервисов** | 13 |
| **Backend моделей** | 17 |
| **Backend миграций** | 1 (InitialCreate) |
| **Frontend страниц** | 27 (12 public + 15 admin) |
| **Frontend компонентов** | 28 |
| **Таблиц в БД** | 36 |

---

## 📚 Дополнительная документация

- [README.md](./README.md) — быстрый старт
- [Tables.md](./Tables.md) — подробная структура БД
- [CLIENT_GUIDE.md](./CLIENT_GUIDE.md) — руководство для заказчика

---

*Техническая документация проекта SibGamer v5.0*