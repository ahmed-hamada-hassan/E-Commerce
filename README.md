# 🛒 E-Commerce API — Backend

> **Branch:** `backend`
> For the full project overview, architecture, and API reference, see the [`main` branch README](https://github.com/ahmed-hamada-hassan/E-Commerce/blob/main/README.md).

---

## Tech Stack

- **.NET 10** / **ASP.NET Core** / **C#**
- **Entity Framework Core** — SQL Server
- **Redis** — Distributed cache & rate limiting
- **MediatR** — CQRS with pipeline behaviors
- **FluentValidation** · **Serilog** · **Scrutor** · **Cloudinary**

---

## Getting Started

### Prerequisites

| Tool | Version |
|:-----|:--------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ |
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | 2019+ |
| [Redis](https://redis.io/) | 7.0+ |

### 1. Clone & Checkout

```bash
git clone https://github.com/ahmed-hamada-hassan/E-Commerce.git
cd E-Commerce
git checkout backend
```

### 2. Configure Environment

Create `appsettings.Development.json` in `E-Commerce.API/`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "JWT": {
    "SecretKey": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "E-Commerce.API",
    "Audience": "E-Commerce.Client",
    "AccessTokenExpirationInMinutes": 60
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "RedisSettings": {
    "ConnectionString": "localhost:6379"
  },
  "AllowOrigins": ["http://localhost:3000"]
}
```

### 3. Run

```bash
dotnet run --project E-Commerce.API
```

> Migrations are applied and the database is seeded automatically on startup via `DbInitializer.SeedAsync`.

API docs available at: `https://localhost:{port}/scalar/v1`

### Test Accounts

| Role | Email | Password |
|:-----|:------|:---------|
| Super Admin | `admin@ecommerce.com` | `Admin@123` |
| Vendor | `vendor@ecommerce.com` | `Vendor@123` |
| Customer | `customer@ecommerce.com` | `Customer@123` |
| Representative | `rep@ecommerce.com` | `Rep@123` |

---

## Project Structure

```
E-Commerce/
├── E-Commerce.API/              # Presentation Layer (Controllers, Middlewares)
├── E-Commerce.Application/      # Application Layer (CQRS Features, Interfaces, DTOs)
├── E-Commerce.Infrastructure/   # Infrastructure (EF Core, Repositories, Services)
├── E-Commerce.Domain/           # Domain Layer (Entities, Enums, Errors, Shared)
└── E-Commerce.slnx
```
