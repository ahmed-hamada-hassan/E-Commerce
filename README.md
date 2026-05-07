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



## 🌐 Live Environment

<p align="left">
  <img src="https://img.shields.io/badge/Hosted_on-Monster_ASP.NET-blue?style=for-the-badge&logo=microsoft" alt="Hosted on Monster ASP.NET" />
</p>

- **Live Base URL:** `https://site67484.siteasp.net` *(Replace `http://localhost:{port}` with this URL in frontend config)*
- **API Documentation (Scalar/Swagger):** [https://site67484.siteasp.net/scalar/v1](https://site67484.siteasp.net/scalar/v1)

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

### 🏢 Infrastructure

| Component | Role in Project |
|:----------|:----------------|
| **SQL Server** | Primary relational database storing all domain entities (Products, Users, Orders, etc.). |
| **Redis** | Distributed cache for performance optimization and sliding-window rate limiting. |
| **Cloudinary** | Cloud storage provider for managing and serving product images and user avatars. |

<br/>

---

<br/>

## 📖 About

**E-Commerce API** is a full-featured backend system powering modern e-commerce operations — from user registration and product management to order processing, payments, returns, and feedback. Designed following **Clean Architecture** principles with **CQRS** via MediatR, ensuring separation of concerns, testability, and maintainability at scale.

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
- Multi-image upload, reorder, and set-primary via Cloudinary
- Vendor-specific product catalogs
- Category management with soft-delete & restore
- Background cleanup jobs for orphaned products

### 🛒 Order Processing
- Cart management with real-time stock validation
- Multi-status order lifecycle (Processing → Shipped → Delivered)
- Order cancellation workflows
- Representative return request handling

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
- **Automated DB seeding** — roles, users, vendor profiles, and addresses on first run

</td>
  </tr>
</table>

<br/>

---

<br/>

## 🏛️ Clean Architecture

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
│   ├── 📁 Controllers/
│   │   ├── BaseApiController.cs          # CurrentUserId, CurrentVendorId helpers
│   │   ├── AuthController.cs             # Register (customer/vendor/admin) & login
│   │   ├── AdminCategoriesController.cs  # Category CRUD + restore (SuperAdmin)
│   │   ├── AdminProductsController.cs    # Product suspend/unsuspend + images (SuperAdmin)
│   │   ├── AdminOrdersController.cs      # Orders overview, shipping, return approval
│   │   ├── AdminUsersController.cs       # User block/unblock/delete/restore
│   │   ├── AdminVendorsController.cs     # Vendor activate/deactivate (SuperAdmin)
│   │   ├── AdminFeedbackController.cs    # Feedback moderation
│   │   ├── CartController.cs             # Shopping cart (Customer)
│   │   ├── OrdersController.cs           # Place, cancel, return orders (Customer)
│   │   ├── CustomerProductsController.cs # Browse products (Public)
│   │   ├── CustomerProfileController.cs  # User profile & avatar
│   │   ├── AddressController.cs          # Address CRUD + set-default (Customer)
│   │   ├── ProductFeedbackController.cs  # Product reviews (Customer)
│   │   ├── VendorProductsController.cs   # Full product + image CRUD (Vendor)
│   │   ├── VendorStoreProfileController.cs # Store profile (Vendor)
│   │   └── RepresentativeOrderController.cs # Return request processing (Representative)
│   ├── 📁 Middlewares/                   # Global exception handler
│   └── Program.cs                        # App configuration & DI
│
├── 📁 E-Commerce.Application/           # Application Layer (Use Cases)
│   ├── 📁 Behaviors/                    # MediatR pipeline behaviors
│   │   ├── LoggingBehavior.cs
│   │   └── ValidateBehavior.cs
│   ├── 📁 Features/                     # CQRS feature slices
│   │   ├── 📁 Auth/
│   │   ├── 📁 Products/ & ProductImages/
│   │   ├── 📁 Categories/
│   │   ├── 📁 Orders/
│   │   ├── 📁 Carts/
│   │   ├── 📁 Feedbacks/
│   │   ├── 📁 Vendors/
│   │   ├── 📁 Addresses/
│   │   └── 📁 Users/
│   └── 📁 Interfaces/
│       ├── 📁 Repositories/
│       ├── 📁 Services/                 # IUserContext, ITokenService, etc.
│       └── 📁 Data/
│
├── 📁 E-Commerce.Infrastructure/
│   ├── 📁 Data/
│   │   ├── AppDbContext.cs
│   │   ├── DbInitializer.cs             # Seeds roles, users, vendor profiles & addresses
│   │   ├── UnitOfWork.cs
│   │   ├── 📁 Configs/
│   │   ├── 📁 Interceptors/             # SoftDeleteInterceptor
│   │   └── 📁 Repositories/
│   ├── 📁 Services/
│   │   ├── CloudinaryService.cs
│   │   ├── TokenService.cs              # JWT + vendor_id claim
│   │   ├── UserContext.cs               # Resolves UserId, VendorId, roles from JWT
│   │   └── 📁 Payments/
│   ├── 📁 BackgroundJobs/
│   └── 📁 Migrations/
│
├── 📁 E-Commerce.Domain/
│   ├── 📁 Entities/
│   │   ├── ApplicationUser.cs
│   │   ├── Product.cs, ProductImage.cs
│   │   ├── Order.cs, OrderItem.cs, Cancellation.cs
│   │   ├── Cart.cs, CartItem.cs
│   │   ├── Payment.cs, Refund.cs, ReturnRequest.cs
│   │   ├── Feedback.cs
│   │   ├── Address.cs
│   │   ├── Category.cs
│   │   └── Vendor.cs
│   ├── 📁 Enums/                        # OrderStatus, PaymentMethod, AddressType, etc.
│   ├── 📁 Errors/
│   ├── 📁 Common/                       # SoftDeletable base
│   └── 📁 Shared/                       # Result<T>, AppRoles
│
└── E-Commerce.slnx
```

<br/>

---

<br/>

## 🔌 API Endpoints

### 🔐 Authentication
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `POST` | `/api/Auth/register-customer` | Register a new customer | ❌ |
| `POST` | `/api/Auth/register-vendor` | Register a new vendor | ❌ |
| `POST` | `/api/Auth/register` | Register admin/representative | 🔒 SuperAdmin |
| `POST` | `/api/Auth/login` | Login & receive JWT + refresh token | ❌ |
| `POST` | `/api/Auth/refresh-token` | Rotate access token | ❌ |

### 🛍️ Customer — Products
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/customer/products` | Browse products (offset pagination) | ❌ |
| `GET` | `/api/customer/products/{id}` | Get product details | ❌ |

### 📦 Vendor — Products & Images
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/vendor/products` | List own products | 🔒 Vendor |
| `GET` | `/api/vendor/products/archived` | List archived products | 🔒 Vendor |
| `GET` | `/api/vendor/products/{id}` | Get product detail | 🔒 Vendor |
| `POST` | `/api/vendor/products` | Create product | 🔒 Vendor |
| `PUT` | `/api/vendor/products/{id}` | Update product | 🔒 Vendor |
| `DELETE` | `/api/vendor/products/{id}` | Archive product | 🔒 Vendor |
| `PATCH` | `/api/vendor/products/{id}/restore` | Restore archived product | 🔒 Vendor |
| `POST` | `/api/vendor/products/{id}/images` | Upload images | 🔒 Vendor |
| `GET` | `/api/vendor/products/{id}/images` | List product images | 🔒 Vendor |
| `GET` | `/api/vendor/products/{id}/images/{imgId}` | Get image detail | 🔒 Vendor |
| `PUT` | `/api/vendor/products/{id}/images/{imgId}` | Replace image | 🔒 Vendor |
| `PUT` | `/api/vendor/products/{id}/images/reorder` | Reorder images | 🔒 Vendor |
| `PUT` | `/api/vendor/products/{id}/images/{imgId}/set-primary` | Set primary image | 🔒 Vendor |
| `DELETE` | `/api/vendor/products/{id}/images/{imgId}` | Delete image | 🔒 Vendor |
| `DELETE` | `/api/vendor/products/{id}/images` | Clear all images | 🔒 Vendor |

### 👤 User — Profile (Customer & Vendor)
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/user/profile` | Get own profile | 🔒 Authenticated |
| `PUT` | `/api/user/profile` | Update personal info | 🔒 Authenticated |
| `PUT` | `/api/user/profile/image` | Update profile avatar | 🔒 Authenticated |

### 🏪 Vendor — Store Profile
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `PUT` | `/api/vendor/store/profile` | Update store info | 🔒 Vendor |

### 📍 Customer — Address
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/addresses` | List addresses | 🔒 Customer |
| `GET` | `/api/addresses/{id}` | Get address detail | 🔒 Customer |
| `POST` | `/api/addresses` | Add addresses | 🔒 Customer |
| `PUT` | `/api/addresses/{id}` | Update address | 🔒 Customer |
| `PATCH` | `/api/addresses/{id}/set-default` | Set default shipping address | 🔒 Customer |
| `DELETE` | `/api/addresses/{id}` | Delete address | 🔒 Customer |

### 🛒 Cart & Orders
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/cart` | View cart | 🔒 Customer |
| `POST` | `/api/cart/items/{productId}` | Add item | 🔒 Customer |
| `PUT` | `/api/cart/items/{productId}` | Update item quantity | 🔒 Customer |
| `DELETE` | `/api/cart/items/{productId}` | Remove item | 🔒 Customer |
| `DELETE` | `/api/cart` | Clear cart | 🔒 Customer |
| `POST` | `/api/orders` | Place an order | 🔒 Customer |
| `GET` | `/api/orders` | List my orders | 🔒 Customer |
| `GET` | `/api/orders/{id}` | Order details | 🔒 Customer |
| `POST` | `/api/orders/{id}/cancel` | Cancel order | 🔒 Customer |
| `POST` | `/api/orders/{id}/return-request` | Request a return | 🔒 Customer |

### ⭐ Feedback
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/products/{id}/feedbacks` | Browse product reviews | ❌ |
| `POST` | `/api/products/{id}/feedbacks` | Submit a review | 🔒 Customer |
| `PUT` | `/api/feedbacks/{id}` | Edit own review | 🔒 Customer |
| `DELETE` | `/api/feedbacks/{id}` | Delete own review | 🔒 Customer |

### 🚚 Representative
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/representative/returns/approved` | List approved return requests | 🔒 Representative / SuperAdmin |
| `POST` | `/api/representative/status/{returnReqId}` | Complete or reject a return | 🔒 Representative / SuperAdmin |

### 🛠️ Admin — Categories
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/categories` | List active categories | 🔒 SuperAdmin |
| `GET` | `/api/admin/categories/deleted` | List deleted categories | 🔒 SuperAdmin |
| `GET` | `/api/admin/categories/{id}` | Category detail | 🔒 SuperAdmin |
| `GET` | `/api/admin/categories/{id}/deleted` | Deleted category detail | 🔒 SuperAdmin |
| `POST` | `/api/admin/categories` | Create category | 🔒 SuperAdmin |
| `PUT` | `/api/admin/categories/{id}` | Update category | 🔒 SuperAdmin |
| `PATCH` | `/api/admin/categories/{id}/restore` | Restore category | 🔒 SuperAdmin |
| `DELETE` | `/api/admin/categories/{id}` | Delete category | 🔒 SuperAdmin |

### 🛠️ Admin — Products
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/products` | List available products | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/archived` | List archived products | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/suspended` | List suspended products | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/{id}/available` | Product detail | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/{id}/archived` | Archived product detail | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/{id}/suspend` | Suspended product detail | 🔒 SuperAdmin |
| `DELETE` | `/api/admin/products/{id}` | Suspend product | 🔒 SuperAdmin |
| `PATCH` | `/api/admin/products/{id}/unsuspend` | Unsuspend product | 🔒 SuperAdmin |
| `GET` | `/api/admin/products/{id}/images` | Product images | 🔒 SuperAdmin |
| `DELETE` | `/api/admin/products/{id}/images/{imgId}` | Remove inappropriate image | 🔒 SuperAdmin |
| `DELETE` | `/api/admin/products/{id}/images` | Clear all product images | 🔒 SuperAdmin |

### 🛠️ Admin — Orders
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/orders/processing` | Processing orders for a day | 🔒 Admin / SuperAdmin |
| `GET` | `/api/admin/orders/overview` | Revenue & order overview | 🔒 Admin / SuperAdmin |
| `PATCH` | `/api/admin/orders/{id}/shipped` | Mark order as shipped | 🔒 Admin / SuperAdmin |
| `POST` | `/api/admin/orders/{returnReqId}/accept-reject-return-req` | Approve/reject return request | 🔒 Admin / SuperAdmin |

### 🛠️ Admin — Users
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/users` | List users | 🔒 SuperAdmin |
| `GET` | `/api/users/{id}` | User detail | 🔒 SuperAdmin |
| `PATCH` | `/api/users/{id}/block` | Block user | 🔒 SuperAdmin |
| `PATCH` | `/api/users/{id}/unblock` | Unblock user | 🔒 SuperAdmin |
| `DELETE` | `/api/users/{id}` | Delete user | 🔒 SuperAdmin |
| `PATCH` | `/api/users/{id}/restore` | Restore user | 🔒 SuperAdmin |

### 🛠️ Admin — Vendors
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/vendors` | List vendors | 🔒 SuperAdmin |
| `GET` | `/api/admin/vendors/{id}` | Vendor detail | 🔒 SuperAdmin |
| `PATCH` | `/api/admin/vendors/{id}/active` | Activate vendor | 🔒 SuperAdmin |
| `PATCH` | `/api/admin/vendors/{id}/deactive` | Deactivate vendor | 🔒 SuperAdmin |

### 🛠️ Admin — Feedback
| Method | Endpoint | Description | Auth |
|:------:|:---------|:------------|:----:|
| `GET` | `/api/admin/feedbacks/pending` | Pending reviews queue | 🔒 SuperAdmin |
| `PATCH` | `/api/admin/feedbacks/{id}/approve` | Approve review | 🔒 SuperAdmin |

<br/>

---

<br/>

## 🚀 Frontend Integration Guide

This API uses standardized patterns designed to streamline frontend integration. Please refer to the guidelines below before starting.

### 1. Using the OpenAPI Specification
An `openapi.yaml` file is provided in the repository root containing all major endpoints and schemas.
**To import into Postman/Insomnia:**
1. Download or clone this repository.
2. Open Postman, click **Import**, and select the `openapi.yaml` file.
3. All collections, endpoints, and request/response models will be generated automatically.

### 2. The Result Pattern
Every API response (success or failure) is wrapped in a consistent `Result<T>` structure. This guarantees a predictable shape for your HTTP clients (like Axios or Fetch).

**Structure:**
```json
{
  "isSuccess": true,
  "isFailure": false,
  "error": {
    "code": "Error.None",
    "message": "No error occurred"
  },
  "value": { ... } // Your requested data goes here
}
```
**Handling Errors:**
If an error occurs (e.g., validation failed, entity not found), `isSuccess` will be `false`, and the `error` object will contain a specific `code` and `message` to display to the user.

### 3. Authentication Flow (JWT)
The API uses JWT Bearer tokens for authentication and short-lived access tokens with long-lived refresh tokens.
1. **Login:** Call `/api/Auth/login` to receive an `accessToken` and a `refreshToken`.
2. **Authorized Requests:** Attach the `accessToken` in the `Authorization` header: `Bearer <your_token>`.
3. **Refresh:** When the `accessToken` expires (returns 401 Unauthorized), call `/api/Auth/refresh-token` using your `refreshToken` to obtain a new pair.

<br/>

---

<br/>

## 🔐 Environment Variables

The live production environment on Monster ASP.NET relies on the following key environment configurations. (Note: Actual values are kept secure via Secrets/Azure Key Vault).

| Variable Area | Purpose |
|:--------------|:--------|
| **ConnectionStrings:DefaultConnection** | Points to the live SQL Server database. |
| **RedisSettings:ConnectionString** | Points to the live Redis instance for caching. |
| **JWT:SecretKey** | High-entropy key used to sign all JWT access tokens. |
| **JWT:Issuer & Audience** | Validates the token source and intended consumer. |
| **CloudinarySettings** | Contains `CloudName`, `ApiKey`, and `ApiSecret` for image uploads. |

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

### 3️⃣ Run the Application

```bash
dotnet run --project E-Commerce.API
```

> Migrations are applied and the database is seeded automatically on startup via `DbInitializer.SeedAsync`.

The interactive API docs are available at:

```
https://localhost:{port}/scalar/v1
```

### 🧪 Test Accounts

The database is automatically seeded with the following accounts on first run:

| Role | Email | Password | Notes |
|:-----|:------|:---------|:------|
| **Super Admin** | `admin@ecommerce.com` | `Admin@123` | Full platform access |
| **Vendor** | `vendor@ecommerce.com` | `Vendor@123` | Vendor profile + products pre-seeded |
| **Customer** | `customer@ecommerce.com` | `Customer@123` | Default shipping address pre-seeded |
| **Representative** | `rep@ecommerce.com` | `Rep@123` | Handles return request completion |

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
| **Soft Delete** | EF Core interceptor auto-sets `IsDeleted` flags with global query filters |
| **Options Pattern** | Strongly-typed, validated configuration sections |
| **Convention-Based DI** | Scrutor auto-registers services by interface markers |

<br/>

---

<br/>

## 🛡️ Security

- 🔑 **JWT Bearer Authentication** with configurable expiration and refresh token rotation
- 🔒 **Role-Based Authorization** policies per endpoint group
- 🧱 **Rate Limiting** — sliding window per IP (auth) and per user (all other endpoints)
- 🔐 **Account Lockout** — auto-lock after 5 failed login attempts (10-minute window)
- 🛡️ **CORS** — configurable allowed origins via `appsettings.json`
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
