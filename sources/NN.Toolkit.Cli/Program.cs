using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.Export;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Cli;

internal static class Program
{
    internal static void Main(string[] args)
    {
        string inputPdfPath = args.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(inputPdfPath))
            inputPdfPath = "contributions.pdf";

        DocumentLoadResult documentLoadResult = ContributionsDocument.LoadFromFile(inputPdfPath);

        DisplayDocument(documentLoadResult.Document);
        DisplayParsingDiagnostics(documentLoadResult.Diagnostics);
        
        ExportToCsv(documentLoadResult.Document);
    }

    private static void DisplayDocument(ContributionsDocument document)
    {
        DataGrid dataGrid = new();

        dataGrid.Columns.Add("Month");
        dataGrid.Columns.Add("Gross Value");
        dataGrid.Columns.Add("Administration Fee");
        dataGrid.Columns.Add("Net Value");
        dataGrid.Columns.Add("Unit Value");
        dataGrid.Columns.Add("Unit Count");
        dataGrid.Columns.Add("Paid in Month");

        foreach (Contribution contribution in document)
        {
            dataGrid.Rows.Add(
                contribution.Month,
                contribution.GrossValue,
                contribution.AdministrationFee,
                contribution.NetValue,
                contribution.UnitValue,
                contribution.UnitCount,
                contribution.PaidInMonth);
        }

        dataGrid.Display();
    }

    private static void DisplayParsingDiagnostics(DocumentParsingDiagnostics diagnostics)
    {
        DataGrid diagnosticsGrid = new();

        diagnosticsGrid.Columns.Add($"Pages ({diagnostics.Pages.Count})");
        diagnosticsGrid.Columns.Add("Use Fallback");
        diagnosticsGrid.Columns.Add("Table Count", HorizontalAlignment.Right);
        diagnosticsGrid.Columns.Add("Row Count", HorizontalAlignment.Right);

        foreach (PageParsingDiagnostics page in diagnostics.Pages)
        {
            string pageNumber = $"Page {page.PageIndex}";
            YesNoBool usedFallbackExtraction = page.UsedFallbackExtraction;
            int tableCount = page.Tables.Count;
            int rowCount = page.Tables
                .Select(x => x.RowCount)
                .Sum();

            diagnosticsGrid.Rows.Add(pageNumber, usedFallbackExtraction, tableCount, rowCount);
        }

        diagnosticsGrid.Display();
    }

    private static void ExportToCsv(ContributionsDocument document)
    {
        using NnTransactionsFile nnTransactionsFile = new("NN_transactions.csv");
        using NnCashTransactionsFile nnCashTransactionsFile = new("NN_cash_transactions.csv");

        foreach (Contribution contribution in document)
        {
            nnTransactionsFile.Write(contribution, document.Header);
            nnCashTransactionsFile.Write(contribution);
        }

        Console.WriteLine();
        Console.WriteLine("Pdf was exported to CSV files:");
        Console.WriteLine("  - NN_transactions.csv");
        Console.WriteLine("  - NN_cash_transactions.csv");
    }
}