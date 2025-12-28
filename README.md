# SibGamer

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![React](https://img.shields.io/badge/React-19-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-blue)
![License](https://img.shields.io/badge/license-MIT-green)

Портал игрового сообщества для Counter-Strike серверов с системой новостей, мероприятий, донатов, VIP/Admin привилегий и Telegram-уведомлений.

---

## 🌐 Продакшн

| Сервис | URL |
|--------|-----|
| **Фронтенд** | [sibgamer-front.onrender.com](https://sibgamer-front.onrender.com) |
| **Бэкенд API** | [sibgamer.onrender.com](https://sibgamer.onrender.com) |
| **Swagger Docs** | [sibgamer.onrender.com/swagger](https://sibgamer.onrender.com/swagger) |
| **GitHub** | [github.com/Zafarovpolat/sibgamer](https://github.com/Zafarovpolat/sibgamer) |
| **Neon DB** | [console.neon.tech](https://console.neon.tech) |

---

## 🛠 Технологический стек

| Компонент | Технология | Версия |
|-----------|------------|--------|
| **Backend** | ASP.NET Core | 9.0 |
| **Frontend** | React + Vite + TypeScript | 19.1.1 |
| **База данных** | PostgreSQL (Neon DB) | 15 |
| **Редактор** | Tiptap | 3.7.2 |
| **Стили** | TailwindCSS | 3.4.18 |
| **Состояние** | Zustand | 5.0.8 |
| **Запросы** | TanStack Query | 5.90.5 |
| **Уведомления** | Telegram Bot API | 22.3.0 |

---

## 🚀 Быстрый старт (Локальная разработка)

### Требования

- **.NET SDK** 9.0+
- **Node.js** 18+ (рекомендуется 20+)
- **PostgreSQL** 15+ или Neon DB

---

### 1. Клонирование репозитория

```bash
git clone https://github.com/Zafarovpolat/sibgamer.git
cd sibgamer
```

---

### 2. Настройка Backend

Создайте `backend/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sibgamer;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "SibGamer",
    "Audience": "SibGamerUsers",
    "ExpireMinutes": 10080
  },
  "FrontendUrl": "http://localhost:5173",
  "ImageBaseUrl": "http://localhost:5000"
}
```

Запуск:
```bash
cd backend
dotnet restore
dotnet ef database update  # Применить миграции
dotnet run
```

📍 Backend: **http://localhost:5000**  
📖 Swagger: **http://localhost:5000/swagger**

---

### 3. Настройка Frontend

Создайте `frontend/.env`:

```env
VITE_API_URL=http://localhost:5000/api
VITE_BASE_URL=http://localhost:5173
VITE_IMAGE_BASE_URL=http://localhost:5000
VITE_SERVER_TZ_OFFSET=180
```

Запуск:
```bash
cd frontend
npm install
npm run dev
```

📍 Frontend: **http://localhost:5173**

---

### 4. Создание администратора

1. Зарегистрируйтесь через UI
2. Выполните SQL (замените email):
```sql
UPDATE users SET is_admin = true WHERE email = 'admin@example.com';
```

---

## 📁 Структура проекта

```
SibGamer/
├── backend/                    # ASP.NET Core API
│   ├── BackgroundServices/     # Фоновые сервисы (7)
│   ├── Controllers/            # API контроллеры (26)
│   ├── Data/                   # DbContext
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Middleware/             # IP блокировка
│   ├── Models/                 # Модели данных (17)
│   ├── Services/               # Бизнес-логика (13)
│   └── Migrations/             # EF Core миграции
│
└── frontend/                   # React SPA
    └── src/
        ├── components/         # UI компоненты (28)
        ├── pages/              # Страницы (12 public + 15 admin)
        ├── hooks/              # Custom hooks
        ├── store/              # Zustand store
        └── lib/                # Утилиты
```

---

## 🔧 Troubleshooting

| Проблема | Решение |
|----------|---------|
| CORS ошибка | Проверьте `FrontendUrl` в `appsettings.json` |
| 500 на API | Проверьте миграции: `dotnet ef database update` |
| Изображения не грузятся | Проверьте `ImageBaseUrl` и `VITE_IMAGE_BASE_URL` |
| Telegram бот не работает | Проверьте токен в админ-панели |

---

## 📋 Документация

- [PROJECT_REVIEW.md](./PROJECT_REVIEW.md) — технический обзор
- [Tables.md](./Tables.md) — структура базы данных (36 таблиц)
- [CLIENT_GUIDE.md](./CLIENT_GUIDE.md) — руководство для заказчика

---

*© 2025 SibGamer. Все права защищены.*