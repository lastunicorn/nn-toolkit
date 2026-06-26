# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NN Toolkit is a .NET 8 library (`DustInTheWind.NN.Toolkit`) for working with files exported from NN (Nationale-Nederlanden) — specifically parsing PDF contribution statements for the Romanian Mandatory Private Pension (Pilon II), and querying the NN REST API for fund graph data.

The NuGet package ID is `DustInTheWind.NN.Toolkit`. Version is set at build time (not in source); `Directory.Build.props` holds `0.0.0.0` as a placeholder.

## Commands

```bash
# Restore, build, test
dotnet restore ./NN.Toolkit.slnx --configfile ./nuget.config
dotnet build ./NN.Toolkit.slnx -c Release
dotnet test ./NN.Toolkit.slnx

# Run a single test (xUnit)
dotnet test ./sources/NN.Toolkit.Tests/NN.Toolkit.Tests.csproj --filter "FullyQualifiedName~MonthDateTests"

# Pack the library (CI sets -p:Version=X.Y.Z)
dotnet pack ./sources/NN.Toolkit/NN.Toolkit.csproj -c Release -o ./artifacts
```

## Architecture

Three projects under `sources/`:

| Project | Target | Role |
|---|---|---|
| `NN.Toolkit` | `net8.0` | Library — public API + PDF parsing internals |
| `NN.Toolkit.Tests` | `net10.0` | xUnit unit tests for the library |
| `NN.Toolkit.Demo` | `net10.0` | Console app showing two use cases |

### Library internals (`NN.Toolkit`)

**`MandatoryPrivatePension/`** — PDF parsing pipeline:
- `ContributionsDocument` (public) — entry point; `LoadFromFile()` opens the PDF, iterates rows, maps to `Contribution` records.
- `Pdf/P2PdfDocument` → `P2PdfPage` → `P2PdfTable` → `P2PdfTableRow` — internal layer wrapping Tabula. `P2PdfPage` first tries the Nurminen table-detection algorithm; falls back to whole-page extraction if no regions are found.
- `DocumentLoadResult` carries both the parsed `ContributionsDocument` and `DocumentParsingDiagnostics` (per-page, per-table counts and extraction approach used).
- `MonthDate` — value type for `MM/YYYY` dates used in contribution rows.

**`ApiAccess/`** — REST API client:
- `NnApiClient` / `INnApiClient` — `GetGraph(startDate, endDate, count)` calls `https://www.nn.ro/api/` and returns `GraphData`. API may return duplicate dates; callers should deduplicate by date (ignore the time component).

### Demo app (`NN.Toolkit.Demo`)

Two use cases selectable in `Program.cs` (one commented out at a time):
- `ParseContributionFileUseCase` — reads a local PDF and prints/exports contributions.
- `DisplayPensionFundFromWebUseCase` — fetches fund graph data via `NnApiClient`.

## Code Conventions

- No `var` — always use the explicit type.
- LINQ lambda parameter: use `x` for the item.
- Prefer `new()` (target-typed) when instantiating objects.
- Object initializer with more than one property: one property per line.
- No curly braces for single-line `if`, `for`, `using` bodies.
- XML documentation only on public types that are exposed via the NuGet package; omit it for solution-internal types.

## Test Conventions (xUnit)

- Test file per public method/constructor: e.g., `QueryTests.cs` for `Query()`.
- All test files for a class go in a directory named `<ClassName>Tests/`.
- Naming pattern: `Having<setup>_When<action>_Then<result>`.
- `Assert.Throws` must use a block body lambda (`() => { ... }`), not an expression body.
