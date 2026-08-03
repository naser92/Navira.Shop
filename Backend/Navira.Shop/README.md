# Navira.Shop

Backend services for a modular e-commerce platform built with ASP.NET Core 9, Entity Framework Core, CQRS-style command/query handling, Keycloak authentication, Redis caching, and RabbitMQ messaging.

> **Current state:** the foundation, category write operations, authentication flows, and permission/menu synchronization are implemented. Product aggregates are modeled, but product, cart, order, payment, and complete read APIs are not yet available.

## Implemented Features

- Category creation, update, and soft deletion
- Keycloak login and refresh-token flows
- JWT bearer authentication and role mapping
- Attribute-based permissions and menu metadata
- Permission/menu discovery and database synchronization
- Separate EF Core write and query contexts
- Command/query buses with automatic handler registration
- Unit-of-work transaction handling
- In-memory or Redis-backed caching and locking
- MassTransit/RabbitMQ publishing infrastructure
- Mapster mapping and FluentValidation discovery
- Central exception handling, input normalization, CORS, and user activity middleware
- OpenAPI generation and Scalar API documentation

## Architecture

The solution uses a layered, DDD-inspired architecture supported by reusable Core libraries:

```text
Navira.Shop.Api
       |
       v
Navira.Shop.Application
       |
       v
Navira.Shop.Domain
       ^
       |
Navira.Shop.Infrastructure

Core libraries provide bus, persistence, caching, security,
web middleware, mapping, configuration, and shared primitives.
```

Controllers contain minimal orchestration and dispatch requests through `IBus` or `IQueryBus`. Application handlers coordinate domain objects and repositories. Infrastructure implements database access, Keycloak integration, and persistence mappings.

Startup is convention-based. `WebAppTypeFinder` discovers and registers:

- `IDependencyRegistrar` implementations
- command and query handlers
- validators
- repositories and application services
- Mapster configurations
- application pipeline configuration hooks

## Project Structure

### Application Projects

| Project | Responsibility |
|---|---|
| `Navira.Shop.Api` | ASP.NET Core entry point, controllers, runtime composition |
| `Navira.Shop.Application` | Commands, queries, handlers, DTOs, and application services |
| `Navira.Shop.Domain` | Catalog and identity aggregates, entities, and value objects |
| `Navira.Shop.Infrastructure` | EF Core contexts, repositories, mappings, and Keycloak client |

### Core Projects

| Project | Responsibility |
|---|---|
| `Navira.Shop.Core` | Entities, results, caching contracts, health-check contracts, shared abstractions |
| `Navira.Shop.Core.Domain` | Aggregate roots, value objects, auditing, and domain exceptions |
| `Navira.Shop.Core.Bus` | Command/query bus, handlers, domain events, and MassTransit setup |
| `Navira.Shop.Core.Persistence.EF` | EF Core contexts, repository contracts, mappings, and interceptors |
| `Navira.Shop.Core.Caching` | Memory and Redis cache implementations |
| `Navira.Shop.Core.Security` | Current-user context, permission authorization, and security helpers |
| `Navira.Shop.Core.Web` | Service registration, middleware, API results, OpenAPI, and Scalar |
| `Navira.Shop.Core.Configuration` | Strongly typed application configuration |
| `Navira.Shop.Core.Infrastructure` | Assembly scanning and file-provider abstractions |
| `Navira.Shop.Core.Mapper` | Mapster configuration and mapping extensions |
| `Navira.Shop.Core.Extensions` | General utilities, data helpers, and Excel support |
| `Navira.Shop.Core.Ioc` | Dependency registration contracts |
| `Navira.Shop.Core.Services` | Shared framework services |
| `Navira.Shop.Core.ViewModels` | Shared grid and permission view models |
| `NaviraShop.Core.Mq` | Message publishing and message factory abstractions |

## Domain Status

### Catalog

- Category aggregate, EF mapping, repository, commands, handlers, and API write endpoints are implemented.
- Categories support hierarchy, unique slugs, tax-category references, ordering, active state, auditing, and soft deletion.
- Product, product variant, `Money`, and `Sku` domain models exist.
- Product persistence mappings and product API/application workflows are not yet implemented.

### Identity and Access

- Login and token refresh are delegated to Keycloak.
- JWT realm and client roles are mapped into ASP.NET Core role claims.
- Permissions and menus can be declared on controllers and actions with attributes.
- Permission, menu, policy, policy-permission, and role-policy models and repositories exist.
- The permission API is currently a placeholder; full policy administration endpoints are still in progress.

## Technology Stack

| Area | Technology |
|---|---|
| Runtime | .NET 9 / ASP.NET Core 9 |
| Database | SQL Server |
| ORM | Entity Framework Core 9 |
| Query utilities | Dapper, DevExtreme.AspNet.Data |
| Authentication | Keycloak, JWT Bearer |
| Caching | ASP.NET Core Memory Cache, Redis, RedLock.net |
| Messaging | MassTransit, RabbitMQ |
| Mapping | Mapster |
| Validation | FluentValidation |
| API documentation | OpenAPI, Scalar |
| Serialization | System.Text.Json, Newtonsoft.Json |
| Data utilities | ClosedXML, ExcelDataReader |

