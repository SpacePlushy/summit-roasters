# Summit Roasters - Claude Code Project Guide

## Project Overview
Enterprise-grade ASP.NET Core MVC e-commerce app (specialty coffee roastery). Serves as a realistic test target for comprehensive test suites. Built with Clean Architecture: Core → Infrastructure → Web.

## Tech Stack
- **.NET 10** (SDK 10.0.102), **C#**, **ASP.NET Core MVC**
- **EF Core** with SQLite (dev), **ASP.NET Identity** for auth
- **Tailwind CSS** via CDN (play script), vanilla JS (no build step)
- **xUnit** + Moq + FluentAssertions for unit/integration tests
- **Playwright** for E2E tests

## Solution Structure
```
SummitRoasters.slnx
├── src/
│   ├── SummitRoasters.Core/        # Domain models, enums, DTOs, interfaces, services (no ASP.NET deps)
│   ├── SummitRoasters.Infrastructure/ # EF Core DbContext, repositories, data seeder
│   └── SummitRoasters.Web/         # MVC controllers, views, viewmodels, JS, CSS
└── tests/
    ├── SummitRoasters.UnitTests/
    ├── SummitRoasters.IntegrationTests/
    └── SummitRoasters.E2ETests/
```

## Build & Run Commands
```bash
dotnet build SummitRoasters.slnx          # Build entire solution
dotnet run --project src/SummitRoasters.Web  # Run app (seeds DB on first start)
dotnet test                                # Run all tests
dotnet test tests/SummitRoasters.UnitTests  # Run unit tests only
```

## Key Architecture Decisions
- **Cart storage**: Session-based via `ICartStorage` abstraction (Core stays ASP.NET-free)
- **FlavorNotes**: Stored as comma-separated string (SQLite compat)
- **Auth**: Manual Identity config (no `--auth` scaffold), cookie-based
- **CSRF for AJAX**: Token from hidden form field + JS header
- **Admin**: Separate `_AdminLayout.cshtml` with sidebar nav
- **Checkout**: Single page with JS-toggled steps (all in DOM for Playwright)

## Seed Data
On first run, seeds: 3 users (admin/sarah/mike), 30 products, 50+ reviews, 10 orders, 3 discount codes.
- Admin: `admin@summitroasters.com` / `Admin123!`
- Customers: `sarah@example.com`, `mike@example.com` / `Customer123!`
- Discount codes: `WELCOME10`, `FIVEBUCKS`, `FREESHIP`

## Test IDs
All interactive elements use `data-testid` attributes following `{component}-{element}-{qualifier}` pattern for Playwright targeting.

## Current State
All 6 implementation phases complete. 215 source files, 0 build errors (4 nullable warnings), 17 tests passing.

## What's Remaining
- **Smoke test** — `dotnet run` has never been executed; runtime behavior is unverified
- **Fix 4 build warnings** — CS8620 nullable mismatch in `Products/Detail.cshtml` line 6
- **~64 TODO tests** — Test scaffolding is in place, most tests are placeholders (see `.claude-memory/implementation-status.md`)
- **README.md** — No repo landing page yet
- **GitHub Actions CI** — No automated test workflow

## Conventions
- File-scoped namespaces throughout
- ViewModels map from domain models (no AutoMapper)
- Repositories return tuples `(List<T>, int TotalCount)` for paged results
- API controllers return anonymous objects `{ itemCount, subtotal }`
- Decimal precision: `(18, 2)` for all money columns
