# 🔗 URL Shortener

URL shortening service built with **ASP.NET Core** and **Entity Framework**, featuring JWT authentication and role-based authorization.

## 🚀 Quick Start

### Prerequisites

- .NET 9.0 SDK
- Docker & Docker Compose

### Setup

1. **Clone and navigate to project**

   ```bash
   git clone https://github.com/Skriler/UrlShortener
   cd UrlShortener
   ```

2. **Configure environment**  
   The `.env` file is already configured. Modify if needed:

   ```env
   SqlServer__Host=localhost
   SqlServer__Port=1433
   SqlServer__Password=Strong@Passw0rd

   SeedingConfig__SystemAdminUser__Username=admin
   SeedingConfig__SystemAdminUser__Email=admin@company.com
   SeedingConfig__SystemAdminUser__Password=Admin123!

   JwtConfig__Secret=Str0ng@Secret
   ```

3. **Start database & Apply migrations**

   ```bash
   docker-compose up -d
   dotnet ef database update
   ```

4. **Access application**
   - API: `https://localhost:44318`
   - Swagger: `https://localhost:44318/swagger`

## 🛠 Tech Stack

| Component    | Technology                  |
| ------------ | --------------------------- |
| **Backend**  | ASP.NET Core 9.0            |
| **Database** | SQL Server (Docker)         |
| **Auth**     | JWT Bearer Tokens           |
| **ORM**      | Entity Framework Code First |
| **Testing**  | xUnit + GitHub Actions      |

## 📊 Implementation Status

### ✅ Fully Implemented

- **REST API** with complete CRUD operations
- **JWT Authentication** with role-based authorization
- **URL Shortening** with unique 6-char alphanumeric codes
- **URL Redirection** via short codes
- **Database** with Code First migrations and auto-seeding
- **Error Handling** with custom middleware and filters
- **Unit Tests** with github actions support

### ⚠️ Partially Implemented

- **React Frontend** (structure only, no integration)
- **About Page** (Razor page works, but JWT integration pending)

## 🔐 User Roles & Permissions

| Role          | Permissions                                           |
| ------------- | ----------------------------------------------------- |
| **Anonymous** | View URL list                                         |
| **User**      | View URLs, Create URLs, Delete own URLs, View details |
| **Admin**     | All user permissions + Delete any URL                 |

## 🛡 API Endpoints

### Authentication

| Method | Endpoint           | Description       | Auth |
| ------ | ------------------ | ----------------- | ---- |
| `POST` | `/api/auth/login`  | User login        | ❌   |
| `GET`  | `/api/auth/me`     | Current user info | ✅   |
| `POST` | `/api/auth/logout` | User logout       | ✅   |

### URL Management

| Method   | Endpoint              | Description      | Auth |
| -------- | --------------------- | ---------------- | ---- |
| `GET`    | `/api/shorturls`      | Get all URLs     | ❌   |
| `GET`    | `/api/shorturls/{id}` | Get URL details  | ✅   |
| `POST`   | `/api/shorturls`      | Create short URL | ✅   |
| `DELETE` | `/api/shorturls/{id}` | Delete URL       | ✅   |

### Redirection

| Method | Endpoint       | Description              | Auth |
| ------ | -------------- | ------------------------ | ---- |
| `GET`  | `/{shortCode}` | Redirect to original URL | ❌   |

## ⚙️ URL Shortening Algorithm

**Random Generation Approach**: Creates 6-character codes using alphanumeric characters (A-Z, a-z, 0-9) with database uniqueness validation and collision handling (max 100 attempts).

```
Character Set: 62 characters → ~56 billion combinations
Default Length: 6 characters
Collision Strategy: Regenerate until unique
```

## 🧪 Testing

```bash
# Run unit tests
dotnet test UrlShortener.Unit/
```

## 📝 Default Seeded Data

- **Admin User**: `admin / Admin123!`
- **Sample Users**: From `seed-users.json`
- **Sample URLs**: From `seed-urls.json`
- **Roles**: Admin, User
