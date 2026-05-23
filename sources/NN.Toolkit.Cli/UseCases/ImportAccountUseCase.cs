using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases;

internal class ImportAccountUseCase : IUseCase
{
    private readonly string filePath;
    private readonly ContributionRepository contributionRepository;

    public ImportAccountUseCase(string filePath, ContributionRepository contributionRepository)
    {
        this.filePath = filePath;
        this.contributionRepository = contributionRepository ?? throw new ArgumentNullException(nameof(contributionRepository));
    }

    public void Execute()
    {
        string filePath = this.filePath ?? "contributions.pdf";

        DocumentLoadResult documentLoadResult = ContributionsDocument.LoadFromFile(filePath);

        foreach (Contribution contribution in documentLoadResult.Document)
            contributionRepository.Add(contribution);

        DisplayParsingDiagnostics(documentLoadResult.Diagnostics);
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
}