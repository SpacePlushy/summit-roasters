# Playwright Learning Handoff

Last updated: 2026-02-19
Repo: `summit-roasters`
Branch: `main`

## Goal
Continue Playwright + xUnit learning curriculum and resume from Module 4 without losing context.

## Current Progress

### Completed
- Module 1 exercises completed.
- Module 2 exercises completed.
- Module 3 exercises completed in `tests/SummitRoasters.PlaywrightLearning/Module03_Interactions/Exercise_Interactions.cs`.
  - Included fixes for strict locator scoping (`filter-apply`) and category URL assertion.

### In Progress
- Module 4 file: `tests/SummitRoasters.PlaywrightLearning/Module04_AssertionsMastery/Exercise_Assertions.cs`
- Exercise status:
  - `Exercise01_VerifyHomepageSections` completed.
  - `Exercise02_VerifyProductDetailContent` completed.
  - `Exercise03_CountCategoryCards` completed.
  - `Exercise04_VerifySearchResultsHaveContent` partially completed.
  - `Exercise05_VerifyLoginFormAttributes` not started yet (`NotImplementedException` still present).

## Exact Next Steps
1. Finish Module 4 Exercise 4 in `tests/SummitRoasters.PlaywrightLearning/Module04_AssertionsMastery/Exercise_Assertions.cs`:
   - Keep existing search heading + count assertions.
   - Add first-card assertions for:
     - name locator (`data-testid^='product-card-name-'`)
     - price locator (`data-testid^='product-card-price-'`)
   - Add price text check containing `$`.
2. Complete Module 4 Exercise 5 in `tests/SummitRoasters.PlaywrightLearning/Module04_AssertionsMastery/Exercise_Assertions.cs`.
3. Then move to Module 5 (`tests/SummitRoasters.PlaywrightLearning/Module05_WaitingAndAsync/Exercise_WaitingPatterns.cs`).

## Run Commands
In terminal 1:
```bash
dotnet run --project src/SummitRoasters.Web
```

In terminal 2:
```bash
dotnet test tests/SummitRoasters.PlaywrightLearning --filter "Module04&Exercise"
```

Then when ready:
```bash
dotnet test tests/SummitRoasters.PlaywrightLearning --filter "Module05&Exercise"
```

## Known Notes
- Playwright strict mode can fail if a locator matches duplicate elements (desktop + mobile). Scope locators to parent containers when needed.
- Prefer URL assertion patterns that match actual app behavior (e.g., query string `category=` instead of `/category` path).
- `Locator` creation is synchronous; `CountAsync`/actions/assertions are async.

## For Next Assistant
Use this repo state as baseline and continue coaching style:
- user writes code
- assistant reviews incrementally
- assistant adds temporary `// FIX:` comments only when requested
- keep guidance concise and practical
