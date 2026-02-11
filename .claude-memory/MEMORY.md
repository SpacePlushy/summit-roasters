# Summit Roasters - Project Memory

## Project Status
- **Phases 1-4 complete**: Solution scaffolding, Core domain, Infrastructure, Web app (all build clean)
- **Phase 5 TODO**: Test scaffolding (unit tests, integration tests, E2E page objects + tests)
- **Phase 6 TODO**: Final verification, global.json, smoke testing
- See `implementation-status.md` for detailed file inventory
- See `architecture.md` for architectural patterns and conventions

## Critical Facts
- Solution file is `.slnx` (not `.sln`) - use `dotnet build SummitRoasters.slnx`
- .NET 10 SDK 10.0.102 on macOS (darwin), no PowerShell available
- SQLite database auto-created on first run, seeded by `DataSeeder`
- Tailwind via CDN play script (no npm build needed)
- User's GitHub: `SpacePlushy`, repo: `summit-roasters` (public)

## Gotchas Learned
- Razor `@{` inside existing code blocks (if/else/foreach) causes build errors - use bare `{` instead
- Agent-generated views often have unbalanced HTML tags - always verify build after view generation
- `.slnx` is the newer solution format in .NET 10 - `dotnet new sln` creates it by default
- The `dotnet new gitignore` template does NOT exclude `.sln`/`.slnx` files
- FluentAssertions 8.x has breaking API changes from 6.x - check method signatures
