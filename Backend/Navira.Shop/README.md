# Navira.Shop

**A modular e-commerce platform built on ASP.NET Core 9**

> ⚠️ **This project is currently in the framework/infrastructure stage.** The core architecture and cross-cutting concerns are implemented. Business feature modules (products, cart, orders, payments) are planned and will be added incrementally.

---

## Project Overview

Navira.Shop is a backend e-commerce platform designed with a modular, clean architecture approach. The solution separates concerns into distinct class libraries, each responsible for a specific infrastructure capability — from caching and persistence to security and messaging.

The goal is to establish a robust, extensible foundation that business modules can plug into with minimal friction. All cross-cutting concerns (dependency injection, exception handling, caching, authentication, input validation, rate limiting, and message queuing) are already in place.

---

## Architecture

The solution follows a **Modular Clean Architecture** pattern. Rather than traditional layered folders, each infrastructure concern lives in its own class library project, promoting separation of responsibility and independent evolution.

```
┌──────────────────────────────────────────────────────┐
│                   Navira.Shop.Api                    │  ← Entry point (controllers, Program.cs)
├──────────────────────────────────────────────────────┤
│                Navira.Shop.Core.Web                  │  ← Middleware, service/app builder extensions
├──────────┬──────────┬──────────┬─────────────────────┤
│  .Bus    │ .Caching │ .Security│ .Mq                 │  ← Infrastructure modules
├──────────┴──────────┴──────────┴─────────────────────┤
│  .Configuration │ .Extensions │ .Mapper │ .Ioc       │  ← Support / utility modules
├──────────────────────────────────────────────────────┤
│  .Persistence │ .Persistence.EF │ .Infrastructure    │  ← Data access & type discovery
├──────────────────────────────────────────────────────┤
│                  Navira.Shop.Core                    │  ← Domain contracts (entities, results, caching)
└──────────────────────────────────────────────────────┘
```

**Key architectural decisions:**

- **Convention-based auto-registration** — Services, repositories, dependency registrars, and mapping configurations are discovered at startup via `ITypeFinder` and registered automatically.
- **CQRS-lite command/query bus** — Commands and queries flow through `IBus` / `IQueryBus` with dedicated handlers, keeping business logic decoupled from controllers.
- **Pluggable app configuration** — Modules implement `IAppConfigService` or `IDependencyRegistrar` to hook into the startup pipeline without modifying the API project.

---

## Implemented Framework Components

| Component | Project(s) | Description |
|---|---|---|
| **Dependency Injection** | `Core.Ioc`, `Core.Web` | Auto-discovery of `IDependencyRegistrar` implementations; convention-based service registration via `ITypeFinder` |
| **Command / Query Bus** | `Core.Bus` | `IBus` for commands, `IQueryBus` for queries, `ICommandHandler<T>` / `IQueryHandler<T, R>` contracts with `IUnitOfWork` integration |
| **Exception Handling** | `Core.Web` | `ExceptionMiddleware` catches `ResultException`, `ApiException`, and unhandled exceptions; returns structured JSON responses with trace IDs |
| **Input Sanitization** | `Core.Web` | `SafeInputMiddleware` validates and sanitizes incoming request bodies |
| **Rate Limiting** | `Core.Web`, `Core` | `RateLimitMiddleware` with `[LimitRequests]` attribute for per-endpoint throttling backed by cache |
| **User Activity Tracking** | `Core.Web` | `UserActivityMiddleware` tracks online users via cache |
| **Caching** | `Core.Caching`, `Core` | Dual-mode caching — in-memory (`MemoryCacheManager`) or Redis (`RedisCacheManager`) with `IStaticCacheManager` abstraction, cache key generation, and distributed locking |
| **Persistence** | `Core.Persistence`, `Core.Persistence.EF` | Generic `IRepository<T, TKey>` with separate `IWriteRepository` / `IQueryRepository` contracts; `IUnitOfWork` for transactional consistency |
| **Entity Model** | `Core` | `IEntity<TKey>`, `IAggregateRoot`, `ISoftDeletableEntity`, `IHasRowVersion`, `IMemoryOptimize` base contracts |
| **Object Mapping** | `Core.Mapper` | Mapster-based mapping with `IMapConfig` auto-discovery and `Map<T>()` extension methods |
| **Configuration** | `Core.Configuration` | Strongly-typed `AppSettings` with sections for Redis, Cache, Keycloak, Scalar, REST clients, and system info |
| **Authentication** | `Core.Web`, `Core.Security` | JWT Bearer authentication, cookie auth, `SecretKeyAuthFilter` for service-to-service auth, `IAppEngin` for current user context |
| **Message Queue** | `Core.Mq` | MassTransit + RabbitMQ integration with `IPublisher` for event publishing and structured logging |
| **API Documentation** | `Core.Web` | Scalar API reference with OpenAPI, configurable via `ScalarConfig` |
| **Health Checks** | `Core`, `Core.Caching`, `Core.Bus` | `IHealthCheckRegistrar` interface; Redis and RabbitMQ health check packages included |
| **Result Pattern** | `Core` | `IResult` / `IResult<T>` with `Result.Success()` / `Result.Fail()` factory methods and `ResultException` for flow control |
| **Extensions & Helpers** | `Core.Extensions` | String, enum, object, collection, and `HttpContext` extensions; `CommonHelper`, `HashHelper`, Excel utilities (`ClosedXML`) |
| **Validation** | `Core.Bus` | FluentValidation integrated into the command bus pipeline |
| **Infrastructure** | `Core.Infrastructure` | `ITypeFinder` / `WebAppTypeFinder` for assembly scanning; `ICoreFileProvider` for file system abstraction |

