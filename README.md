
# NN Toolkit

`NN Toolkit` is a .NET library for working with files exported from NN (Nationale-Nederlanden), currently focused on parsing PDF contribution statements for the Romanian Mandatory Private Pension (Pilon II).

NN Group reference:

- https://en.wikipedia.org/wiki/NN_Group

The package is published as `DustInTheWind.NN.Toolkit`.

## What The Library Does Today

The current public functionality is centered around one workflow:

1. Load a PDF exported from the NN Direct mobile app.
2. Parse the contributions table across all pages.
3. Return:
   - a strongly-typed `ContributionsDocument` (header + contribution rows)
   - parsing diagnostics (`DocumentParsingDiagnostics`) so you can inspect how extraction behaved.

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

## Export The Contributions PDF From NN Direct

In NN Direct mobile app:

1. Log in.
2. Open **Pensie privata obligatorie**.
3. Open **Istoric contribuții**.
4. Select the full interval you need.
5. Use the menu (three dots) and tap **Descarcă raport**.

You will get a PDF containing contribution rows that can be parsed with this toolkit.

## Quick Start

Create a small console app and parse your exported file:

```csharp
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

DocumentLoadResult result = ContributionsDocument.LoadFromFile("contributions.pdf");

ContributionsDocument document = result.Document;

Console.WriteLine($"Rows: {document.Count}");

foreach (Contribution contribution in document)
{
	Console.WriteLine(
		$"Month={contribution.Month}, Gross={contribution.GrossValue}, Net={contribution.NetValue}, PaidInMonth={contribution.PaidInMonth}");
}

Console.WriteLine($"Pages parsed: {result.Diagnostics.Pages.Count}");
```

`LoadFromFile()` is the main entry point and returns both data and diagnostics.

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

When a document is parsed using `ContributionsDocument.LoadFromFile()`, both the parsed data and parsing diagnostics are returned.

### Practical Example

```csharp
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

DocumentLoadResult result = ContributionsDocument.LoadFromFile("contributions.pdf");

foreach (PageParsingDiagnostics page in result.Diagnostics.Pages)
{
	int totalRows = page.Tables.Sum(t => t.RowCount);

	Console.WriteLine(
		$"Page {page.PageIndex}: Fallback={page.UsedFallbackExtraction}, Tables={page.Tables.Count}, Rows={totalRows}");
}
```

`UsedFallbackExtraction` is true when the algorithm detecting tables in the PDF page did not succeeded from the first try and used a fall-back approach. See the code if more details are needed.

## Error Handling And Failure Modes

### Exceptions You May See

- `ArgumentNullException`: when required arguments are null in lower-level APIs.
- `ArgumentOutOfRangeException`: for invalid month/year values or invalid column index access.
- `InvalidDataException` (`DustInTheWind.NN.Toolkit.MandatoryPrivatePension.InvalidDataException`): when a table cell cannot be parsed to the expected type (date/decimal). The exception message includes the raw value and the full row.
- `InvalidOperationException`: if PDF row enumeration is attempted before opening a PDF document in internal flow.
- Exceptions from the underlying PDF pipeline (for example file access or malformed PDF scenarios).

### Recommended Consumer Pattern

Wrap parsing in `try/catch`, then inspect diagnostics when parsing succeeds:

```csharp
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

try
{
	DocumentLoadResult result = ContributionsDocument.LoadFromFile("contributions.pdf");

	// Use result.Document
	// Inspect result.Diagnostics
}
catch (InvalidDataException ex)
{
	Console.Error.WriteLine($"Could not parse contribution data: {ex.Message}");
}
catch (Exception ex)
{
	Console.Error.WriteLine($"Parsing failed: {ex.Message}");
}
```

## End-To-End Example: Export To CSV

The repository includes a sample CLI project in `sources/NN.Toolkit.Cli` that demonstrates:

- reading `contributions.pdf`
- writing CSV files (`NN_transactions.csv`, `NN_cash_transactions.csv`)
- printing parsed data and diagnostics.

You can use this project as a reference implementation for your own importer/exporter tools.
