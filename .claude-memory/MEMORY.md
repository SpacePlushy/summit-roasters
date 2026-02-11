# Summit Roasters - Project Memory

## Project Status
- **All 6 phases complete**: ~147 files, builds clean, 17 tests pass (12 unit + 5 integration + 3 skipped E2E)
- See `implementation-status.md` for detailed checklist
- See `architecture.md` for architectural patterns and conventions
- GitHub: `SpacePlushy/summit-roasters` (public)

## Critical Facts
- Solution file is `.slnx` (not `.sln`) - use `dotnet build SummitRoasters.slnx`
- .NET 10 SDK 10.0.102 on macOS (darwin), no PowerShell available
- SQLite database auto-created on first run, seeded by `DataSeeder`
- Tailwind via CDN play script (no npm build needed)

## Gotchas Learned
- Razor `@{` inside existing code blocks (if/else/foreach) causes build errors - use bare `{` instead
- Agent-generated views often have unbalanced HTML tags - always verify build after view generation
- `.slnx` is the newer solution format in .NET 10 - `dotnet new sln` creates it by default
- **EF Core 10 integration testing**: Must remove `IDbContextOptionsConfiguration<T>` descriptors (not just `DbContextOptions<T>`) when swapping SQLite for InMemory in WebApplicationFactory, or you get dual-provider conflict
- FluentAssertions 8.x has breaking API changes from 6.x - check method signatures

## Seed Data Credentials
- Admin: admin@summitroasters.com / Admin123!
- Customers: sarah@example.com, mike@example.com / Customer123!
- Discount codes: WELCOME10 (10%), FIVEBUCKS ($5 off), FREESHIP

## Test Expansion
- ~64 TODO tests documented across unit/integration/E2E files
