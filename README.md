<p align="center">
  <img src="https://img.icons8.com/3d-fluency/94/shopping-cart.png" alt="E-Commerce Logo" width="94" height="94" />
</p>

<h1 align="center">🛒 E-Commerce API</h1>

<p align="center">
  <strong>A production-ready, enterprise-grade RESTful API for modern e-commerce platforms</strong><br/>
  <em>Built with Clean Architecture · CQRS · Domain-Driven Design</em>
</p>

<p align="center">
  <a href="https://github.com/ahmed-hamada-hassan/E-Commerce"><img src="https://img.shields.io/github/repo-size/ahmed-hamada-hassan/E-Commerce?style=for-the-badge&color=0d1117&labelColor=1a1b27&logo=github" alt="Repo Size" /></a>
  <a href="https://github.com/ahmed-hamada-hassan/E-Commerce/stargazers"><img src="https://img.shields.io/github/stars/ahmed-hamada-hassan/E-Commerce?style=for-the-badge&color=f5a623&labelColor=1a1b27&logo=star" alt="Stars" /></a>
  <a href="https://github.com/ahmed-hamada-hassan/E-Commerce/network/members"><img src="https://img.shields.io/github/forks/ahmed-hamada-hassan/E-Commerce?style=for-the-badge&color=00b4d8&labelColor=1a1b27&logo=git" alt="Forks" /></a>
  <a href="https://github.com/ahmed-hamada-hassan/E-Commerce/issues"><img src="https://img.shields.io/github/issues/ahmed-hamada-hassan/E-Commerce?style=for-the-badge&color=e63946&labelColor=1a1b27&logo=target" alt="Issues" /></a>
  <a href="https://github.com/ahmed-hamada-hassan/E-Commerce/commits/main"><img src="https://img.shields.io/github/last-commit/ahmed-hamada-hassan/E-Commerce?style=for-the-badge&color=2ecc71&labelColor=1a1b27&logo=git" alt="Last Commit" /></a>
</p>

<br/>

---

<br/>

## 🏗️ Tech Stack

<table align="center">
  <tr>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/ASP.NET_Core-0078D4?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/EF_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
    </td>
  </tr>
  <tr>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white" alt="Redis" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" alt="JWT" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/Cloudinary-3448C5?style=for-the-badge&logo=cloudinary&logoColor=white" alt="Cloudinary" />
    </td>
  </tr>
  <tr>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/MediatR-FF6B6B?style=for-the-badge&logoColor=white" alt="MediatR" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/Serilog-2C3E50?style=for-the-badge&logoColor=white" alt="Serilog" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/FluentValidation-1E88E5?style=for-the-badge&logoColor=white" alt="FluentValidation" />
    </td>
    <td align="center" width="140">
      <img src="https://img.shields.io/badge/Scrutor-8E44AD?style=for-the-badge&logoColor=white" alt="Scrutor" />
    </td>
  </tr>
</table>

<br/>

---

<br/>

## 📖 About

**E-Commerce API** is a full-featured backend system that powers modern e-commerce operations — from user registration and product management to order processing, payments, returns, and feedback. The API is designed following **Clean Architecture** principles with **CQRS** (Command Query Responsibility Segregation) via MediatR, ensuring separation of concerns, testability, and maintainability at scale.

### 🎯 Problem vs. Solution

| Problem | Solution |
|:--------|:---------|
| Monolithic codebases become unmanageable | **Clean Architecture** with strict layer separation |
| Business logic leaks into controllers | **CQRS + MediatR** encapsulates every use case |
| Validation scattered across layers | **FluentValidation pipeline behavior** enforces rules before handlers execute |
| Hard-coded DI registrations grow endlessly | **Scrutor** auto-scans and registers services by convention |
| No audit trail for deleted data | **Soft-delete interceptor** preserves records with `IsDeleted` flags |
| Unstructured logging makes debugging hard | **Serilog** with structured, async file + console sinks |
| API abuse and brute-force attacks | **Rate limiting** (IP-based + user-based sliding windows) |

<br/>

---

<br/>

## ✨ Key Features

<table>
  <tr>
    <td>

