# Task Management API

## 🛠️ Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core + SQL Server
- ASP.NET Core Identity
- JWT Authentication
- Redis Caching
- Swagger / OpenAPI
- AutoMapper

---

## ⚙️ Setup Instructions

### 1. Prerequisites
- .NET 8 SDK
- SQL Server
- Redis (localhost:6379)

### 2. Clone the project
git clone https://github.com/yourrepo/task-management-api.git
cd task-management-api

### 3. Update appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Task;Trusted_Connection=True;encrypt=false;",
    "IdentityConnection": "Server=.;Database=Task_ApiIdentity;Trusted_Connection=True;encrypt=false;",
    "Redis": "localhost:6379"
  },
  "JWT": {
    "Authkey": "SecurityKeyuserjoooAymanoker24111112",
    "ValidAudience": "MySecureApiUsers",
    "ValidIssuer": "https://localhost:7131/",
    "DurationInDays": 1
  }
}

### 4. Run the project
dotnet run --project API

---

## 🗄️ Database
- Migrations run automatically on startup
- Seeding runs automatically on first launch

---

## 👤 Seeded Admin Credentials
| Role  | Email               | Password  |
|-------|---------------------|-----------|
| Admin | admin@example.com   | Admin@123 |
| User  | Yousef@example.com  | Yousef@123|
| User  | Salma@example.com   | Salma@123 |

---

## 📌 API Endpoints

### Auth
| Method | Endpoint                  | Auth     | Description          |
|--------|---------------------------|----------|----------------------|
| POST   | /api/account/register     | ❌       | Register new user    |
| POST   | /api/account/login        | ❌       | Login + get token    |
| POST   | /api/account/refresh-token| ❌       | Refresh JWT token    |
| POST   | /api/account/logout       | ✅ JWT   | Logout               |
| GET    | /api/account/me           | ✅ JWT   | Get current user     |

### Admin (Admin Role Only)
| Method | Endpoint                      | Description       |
|--------|-------------------------------|-------------------|
| GET    | /api/admin/GetUsers_Active    | Get active users  |
| GET    | /api/admin/GetUsers_NotActive | Get deleted users |
| POST   | /api/admin/CreateUser         | Create new user   |
| DELETE | /api/admin/DeleteUser/{id}    | Soft delete user  |

### Tasks (Authenticated Users)
| Method | Endpoint                  | Description              |
|--------|---------------------------|--------------------------|
| GET    | /api/task                 | Get my tasks             |
| GET    | /api/task/{id}            | Get task by ID (cached)  |
| POST   | /api/task                 | Create new task          |
| PATCH  | /api/task/{id}/status     | Update task status       |

---

## 🔴 Redis Caching
- GET /api/task/{id} → cached for 10 minutes
- Cache invalidated when task status is updated

---

## ⚙️ Background Processing
- When a task is created → saved in DB with Status = Pending
- Task ID sent to in-memory queue (ConcurrentQueue)
- BackgroundService picks it up every 2 seconds
- After 3 seconds processing → Status updated to InProgress

---

## 🧠 Business Logic
1. Tasks sorted by Priority (High → Low) then by CreatedAt
2. Duplicate task prevention: same title + same user + same day → rejected

---

## 📁 Project Structure
TaskManagement/
├── Core/                    → Entities, Enums, Interfaces
│   ├── Entities/
│   ├── Enums/
│   └── Services.Contract/
├── Service/                 → Business Logic
│   ├── AuthService.cs
│   ├── AdminService.cs
│   ├── TaskService.cs
│   └── ResponseCacheService.cs
├── Repository/              → Data Access
│   ├── Data/
│   │   ├── StoreContext.cs
│   │   └── DataSeeding/
│   └── Identity/
│       ├── AppIdentityDbContext.cs
│       └── DataSeeding/
└── API/                     → Controllers, DTOs, Middleware
    ├── Controllers/
    ├── DTOs/
    ├── Helpers/
    └── Middleware/

---

## Assumptions
- Each user can only see and update their own tasks
- Admin cannot create tasks, only manage users
- Soft delete used for users (IsDeleted flag)
- RefreshToken stored in DB, expires after 7 days
- AccessToken expires after 1 day