---

## Technology Stack

| Category | Technology |
|---|---|
| **Runtime** | .NET 9 / ASP.NET Core 9 |
| **ORM** | Entity Framework Core 9 |
| **Micro ORM** | Dapper |
| **Caching** | In-memory cache / Redis (StackExchange.Redis, RedLock.net) |
| **Message Broker** | RabbitMQ via MassTransit |
| **Authentication** | JWT Bearer, Cookie Auth, Keycloak (config ready) |
| **Object Mapping** | Mapster |
| **Validation** | FluentValidation |
| **Serialization** | System.Text.Json, Newtonsoft.Json |
| **API Documentation** | OpenAPI + Scalar |
| **REST Client** | RestEase |
| **Excel Processing** | ClosedXML, ExcelDataReader |
| **Health Checks** | ASP.NET Core Health Checks (Redis, RabbitMQ) |

---

## Project Structure

```
Navira.Shop/
├── Navira.Shop.Api/                    # Web API entry point
│   ├── Controllers/                    # API controllers
│   ├── Program.cs                      # Application bootstrap
│   ├── appsettings.json                # Base configuration
│   └── appsettings.Development.json    # Development overrides
│
├── Navira.Shop.Core/                   # Domain contracts & abstractions
│   ├── Bus/                            # IEvent, IMessage interfaces
│   ├── Caching/                        # Cache key, IStaticCacheManager, ILocker
│   ├── ComponentModel/                 # ReaderWriteLock utilities
│   ├── Entity/                         # IEntity, IAggregateRoot, ISoftDeletableEntity
│   ├── HealthCheck/                    # IHealthCheckRegistrar
│   ├── JsonConverter/                  # Custom JSON converters
│   ├── Persistence/                    # IRepository, IUnitOfWork
│   ├── RateLimit/                      # LimitRequestsAttribute, ClientStatistics
│   ├── Results/                        # IResult, Result, ResultException
│   └── Service/                        # IBaseService marker interface
│
├── Navira.Shop.Core.Bus/              # Command/query bus (CQRS)
│   ├── IBus.cs / IQueryBus            # Bus contracts
│   ├── ICommand.cs / ICommandHandler  # Command pipeline
│   └── BusControl.cs                  # Bus implementation with FluentValidation
│
├── Navira.Shop.Core.Caching/          # Caching implementations
│   ├── MemoryCacheManager.cs          # In-memory cache
│   ├── RedisCacheManager.cs           # Redis-backed cache
│   └── Redis/                         # Redis connection wrapper
│
├── Navira.Shop.Core.Configuration/    # Strongly-typed settings
│   ├── AppSettings.cs                 # Root configuration class
│   ├── RedisConfig.cs                 # Redis settings
│   ├── CacheConfig.cs                 # Cache behavior settings
│   ├── KeycloakConfig.cs              # Keycloak integration settings
│   └── ScalarConfig.cs                # API documentation settings
│
├── Navira.Shop.Core.Extensions/       # Utility extension methods
│   ├── Helper/                        # CommonHelper, HashHelper
│   └── *.cs                           # String, enum, object, collection extensions
│
├── Navira.Shop.Core.Infrastructure/   # Type discovery & file system
│   ├── WebAppTypeFinder.cs            # Assembly scanning
│   └── CoreFileProvider.cs            # File system abstraction
│
├── Navira.Shop.Core.Ioc/             # IoC contracts
│   └── IDependencyRegistrar.cs        # Module DI registration interface
│
├── Navira.Shop.Core.Mapper/          # Object mapping
│   ├── IMapConfig.cs                  # Mapping configuration interface
│   └── MapperExtensionMethods.cs      # Map<T>() extensions (Mapster)
│
├── Navira.Shop.Core.Persistence/     # Persistence abstractions (standalone)
│
├── Navira.Shop.Core.Persistence.EF/  # EF Core repository contracts
│   └── RepositoresContracts/          # IRepository, IWriteRepository, IQueryRepository
│
├── Navira.Shop.Core.Security/        # Authentication & authorization
│   ├── AppEngin.cs                    # Current user context (IAppEngin)
│   ├── SecretKeyAuthFilter.cs         # Service-to-service secret key auth
│   ├── NaviraEncryption.cs            # Encryption utilities
│   └── TokenSensitiveData.cs          # Token handling
│
├── Navira.Shop.Core.Web/             # Web layer orchestration
│   ├── Config/                        # IAppConfigService
│   ├── Extentions/                    # ServiceCollection & ApplicationBuilder extensions
│   └── Middleware/                    # Exception, SafeInput, RateLimit, UserActivity
│
├── NaviraShop.Core.Mq/               # Message queue (MassTransit + RabbitMQ)
│   ├── IPublisher.cs                  # Publishing contract
│   ├── Publisher.cs                   # MassTransit publisher
│   └── MessageFactory.cs              # Log message factory
│
└── Navira.Shop.slnx                  # Solution file
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (or another EF Core-compatible database)
- Redis (optional — required only if `RedisConfig.Enabled` is `true`)
- RabbitMQ (optional — required for message queue functionality)

### Clone the Repository

```bash
git clone https://github.com/your-org/Navira.Shop.git
cd Navira.Shop/Backend/Navira.Shop
```

---

## Installation

**Restore packages:**

```bash
dotnet restore Navira.Shop.slnx
```

**Build the solution:**

```bash
dotnet build Navira.Shop.slnx
```

**Run the API project:**

```bash
dotnet run --project Navira.Shop.Api
```

**Run database migrations** (when EF migrations are added):

```bash
dotnet ef database update --project Navira.Shop.Api
```

Once running, the Scalar API documentation is available at the configured endpoint (enabled by default in development).

---

## Environment Configuration

Configuration is managed through `appsettings.json` and environment-specific overrides (e.g., `appsettings.Development.json`). All settings bind to the strongly-typed `AppSettings` class.

### Key Configuration Sections

| Section | Purpose |
|---|---|
| `ConnectionStrings` | Database connection strings (keyed dictionary) |
| `RedisConfig` | Redis connection, caching toggle, data protection key storage |
| `CacheConfig` | Cache key prefix, default/short-term/bundled cache durations |
| `Keycloak` | Keycloak identity provider settings (BaseUrl, Realm, ClientId, ClientSecret) |
| `Scalar` | API documentation settings (Enabled, Version, UrlPrefix) |
| `SystemInfo` | Application identity (Id, Name, Title, Description, AllowOrigins) |
| `RestsConfig` | External REST service endpoints and tokens |
| `Issuer` / `Audience` / `SigningKey` | JWT authentication parameters |
| `Logging` | Standard ASP.NET Core log level configuration |
| `Values` | Generic key-value store for application-specific settings |

### Environment Variables

All configuration sections can be overridden using environment variables following the standard ASP.NET Core pattern:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=NaviraShop;..."
export RedisConfig__Enabled=true
export RedisConfig__ConnectionString="localhost:6379"
export ASPNETCORE_ENVIRONMENT=Development
```