### 🔐 Authentication & Authorization
- JWT-based authentication with refresh tokens
- Role-based access control (SuperAdmin, Admin, Vendor, Customer, Representative)
- Account lockout protection
- Configurable password policies

### 📦 Product Management
- Full CRUD with soft-delete and restore
- Multi-image upload via Cloudinary
- Vendor-specific product catalogs
- Category management with hierarchy
- Background cleanup jobs for expired products

### 🛒 Order Processing
- Cart management with real-time stock validation
- Multi-status order lifecycle tracking
- Order cancellation workflows
- Representative order assignment

</td>
    <td>

### 💳 Payments & Refunds
- Payment factory pattern (extensible methods)
- Payment status tracking
- Automated refund processing
- Return request management with admin approval

### ⭐ Feedback & Reviews
- Product review system with admin moderation
- Feedback approval pipeline
- Customer feedback CRUD operations

### ⚙️ Infrastructure
- Redis distributed caching
- IP & user-based rate limiting
- Global exception handling middleware
- Structured logging with Serilog
- Cursor & offset pagination support
- Database seeding with initial data

</td>
  </tr>
</table>

<br/>

---

<br/>

## 🏛️ Clean Architecture

The solution strictly follows the **Clean Architecture** pattern with four independent layers. Dependencies flow **inward only** — outer layers depend on inner layers, never the reverse.

```
                    ┌──────────────────────────────────┐
                    │          E-Commerce.API          │  ← Presentation Layer
                    │  Controllers · Middlewares ·     │
                    │  Contracts · Program.cs          │
                    └───────────────┬──────────────────┘
                                    │ depends on
                    ┌───────────────▼──────────────────┐
                    │      E-Commerce.Application      │  ← Application Layer
                    │  Features (CQRS) · Behaviors ·   │
                    │  Interfaces · DTOs · Validators  │
                    └───────────────┬──────────────────┘
                                    │ depends on
                    ┌───────────────▼──────────────────┐
                    │    E-Commerce.Infrastructure     │  ← Infrastructure Layer
                    │  Data (EF Core) · Repositories · │
                    │  Services · Migrations · Jobs    │
                    └───────────────┬──────────────────┘
                                    │ depends on
                    ┌───────────────▼──────────────────┐
                    │        E-Commerce.Domain         │  ← Domain Layer (Core)
                    │  Entities · Enums · Errors ·     │
                    │  Shared (Result Pattern)         │
                    └──────────────────────────────────┘
```

<br/>

### 📂 Project Structure

