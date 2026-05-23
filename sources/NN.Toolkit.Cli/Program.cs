using DustInTheWind.ConsoleTools.Arguments;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Export;
using DustInTheWind.NN.Toolkit.Cli.UseCases;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ExportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportAccount;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Cli;

internal static class Program
{
    // account import "contributions.pdf"
    // account import --file "contributions.pdf"
    // account clear
    // account export --format pp
    // account

    // fund import --from 2026-01-01 --to 2026-12-31
    // fund import --year 2026
    // fund import --file "historical_2008.csv"
    // fund clear
    // fund

    // help

    internal static void Main(string[] args)
    {
        Arguments arguments = new(args);

        IUseCase useCase = CreateUseCase(arguments) ?? new HelpUseCase();
        useCase.Execute();

        // if (string.IsNullOrWhiteSpace(noun))
        //     noun = "contributions.pdf";
        //
        // DocumentLoadResult documentLoadResult = ContributionsDocument.LoadFromFile(noun);
        //
        // DisplayDocument(documentLoadResult.Document);
        // DisplayParsingDiagnostics(documentLoadResult.Diagnostics);
        //
        // ExportToCsv(documentLoadResult.Document);
    }

    private static IUseCase CreateUseCase(Arguments arguments)
    {
        if (arguments.Count == 0)
            return null;

        Argument noun = arguments[0];
        if (noun?.Type != ArgumentType.Ordinal)
            return null;

        switch (noun.Value)
        {
            case "help":
                return new HelpUseCase();

            case "account":
                Argument verb = arguments[1];
                if (verb?.Type == ArgumentType.Ordinal)
                    switch (verb.Value)
                    {
                        case "import":
                        {
                            Argument file = arguments["file"];
                            return new ImportAccountUseCase(file?.Value, new ContributionRepository());
                        }

                        case "export":
                        {
                            Argument format = arguments["format"];
                            return new ExportAccountUseCase(format?.Value, new ContributionRepository());
                        }
                    }

                return new ShowAccountUseCase();

            case "fund":
                return new ShowFundUseCase();

            default:
                throw new Exception("Unknown command " + noun);
        }
    }

    // private static void DisplayDocument(ContributionsDocument document)
    // {
    //     DataGrid dataGrid = new();
    //
    //     dataGrid.Columns.Add("Month", HorizontalAlignment.Center);
    //     dataGrid.Columns.Add("Gross Value", HorizontalAlignment.Right);
    //     dataGrid.Columns.Add("Administration Fee", HorizontalAlignment.Right);
    //     dataGrid.Columns.Add("Net Value", HorizontalAlignment.Right);
    //     dataGrid.Columns.Add("Unit Value", HorizontalAlignment.Right);
    //     dataGrid.Columns.Add("Unit Count", HorizontalAlignment.Right);
    //     dataGrid.Columns.Add("Paid in Month", HorizontalAlignment.Center);
    //
    //     foreach (Contribution contribution in document)
    //     {
    //         dataGrid.Rows.Add(
    //             contribution.Month,
    //             contribution.GrossValue,
    //             contribution.AdministrationFee,
    //             contribution.NetValue,
    //             contribution.UnitValue,
    //             contribution.UnitCount,
    //             contribution.PaidInMonth);
    //     }
    //
    //     dataGrid.Display();
    // }

    // private static void DisplayParsingDiagnostics(DocumentParsingDiagnostics diagnostics)
    // {
    //     DataGrid diagnosticsGrid = new();
    //
    //     diagnosticsGrid.Columns.Add($"Pages ({diagnostics.Pages.Count})");
    //     diagnosticsGrid.Columns.Add("Use Fallback");
    //     diagnosticsGrid.Columns.Add("Table Count", HorizontalAlignment.Right);
    //     diagnosticsGrid.Columns.Add("Row Count", HorizontalAlignment.Right);
    //
    //     foreach (PageParsingDiagnostics page in diagnostics.Pages)
    //     {
    //         string pageNumber = $"Page {page.PageIndex}";
    //         YesNoBool usedFallbackExtraction = page.UsedFallbackExtraction;
    //         int tableCount = page.Tables.Count;
    //         int rowCount = page.Tables
    //             .Select(x => x.RowCount)
    //             .Sum();
    //
    //         diagnosticsGrid.Rows.Add(pageNumber, usedFallbackExtraction, tableCount, rowCount);
    //     }
    //
    //     diagnosticsGrid.Display();
    // }
}