using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Cli;

internal static class Program
{
    internal static void Main(string[] args)
    {
        DocumentLoadResult documentLoadResult = ContributionsDocument.LoadFromFile("contributions.pdf");

        Export(documentLoadResult.Document);
        DisplayData(documentLoadResult.Document);
        DisplayStatistics(documentLoadResult.Diagnostics);
    }

    private static void Export(ContributionsDocument document)
    {
        using StreamWriter output = new("NN_transactions.csv");
        output.WriteLine("Security Name,Ticker Symbol,Date,Time,Value,Shares,Type,Fees,Note");

        using StreamWriter cashOutput = new("NN_cash_transactions.csv");
        cashOutput.WriteLine("Type,Cash Account,Date,Time,Value,Note");

        foreach (Contribution contribution in document)
        {
            string date = $"{contribution.PaidInMonth.Year:00}-{contribution.PaidInMonth.Month:00}-01";
            string note = string.Join("; ", document.Header.Zip(contribution.ToStringArray(), (h, v) => $"{h}={v}"));

            output.WriteLine($"NN,NN,{date},08:05,{contribution.GrossValue},{contribution.UnitCount},Buy,{contribution.AdministrationFee},\"{note}\"");
            cashOutput.WriteLine($"Deposit,NN,{date},08:00,{contribution.GrossValue},\"Luna: {contribution.Month}\"");
        }
    }

    private static void DisplayData(ContributionsDocument document)
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
            dataGrid.Rows.Add(contribution.Month, contribution.GrossValue, contribution.AdministrationFee, contribution.NetValue, contribution.UnitValue, contribution.UnitCount,
                contribution.PaidInMonth);
        }

        dataGrid.Display();
    }

    private static void DisplayStatistics(DocumentParsingDiagnostics diagnostics)
    {
        DataGrid diagnosticsGrid = new();

        diagnosticsGrid.Columns.Add($"Pages ({diagnostics.Pages.Count})");
        diagnosticsGrid.Columns.Add("Table Count", HorizontalAlignment.Right);
        diagnosticsGrid.Columns.Add("Row Count", HorizontalAlignment.Right);

        foreach (PageParsingDiagnostics page in diagnostics.Pages)
        {
            int tableCount =  page.Tables.Count;
            int rowCount = page.Tables
                .Select(x => x.RowCount)
                .Sum();
            
            diagnosticsGrid.Rows.Add($"Page {page.PageIndex}", tableCount, rowCount);
        }

        diagnosticsGrid.Display();
    }
}