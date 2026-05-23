# NN Toolkit

`NN Toolkit` is a .NET library for working with files exported from NN (Nationale-Nederlanden), currently focused on
parsing PDF contribution statements for the Romanian Mandatory Private Pension (Pilon II).

NN Group reference:

- https://en.wikipedia.org/wiki/NN_Group

The package is published as `DustInTheWind.NN.Toolkit`.

## What The Library Does Today

The current public functionality is centered around one workflow:

1. Load a PDF exported from the NN Direct mobile app.
2. Parse the contributions table across all pages: `ContributionsDocument`
3. Provide parsing diagnostics: `DocumentParsingDiagnostics`

## Installation

Package Manager:

```powershell
Install-Package DustInTheWind.NN.Toolkit
```

.NET CLI:

```bash
dotnet add package DustInTheWind.NN.Toolkit
```

## Runtime Requirements

- Library target framework: `.NET 8.0` (`net8.0`)
- The parser relies on PDF table extraction via `Tabula`.

If you are consuming the NuGet package, transitive dependencies are resolved automatically.

## Quick Start

### a) Export the Contributions PDF From NN Direct

In NN Direct mobile app:

1. Log in.
2. Open **Pensie privata obligatorie**.
3. Open **Istoric contribuții**.
4. Select the full interval you need.
5. Use the menu (three dots) and tap **Descarcă raport**.

You will get a PDF containing contribution rows that can be parsed with this toolkit.

### b) Parse the Exported Document

Create a small console app and parse your exported file:

```csharp
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

DocumentLoadResult result = ContributionsDocument.LoadFromFile("contributions.pdf");
ContributionsDocument document = result.Document;

foreach (Contribution contribution in document)
{
	...
}
```

## `Contribution` Record

Each row is mapped to:

- `Month` (`MonthDate`)
    - expects date in the `MM/YYYY` style.

- `GrossValue` (`decimal`)
- `AdministrationFee` (`decimal`)
- `NetValue` (`decimal`)
- `UnitValue` (`decimal`)
- `UnitCount` (`decimal`)
- `PaidInMonth` (`MonthDate`)
    - expects date in the `MM/YYYY` style.

## Parsing Diagnostics

When a document is parsed using `ContributionsDocument.LoadFromFile()`, both the parsed data and parsing diagnostics are
returned.

```csharp
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

DocumentLoadResult result = ContributionsDocument.LoadFromFile("contributions.pdf");

foreach (PageParsingDiagnostics page in result.Diagnostics.Pages)
{
	...
}
```

## End-To-End Example: Export To CSV

The repository includes a sample CLI project in `sources/NN.Toolkit.Cli` that demonstrates:

- reading `contributions.pdf`
- writing CSV files (`NN_transactions.csv`, `NN_cash_transactions.csv`)
- printing parsed data and diagnostics.

You can use this project as a reference implementation for your own importer/exporter tools.
