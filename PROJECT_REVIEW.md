# SibGamer - Полный обзор проекта

> **Последнее обновление:** 21 декабря 2025  
> **Версия:** 3.0  
> **Статус:** ✅ Развёрнуто

---

## 🌐 Деплой

| Сервис | URL | Статус |
|--------|-----|--------|
| **GitHub** | [github.com/Zafarovpolat/sibgamer](https://github.com/Zafarovpolat/sibgamer) | ✅ |
| **Frontend** | [sibgamer-front.onrender.com](https://sibgamer-front.onrender.com) | ✅ |
| **Backend** | [sibgamer.onrender.com](https://sibgamer.onrender.com) | ✅ |
| **Database** | [Supabase](https://supabase.com/dashboard/project/oktzzeertnqlhrrvisqs) | ✅ |

---

## 📋 Общее описание проекта

**SibGamer** - портал игрового сообщества для Counter-Strike серверов с системой новостей, мероприятий, донатов, VIP/Admin привилегий и Telegram-уведомлений.

### Технологический стек

| Компонент | Технология | Версия |
|-----------|------------|--------|
| **Backend** | ASP.NET Core | 9.0 |
| **Frontend** | React + Vite + TypeScript | 19.1.1 |
| **База данных** | PostgreSQL (Supabase) | 15 |
| **Редактор контента** | Tiptap | 3.7.2 |
| **Стилизация** | TailwindCSS | 3.4.18 |
| **Состояние** | Zustand | 5.0.8 |
| **Запросы** | TanStack Query | 5.90.5 |
| **Telegram** | Telegram.Bot | 22.3.0 |

---

## 🏗️ Архитектура проекта

### Структура каталогов

```
SibGamer/
├── backend/                    # ASP.NET Core API
│   ├── BackgroundServices/     # Фоновые сервисы (7 файлов)
│   ├── Controllers/            # API контроллеры (15 + 10 admin)
│   ├── Data/                   # DbContext и DTO
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Middleware/             # IP блокировка
│   ├── Migrations/             # EF Core миграции
│   ├── Models/                 # Модели данных (16 файлов)
│   ├── Services/               # Бизнес-логика (13 сервисов)
│   ├── Utils/                  # Утилиты (JSON, Slug, DateTime)
│   ├── db/                     # SQL схема
│   └── wwwroot/                # Статические файлы (uploads)
│
└── frontend/                   # React SPA
    ├── public/                 # Публичные ресурсы
    └── src/
        ├── components/         # React компоненты (20 файлов)
        ├── config/             # Конфигурация API
        ├── hooks/              # Кастомные хуки
        ├── lib/                # Axios, auth, media утилиты
        ├── pages/              # Страницы (12 public + 14 admin)
        ├── store/              # Zustand store
        ├── types/              # TypeScript типы
        └── utils/              # Утилиты
```

---

## 📊 Детальный анализ компонентов

### Backend Controllers (25 контроллеров)

#### Публичные API:
| Контроллер | Описание |
|------------|----------|
| `AuthController` | Регистрация, логин, сброс пароля |
| `NewsController` | CRUD новостей, комментарии, лайки |
| `EventsController` | CRUD мероприятий |
| `DonationController` | Система донатов, тарифы |
| `ProfileController` | Профиль пользователя |
| `NotificationsController` | Уведомления пользователей |
| `CustomPagesController` | Кастомные страницы (public) |
| `ServersController` | Информация о серверах |
| `SettingsController` | Настройки сайта |
| `UploadController` | Загрузка файлов |
| `YooMoneyWebhookController` | Обработка платежей YooMoney |

#### Admin API (/api/admin/):
| Контроллер | Описание |
|------------|----------|
| `AdminDonationController` | Управление донатами |
| `AdminUsersController` | Управление пользователями |
| `AdminEventsController` | Управление мероприятиями |
| `AdminNewsController` | Управление новостями |
| `AdminEmailController` | SMTP настройки |
| `AdminServersController` | Управление серверами |
| `AdminSettingsController` | Общие настройки |
| `AdminSliderController` | Управление слайдером |
| `AdminTelegramController` | Telegram настройки |
| `AdminNotificationsController` | Массовые уведомления |
| `CustomPagesController (Admin)` | Управление страницами |

### Фоновые сервисы (7 сервисов)

| Сервис | Функционал |
|--------|------------|
| `TelegramBotBackgroundService` | Telegram бот (подписка /start, /stop) |
| `PrivilegeExpirationService` | Истечение VIP/Admin привилегий |
| `EventNotificationBackgroundService` | Уведомления о мероприятиях |
| `ServerMonitoringService` | Мониторинг игровых серверов |
| `VipSyncBackgroundService` | Синхронизация VIP с SourceBans |
| `AdminSyncBackgroundService` | Синхронизация Admin с SourceBans |
| `PendingPaymentCancellationService` | Отмена неоплаченных транзакций |

### База данных (35+ таблиц)

#### Основные сущности:
- `users` - пользователи (с SteamID)
- `news`, `newscomments`, `newslikes`, `newsmedia`, `newsviews`
- `events`, `eventcomments`, `eventlikes`, `eventmedia`, `eventviews`
- `custompages`, `custompagemedia`, `custompageviews`
- `servers` - игровые сервера
- `sliderimages` - слайдер на главной

#### Донаты и привилегии:
- `donation_packages`, `donation_transactions`
- `vip_tariffs`, `vip_tariff_options`
- `admin_tariff_groups`, `admin_tariffs`, `admin_tariff_options`
- `user_vip_privileges`, `user_admin_privileges`

#### Настройки и интеграции:
- `sitesettings` - key-value настройки
- `smtp_settings` - SMTP конфигурация
- `yoomoney_settings` - настройки платежей
- `sourcebans_settings` - интеграция с SourceBans
- `vip_settings` - настройки VIP
- `telegramsubscribers` - подписчики Telegram

### Frontend страницы

#### Публичные (12 страниц):
- `Home.tsx` - главная страница
- `News.tsx`, `NewsDetail.tsx` - новости
- `Events.tsx`, `EventDetail.tsx` - мероприятия
- `Donate.tsx` - страница донатов
- `Profile.tsx` - профиль пользователя
- `MemberApplication.tsx` - заявка на членство
- `Notifications.tsx` - уведомления
- `CustomPageDetail.tsx` - кастомная страница
- `ResetPassword.tsx` - сброс пароля
- `DonationSuccess.tsx` - успешный донат

#### Admin панель (14 страниц):
- `AdminDashboard.tsx` - дашборд
- `AdminDonationTariffs.tsx` - тарифы
- `AdminDonationSettings.tsx` - настройки донатов
- `AdminDonationMonitoring.tsx` - мониторинг
- `AdminEmail.tsx` - email настройки
- `AdminEvents.tsx` - мероприятия
- `AdminSlider.tsx` - слайдер
- `AdminNews.tsx` - новости
- `AdminCustomPages.tsx` - страницы
- `AdminVipApplications.tsx` - VIP заявки
- `AdminServers.tsx` - сервера
- `AdminUsers.tsx` - пользователи
- `AdminTgNotifications.tsx` - Telegram
- `AdminNotifications.tsx` - уведомления

---

## ✅ Реализованный функционал

### Основные функции
- [x] Регистрация и авторизация (JWT)
- [x] Восстановление пароля через email
- [x] Личный кабинет с историей покупок
- [x] Новости с комментариями и лайками
- [x] Мероприятия с датами начала/окончания
- [x] Кастомные страницы
- [x] Слайдер на главной

### Донаты и привилегии
- [x] VIP тарифы с опциями (сроки, цены)
- [x] Admin тарифы с группами
- [x] YooMoney интеграция
- [x] Автоматическая выдача привилегий
- [x] Синхронизация с SourceBans++
- [x] Автоматическое истечение привилегий

### Telegram
- [x] Telegram бот для уведомлений
- [x] Подписка/отписка через /start, /stop
- [x] Уведомления о новостях и событиях
- [x] Уведомления об истечении привилегий

### Админ-панель
- [x] Управление всем контентом
- [x] Мониторинг транзакций
- [x] Настройки SMTP, YooMoney, Telegram
- [x] Блокировка пользователей и IP

---

## 🔧 Рекомендации по развитию

### Приоритет 1 (Улучшения)
- [ ] Улучшить RichTextEditor (модальные окна вместо prompt)
- [ ] Добавить drag-and-drop для изображений
- [ ] Добавить систему разделов для страниц

### Приоритет 2 (Новые функции)
- [ ] Интеграция с форумом
- [ ] Система достижений
- [ ] Промокоды и скидки

### Приоритет 3 (Технический долг)
- [ ] Рефакторинг больших контроллеров
- [ ] Добавить кэширование (Redis)
- [ ] Автоматические тесты
- [ ] Docker-контейнеризация

---

## 🔐 Environment Variables

### Backend (Render)
```env
ConnectionStrings__DefaultConnection=Host=xxx.supabase.co;Database=postgres;Username=postgres;Password=xxx
Jwt__Key=xxx
Jwt__Issuer=SibGamer
Jwt__Audience=SibGamerUsers
FrontendUrl=https://sibgamer-front.onrender.com
ImageBaseUrl=https://sibgamer.onrender.com
```

### Frontend (Render)
```env
VITE_API_URL=https://sibgamer.onrender.com/api
VITE_BASE_URL=https://sibgamer-front.onrender.com
VITE_IMAGE_BASE_URL=https://sibgamer.onrender.com
VITE_SERVER_TZ_OFFSET=180
```

---

## 📚 Дополнительная документация

- [README.md](./README.md) — быстрый старт
- [Tables.md](./Tables.md) — структура базы данных
- [CLIENT_GUIDE.md](./CLIENT_GUIDE.md) — руководство для заказчика

---

*Техническая документация проекта SibGamer*