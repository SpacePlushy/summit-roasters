# Summit Roasters

Enterprise-grade ASP.NET Core MVC e-commerce application for a specialty coffee roastery. Built with Clean Architecture as a realistic test target for comprehensive unit, integration, and end-to-end test suites.

## Tech Stack

- **.NET 10** / C# / ASP.NET Core MVC
- **Entity Framework Core** with SQLite
- **ASP.NET Identity** (cookie-based authentication)
- **Tailwind CSS** (CDN) + vanilla JavaScript
- **xUnit** + Moq + FluentAssertions (unit & integration tests)
- **Playwright** (end-to-end tests)

## Architecture

Clean Architecture with three layers:

```
SummitRoasters.slnx
├── src/
│   ├── SummitRoasters.Core/           # Domain models, enums, DTOs, interfaces, services
│   ├── SummitRoasters.Infrastructure/ # EF Core DbContext, repositories, data seeder
│   └── SummitRoasters.Web/           # MVC controllers, views, view models, JS, CSS
└── tests/
    ├── SummitRoasters.UnitTests/
    ├── SummitRoasters.IntegrationTests/
    └── SummitRoasters.E2ETests/
```

**Core** has no ASP.NET dependencies. **Infrastructure** implements data access. **Web** handles HTTP concerns, views, and static assets.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

No other dependencies required. SQLite database is created automatically on first run.

## Getting Started

```bash
# Clone the repository
git clone https://github.com/spaceplushy/summit-roasters.git
cd summit-roasters

# Build the solution
dotnet build SummitRoasters.slnx

# Run the application
dotnet run --project src/SummitRoasters.Web
```

The app will be available at `https://localhost:5001` (or the port shown in the console). The database is created and seeded automatically on first startup.

## Running Tests

```bash
# Run all tests (unit + integration)
dotnet test

# Run only unit tests
dotnet test tests/SummitRoasters.UnitTests

# Run only integration tests
dotnet test tests/SummitRoasters.IntegrationTests

# Run E2E tests (requires the app to be running)
dotnet test tests/SummitRoasters.E2ETests
```

## Seed Data

On first run, the application seeds the database with sample data:

### Users

| Role     | Email                        | Password       |
|----------|------------------------------|----------------|
| Admin    | `admin@summitroasters.com`   | `Admin123!`    |
| Customer | `sarah@example.com`          | `Customer123!` |
| Customer | `mike@example.com`           | `Customer123!` |

### Products & Content

- 30 specialty coffee products across multiple categories
- 50+ product reviews
- 10 sample orders

### Discount Codes

| Code         | Description           |
|--------------|-----------------------|
| `WELCOME10`  | 10% off               |
| `FIVEBUCKS`  | $5 off                |
| `FREESHIP`   | Free shipping         |

## Key Features

- Product catalog with filtering, search, and pagination
- Product detail pages with reviews, ratings, and brewing tips
- Shopping cart with session-based storage
- Multi-step checkout with discount code support
- User account management with order history
- Admin dashboard for product, order, and user management
- Responsive design with Tailwind CSS

## Test IDs

All interactive elements use `data-testid` attributes following the `{component}-{element}-{qualifier}` pattern for reliable Playwright targeting.
