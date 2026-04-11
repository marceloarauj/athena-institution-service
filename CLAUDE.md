# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the solution
dotnet build

# Run the API (from Institution/ directory)
dotnet run --project Institution/Institution.csproj

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~ClassName"

# Add an EF Core migration
dotnet ef migrations add <MigrationName> --project Institution.Infrastructure --startup-project Institution

# Apply migrations
dotnet ef database update --project Institution.Infrastructure --startup-project Institution
```

## Local Infrastructure

The API requires two local services:

- **PostgreSQL** on port `5455` — database `postgres_institutiondb`, user `admin`, password `admin123`
- **LocalStack** on port `4566` — emulates AWS S3, bucket `institutions`

Both are configured in `Institution/appsettings.json`.

## Architecture

This is a **Clean Architecture** ASP.NET Core 10 REST API for managing educational institutions within the Athena Students Union platform.

### Layer Overview

| Project | Role |
|---|---|
| `Institution` | ASP.NET Core host — controllers, DI wiring, middleware registration |
| `Institution.Application` | CQRS commands, handlers, DTOs, service/repository interfaces |
| `Institution.Domain` | Entity models, enums — no dependencies on other layers |
| `Institution.Infrastructure` | EF Core DbContext, repositories, service implementations, middleware |
| `Institution.Tests` | xUnit unit tests |

### Request Flow

```
HTTP Request
  → InstitutionMiddleware  (populates InstitutionContext.Alias)
  → Controller             (thin — only calls mediator.Send())
  → IMediator              (from custom AthenaUnionLibrary/Mediator package)
  → Handler                (in Institution.Application/Handlers/)
  → Service / Repository   (concrete impls in Institution.Infrastructure/)
  → AppDbContext (PostgreSQL via EF Core)
```

### CQRS Pattern

Commands live in `Institution.Application/Commands/` as records implementing `IRequestMessage<TResponse>`. Each command has a corresponding handler in `Institution.Application/Handlers/`. All handlers are registered via the custom `Mediator` library in `ServiceCollectionExtension.AddMediatorConfig()`. New commands must be registered there.

### Key Concepts

- **InstitutionContext** — scoped middleware context that carries the institution `Alias` across the request pipeline. Currently hardcoded (TODO: resolve from request headers).
- **UnitOfWork** — wraps EF Core transaction management; injected into handlers alongside repositories.
- **GradeService** — singleton that evaluates NCalc boolean expressions for grade approval (e.g., `"A >= 5 && B >= 6"`). Variables are injected by key from `TestGradeDto`.
- **AmazonService** — wraps AWS S3 SDK, configured via `AmazonS3Options` (bound from `appsettings.json` `"Aws"` section).
- **AthenaApiResponse** — standardized API response wrapper from `AthenaUnionLibrary`; controllers return `response.AsResult()`.

### Extension Method Conventions

DI registration uses C# extension extensions (`extension(IServiceCollection services) { ... }`), a preview .NET feature. Middleware registration uses `extension(WebApplication app) { ... }` in `WebApplicationExtensions`.