```
E-Commerce/
│
├── 📁 E-Commerce.API/                    # Presentation Layer
│   ├── 📁 Contracts/                     # Mapping extensions (Entity ↔ DTO)
│   ├── 📁 Controllers/                   # API endpoints
│   │   ├── AuthController.cs             # Registration & login
│   │   ├── AdminCategoriesController.cs  # Category management (admin)
│   │   ├── AdminProductsController.cs    # Product management (admin)
│   │   ├── AdminOrdersController.cs      # Order management (admin)
│   │   ├── AdminCustomersController.cs   # Customer management (admin)
│   │   ├── AdminVendorsController.cs     # Vendor management (admin)
│   │   ├── AdminFeedbackController.cs    # Feedback moderation (admin)
│   │   ├── CartController.cs             # Shopping cart
│   │   ├── OrdersController.cs           # Customer orders
│   │   ├── ProductFeedbackController.cs  # Product reviews
│   │   ├── VendorProductsController.cs   # Vendor product CRUD
│   │   └── RepresentativeOrderController # Representative orders
│   ├── 📁 Middlewares/                   # Global exception handler
│   └── Program.cs                        # App configuration & DI
│
├── 📁 E-Commerce.Application/           # Application Layer (Use Cases)
│   ├── 📁 Behaviors/                    # MediatR pipeline behaviors
│   │   ├── LoggingBehavior.cs           # Request/response logging
│   │   └── ValidateBehavior.cs          # FluentValidation pipeline
│   ├── 📁 Features/                     # CQRS feature slices
│   │   ├── 📁 Auth/                     # Login, Register commands
│   │   ├── 📁 Products/                 # Product queries & commands
│   │   ├── 📁 Categories/              # Category queries & commands
│   │   ├── 📁 Orders/                   # Order queries & commands
│   │   ├── 📁 Carts/                    # Cart queries & commands
│   │   ├── 📁 Feedbacks/               # Feedback queries & commands
│   │   ├── 📁 Vendors/                  # Vendor queries & commands
│   │   └── 📁 Users/                    # User profile management
│   └── 📁 Interfaces/                   # Abstractions & contracts
│       ├── 📁 Repositories/             # Repository interfaces
│       ├── 📁 Services/                 # Service interfaces
│       └── 📁 Data/                     # DbContext interface
│
├── 📁 E-Commerce.Infrastructure/        # Infrastructure Layer
│   ├── 📁 Data/
│   │   ├── AppDbContext.cs              # EF Core DbContext
│   │   ├── DbInitializer.cs             # Seed data
│   │   ├── UnitOfWork.cs                # Unit of Work pattern
│   │   ├── 📁 Configs/                  # Entity type configurations
│   │   ├── 📁 Interceptors/             # Soft-delete interceptor
│   │   └── 📁 Repositories/             # Repository implementations
│   ├── 📁 Services/                     # External service integrations
│   │   ├── CloudinaryService.cs         # Image upload service
│   │   ├── TokenService.cs              # JWT token generation
│   │   ├── UserContext.cs               # Current user resolver
│   │   └── 📁 Payments/                 # Payment processing
│   ├── 📁 BackgroundJobs/               # Hosted services
│   └── 📁 Migrations/                   # EF Core migrations
│
├── 📁 E-Commerce.Domain/               # Domain Layer (Core)
│   ├── 📁 Entities/                     # Domain entities
│   │   ├── ApplicationUser.cs           # User aggregate root
│   │   ├── Product.cs                   # Product entity
│   │   ├── Order.cs                     # Order aggregate root
│   │   ├── Category.cs                  # Category entity
│   │   ├── Cart.cs / CartItem.cs        # Shopping cart
│   │   ├── Payment.cs                   # Payment entity
│   │   ├── Feedback.cs                  # Product review
│   │   ├── ReturnRequest.cs             # Return request
│   │   ├── Refund.cs                    # Refund entity
│   │   └── Vendor.cs                    # Vendor profile
│   ├── 📁 Enums/                        # Domain enumerations
│   ├── 📁 Errors/                       # Typed domain errors
│   ├── 📁 Common/                       # Soft-delete base classes
│   └── 📁 Shared/                       # Result pattern & roles
│
└── E-Commerce.slnx                      # Solution file
```

<br/>

---

<br/>

## 🔌 API Endpoints

### 🔐 Authentication
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `POST` | `/api/auth/register` | Register a new user | ❌ |
| `POST` | `/api/auth/login` | Login & get JWT token | ❌ |

### 📦 Products
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/products` | Browse products (customer) | ❌ |
| `GET` | `/api/vendor/products` | List vendor's products | 🔒 Vendor |
| `POST` | `/api/vendor/products` | Create a product | 🔒 Vendor |
| `PUT` | `/api/vendor/products/{id}` | Update a product | 🔒 Vendor |
| `DELETE` | `/api/vendor/products/{id}` | Delete a product | 🔒 Vendor |
| `POST` | `/api/vendor/products/{id}/images` | Upload product images | 🔒 Vendor |

### 🗂️ Categories (Admin)
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/categories` | List all categories | 🔒 Admin |
| `POST` | `/api/admin/categories` | Create a category | 🔒 Admin |
| `PUT` | `/api/admin/categories/{id}` | Update a category | 🔒 Admin |
| `DELETE` | `/api/admin/categories/{id}` | Delete a category | 🔒 Admin |
| `PATCH` | `/api/admin/categories/{id}/restore` | Restore deleted category | 🔒 Admin |