## Prerequisites

- .NET 9 SDK
- SQL Server
- A Keycloak realm and client
- RabbitMQ when `RabbitMQ:AutoConfig` is enabled
- Redis when `RedisConfig:Enabled` is enabled

No Docker configuration is currently included.

## Configuration

Development settings are loaded from `Navira.Shop.Api/appsettings.Development.json`. Replace all repository-provided connection strings, tokens, client secrets, and passwords before running the application.

Do not store production secrets in tracked JSON files. Use environment variables, a secret manager, or .NET user secrets:

```bash
cd Navira.Shop.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:WriteConnection" "<sql-server-connection>"
dotnet user-secrets set "ConnectionStrings:ReadConnection" "<sql-server-connection>"
dotnet user-secrets set "KeycloakConfig:BaseUrl" "http://localhost:8080"
dotnet user-secrets set "KeycloakConfig:Realm" "Navira"
dotnet user-secrets set "KeycloakConfig:ClientId" "navira-shop-api"
dotnet user-secrets set "KeycloakConfig:ClientSecret" "<client-secret>"
```

Key configuration sections:

| Section | Purpose |
|---|---|
| `SystemInfo` | Application identity and allowed CORS origins |
| `ConnectionStrings:WriteConnection` | SQL Server write database |
| `ConnectionStrings:ReadConnection` | SQL Server query database |
| `KeycloakConfig` | Keycloak base URL, realm, client ID, and client secret |
| `CacheConfig` | Cache prefix and expiration durations |
| `RedisConfig` | Redis connection, cache selection, and locking behavior |
| `RabbitMQ` | Broker address, credentials, and automatic configuration switch |
| `Scalar` | Enables or disables Scalar/OpenAPI mapping |
| `RestsConfig` | External service definitions |

For a local run without Redis or RabbitMQ:

```bash
export RedisConfig__Enabled=false
export RabbitMQ__AutoConfig=false
```

`KeycloakConfig` must still be present because authentication is configured during startup.

## Build and Run

Run commands from the repository root:

```bash
dotnet restore Navira.Shop.Api/Navira.Shop.Api.csproj
dotnet build Navira.Shop.Api/Navira.Shop.Api.csproj
dotnet run --project Navira.Shop.Api/Navira.Shop.Api.csproj --launch-profile Navira.Shop.Api
```

The default launch profile uses:

- API: `http://localhost:5979`
- Scalar: `http://localhost:5979/scalar/v1`
- OpenAPI JSON: `http://localhost:5979/openapi/v1.json`

An HTTPS launch profile is also available at `https://localhost:7194`.

### Solution File Note

`Navira.Shop.slnx` currently contains machine-specific relative project paths. Use the API project commands above until the solution file is regenerated with portable paths.

### Database Note

The application configures separate `WriteDbContext` and `QueryDbContext` instances. No EF Core migrations are committed, and the current migrations assembly/design-time factory configuration needs correction before migration commands can be used reliably.

## API Surface

| Method | Route | Status |
|---|---|---|
| `POST` | `/api/auth/login` | Implemented |
| `POST` | `/api/auth/refresh` | Implemented |
| `GET` | `/api/auth/me` | Placeholder |
| `POST` | `/api/category` | Implemented, authenticated |
| `PUT` | `/api/category/{id}` | Implemented, authenticated |
| `DELETE` | `/api/category/{id}` | Implemented, authenticated |
| `GET` | `/api/config/PublishPermissions/{name}` | Implemented synchronization utility |
| `POST` | `/api/permission` | Placeholder, authenticated |

API results use the shared `IResult`/`IResult<T>` response pattern. Application-level failures may therefore be represented inside an HTTP 200 response body.

## Current Limitations

- No committed database migrations
- No automated test projects
- No Docker or Compose setup
- No product, cart, order, payment, or inventory APIs
- No category read/list endpoint
- No mapped health-check endpoint despite available health-check abstractions/packages
- Product domain models are not yet connected to persistence or application workflows
- Development configuration contains environment-specific values that must be replaced

## Development Roadmap

- [x] Modular startup and convention-based dependency registration
- [x] Command/query bus and unit-of-work pipeline
- [x] Category write operations
- [x] Keycloak login and refresh
- [x] Permission and menu metadata synchronization
- [x] Memory/Redis caching infrastructure
- [x] RabbitMQ publishing infrastructure
- [ ] Regenerate the portable solution file
- [ ] Add and verify EF Core migrations
- [ ] Add category and catalog query endpoints
- [ ] Complete product persistence and APIs
- [ ] Complete policy and permission administration
- [ ] Add shopping cart, orders, payments, and inventory
- [ ] Add automated tests
- [ ] Add containerized local development

## Contributing

1. Create a focused feature branch.
2. Follow the existing Application/Domain/Infrastructure separation.
3. Add handlers, repositories, mappings, and tests where applicable.
4. Keep secrets and environment-specific configuration out of commits.
5. Build the API project before opening a pull request.
