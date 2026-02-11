# Summit Roasters - Project Memory

## Project Status
- **All 6 phases complete**: ~215 source files, 0 build errors, 17 tests pass (12 unit + 5 integration + 3 skipped E2E)
- See `implementation-status.md` for detailed checklist
- See `architecture.md` for architectural patterns and conventions
- GitHub: `SpacePlushy/summit-roasters` (public)

## What Still Needs To Be Done

### High Priority
1. **Smoke test the running app** — `dotnet run` has never been executed. DataSeeder, page routing, session cart, and auth flows are untested at runtime. There may be runtime errors that compile fine but crash on HTTP request.
2. **Fix 4 build warnings** — `Products/Detail.cshtml` line 6 has nullable reference type mismatches (`CS8620`). Should be a quick fix to the breadcrumb tuple type.
3. **Implement ~64 TODO tests** — Test scaffolding is in place but most tests are placeholders:
   - Unit: ~24 TODOs (discount edge cases, stock release, cart merging, order status transitions, review/search services)
   - Integration: ~8 TODOs (OrderFlowTests is entirely TODO, plus gaps in other files)
   - E2E: ~32 TODOs (Cart, Checkout, Search, Review, Admin tests are all TODO-only)

### Nice To Have
4. **README.md** — GitHub repo has no landing page / project description
5. **GitHub Actions CI** — No workflow to run `dotnet test` on push
6. **E2E test infrastructure** — Playwright browsers aren't installed; no test fixture for launching app + browser together
7. **Git author config** — Commits show auto-detected hostname identity instead of user name/email

## Critical Facts
- Solution file is `.slnx` (not `.sln`) — use `dotnet build SummitRoasters.slnx`
- .NET 10 SDK 10.0.102 on macOS (darwin), no PowerShell available
- SQLite database auto-created on first run, seeded by `DataSeeder`
- Tailwind via CDN play script (no npm build needed)
- Git remote uses SSH (`git@github.com:SpacePlushy/summit-roasters.git`) — HTTPS auth not configured

## Gotchas Learned
- Razor `@{` inside existing code blocks (if/else/foreach) causes build errors — use bare `{` instead
- Agent-generated views often have unbalanced HTML tags — always verify build after view generation
- `.slnx` is the newer solution format in .NET 10 — `dotnet new sln` creates it by default
- **EF Core 10 integration testing**: Must remove `IDbContextOptionsConfiguration<T>` descriptors (not just `DbContextOptions<T>`) when swapping SQLite for InMemory in WebApplicationFactory, or you get dual-provider conflict
- FluentAssertions 8.x has breaking API changes from 6.x — check method signatures

## Seed Data Credentials
- Admin: admin@summitroasters.com / Admin123!
- Customers: sarah@example.com, mike@example.com / Customer123!
- Discount codes: WELCOME10 (10%), FIVEBUCKS ($5 off), FREESHIP
