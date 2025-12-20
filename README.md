# SibGamer

Портал игрового сообщества для Counter-Strike серверов с системой новостей, мероприятий, донатов, VIP/Admin привилегий и Telegram-уведомлений.

## 🛠 Технологический стек

| Компонент | Технология |
|-----------|------------|
| **Backend** | ASP.NET Core 8.0 |
| **Frontend** | React 19 + Vite + TypeScript |
| **База данных** | MySQL 8.0 |
| **Редактор** | Tiptap |
| **Стили** | TailwindCSS |
| **Состояние** | Zustand |
| **Запросы** | TanStack Query |

---

## 🚀 Быстрый старт

### Требования

- **.NET SDK** 8.0+
- **Node.js** 18+ (рекомендуется 20+)
- **MySQL** 8.0+
- **npm** 9+

---

### 1. Настройка базы данных

```sql
-- Создание БД и пользователя
CREATE DATABASE sibgamer CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'sibgamer'@'localhost' IDENTIFIED BY 'your_password';
GRANT ALL PRIVILEGES ON sibgamer.* TO 'sibgamer'@'localhost';
FLUSH PRIVILEGES;
```

```bash
# Импорт схемы
cd backend/db
mysql -u sibgamer -p sibgamer < schema.sql
```

---

### 2. Настройка Backend

Отредактируйте `backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sibgamer;User=sibgamer;Password=your_password;Port=3306;"
  },
  "FrontendUrl": "http://localhost:5173",
  "ImageBaseUrl": "http://localhost:5000"
}
```

Запуск:
```bash
cd backend
dotnet restore
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
2. Выполните SQL:
```sql
UPDATE Users SET IsAdmin = 1 WHERE Email = 'your-email@example.com';
```

---

## 📁 Структура проекта

```
SibGamer/
├── backend/                 # ASP.NET Core API
│   ├── Controllers/         # API контроллеры
│   ├── Services/            # Бизнес-логика
│   ├── Models/              # Модели данных
│   ├── BackgroundServices/  # Фоновые сервисы
│   └── db/                  # SQL схема
│
└── frontend/                # React SPA
    └── src/
        ├── components/      # UI компоненты
        ├── pages/           # Страницы
        ├── hooks/           # Кастомные хуки
        ├── store/           # Zustand store
        └── lib/             # Утилиты
```

---

## 🔧 Troubleshooting

| Проблема | Решение |
|----------|---------|
| CORS ошибка | Проверьте `FrontendUrl` в `appsettings.json` |
| Нет подключения к БД | Проверьте MySQL и настройки подключения |
| Изображения не грузятся | Проверьте `ImageBaseUrl` и `VITE_IMAGE_BASE_URL` |

---

## 📋 Документация

- [PROJECT_REVIEW.md](./PROJECT_REVIEW.md) — детальный обзор проекта и план доработки

# SibGamer

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![React](https://img.shields.io/badge/React-19-blue)
![MySQL](https://img.shields.io/badge/MySQL-8.0-orange)
![License](https://img.shields.io/badge/license-MIT-green)

Портал игрового сообщества для Counter-Strike серверов...