### 🛒 Cart & Orders
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/cart` | View cart | 🔒 Customer |
| `POST` | `/api/cart/items` | Add item to cart | 🔒 Customer |
| `DELETE` | `/api/cart/items/{id}` | Remove cart item | 🔒 Customer |
| `POST` | `/api/orders` | Place an order | 🔒 Customer |
| `GET` | `/api/orders` | List customer orders | 🔒 Customer |
| `POST` | `/api/orders/{id}/cancel` | Cancel an order | 🔒 Customer |
| `POST` | `/api/orders/{id}/return` | Request a return | 🔒 Customer |

### ⭐ Feedback
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/products/{id}/feedbacks` | View product reviews | ❌ |
| `POST` | `/api/products/{id}/feedbacks` | Submit a review | 🔒 Customer |
| `PUT` | `/api/feedbacks/{id}` | Edit own review | 🔒 Customer |
| `DELETE` | `/api/feedbacks/{id}` | Delete own review | 🔒 Customer |
| `GET` | `/api/admin/feedbacks/pending` | View pending reviews | 🔒 Admin |
| `PATCH` | `/api/admin/feedbacks/{id}/approve` | Approve a review | 🔒 Admin |

<br/>

---

<br/>

## 🚀 Getting Started

### Prerequisites

| Tool | Version |
|:-----|:--------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ |
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | 2019+ |
| [Redis](https://redis.io/) | 7.0+ |

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/ahmed-hamada-hassan/E-Commerce.git
cd E-Commerce
```

### 2️⃣ Configure Environment

Create an `appsettings.Development.json` file in the `E-Commerce.API` directory:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "JWT": {
    "SecretKey": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "E-Commerce.API",
    "Audience": "E-Commerce.Client",
    "ExpirationInMinutes": 60
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "RedisSettings": {
    "ConnectionString": "localhost:6379"
  }
}
```

### 3️⃣ Apply Migrations & Seed Data

```bash
dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.API
```

### 4️⃣ Run the Application

```bash
dotnet run --project E-Commerce.API
```

The API will be available at `https://localhost:5001` and the interactive API docs at:

```
https://localhost:5001/scalar/v1
```

### 🧪 Test Accounts

The database is seeded with default accounts for testing. You can use these credentials to explore different role-based functionalities:

| Role | Email | Password |
|:-----|:------|:---------|
| **Super Admin** | `admin@ecommerce.com` | `Admin@123` |
| **Vendor** | `vendor@ecommerce.com` | `Vendor@123` |
| **Customer** | `customer@ecommerce.com` | `Customer@123` |

<br/>

---

<br/>

## 🧩 Design Patterns & Principles

| Pattern | Usage |
|:--------|:------|
| **Clean Architecture** | Strict layer separation with inward-only dependencies |
| **CQRS** | Commands and Queries separated via MediatR handlers |
| **Repository Pattern** | Abstractions over data access with EF Core implementations |
| **Unit of Work** | Atomic database operations across multiple repositories |
| **Result Pattern** | Explicit success/failure handling instead of exceptions |
| **Factory Pattern** | Payment method creation via `PaymentFactory` |
| **Pipeline Behaviors** | Cross-cutting concerns (logging, validation) via MediatR |
| **Soft Delete** | EF Core interceptor auto-sets `IsDeleted` flags |
| **Options Pattern** | Strongly-typed, validated configuration sections |
| **Convention-Based DI** | Scrutor auto-registers services by interface markers |

<br/>

---

<br/>

## 🛡️ Security

- 🔑 **JWT Bearer Authentication** with configurable expiration
- 🔒 **Role-Based Authorization** policies per endpoint
- 🧱 **Rate Limiting** — sliding window per IP and per user
- 🔐 **Account Lockout** — auto-lock after 5 failed attempts
- 🛡️ **CORS** — configurable allowed origins
- 🔏 **Data Protection** — ASP.NET Core Data Protection API
- 📝 **User Secrets** — sensitive config kept out of source control

<br/>

---

<br/>

## 📬 Contact

<p align="center">
  <a href="https://github.com/ahmed-hamada-hassan">
    <img src="https://img.shields.io/badge/GitHub-ahmed--hamada--hassan-181717?style=for-the-badge&logo=github" alt="GitHub" />
  </a>
</p>

<br/>

---

<p align="center">
  <sub>⭐ If you find this project useful, consider giving it a star!</sub>
</p>