---

## Current Development Status

| Area | Status |
|---|---|
| Core architecture & modular structure | ✅ Complete |
| Dependency injection & auto-registration | ✅ Complete |
| Command/query bus (CQRS) | ✅ Complete |
| Exception handling middleware | ✅ Complete |
| Input sanitization middleware | ✅ Complete |
| Rate limiting middleware | ✅ Complete |
| Caching (in-memory + Redis) | ✅ Complete |
| Repository pattern (EF Core) | ✅ Complete |
| JWT authentication infrastructure | ✅ Complete |
| MassTransit / RabbitMQ messaging | ✅ Complete |
| Object mapping (Mapster) | ✅ Complete |
| API documentation (Scalar) | ✅ Complete |
| Health checks infrastructure | ✅ Complete |
| Result pattern | ✅ Complete |
| Business domain models | 🔲 Planned |
| Product management | 🔲 Planned |
| Shopping cart | 🔲 Planned |
| Order processing | 🔲 Planned |
| Payment integration | 🔲 Planned |
| Admin panel | 🔲 Planned |

---

## Roadmap

The following business features are planned for upcoming development phases:

- [ ] **Product Management** — Categories, product catalog, inventory tracking, image management
- [ ] **Shopping Cart** — Cart operations, session-based and authenticated carts, price calculations
- [ ] **Order Processing** — Order creation, status management, order history
- [ ] **Payment Integration** — Payment gateway integration, transaction management, refund handling
- [ ] **User Management** — Customer profiles, address book, order history
- [ ] **Admin Panel** — Dashboard, product/order/user management, analytics
- [ ] **Search & Filtering** — Full-text search, faceted filtering, sorting
- [ ] **Notifications** — Email and SMS notifications for order events
- [ ] **Promotions & Discounts** — Coupon codes, promotional pricing, bulk discounts

---

## Contributing

Contributions are welcome! To get started:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m "Add your feature"`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

Please ensure your code follows the existing project conventions and includes appropriate documentation.

---

## License

This project is licensed under the [MIT License](LICENSE).
