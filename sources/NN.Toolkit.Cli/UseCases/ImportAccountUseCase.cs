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

        DocumentLoadResult documentLoadResult = ParseDocument(filePath);
        DisplayParsingDiagnostics(documentLoadResult.Diagnostics);

        Import(documentLoadResult.Document);
    }

    private DocumentLoadResult ParseDocument(string filePath)
    {
        Console.WriteLine($"Parsing document '{filePath}'");

        DocumentLoadResult documentLoadResult = ContributionsDocument.LoadFromFile(filePath);

        foreach (Contribution contribution in documentLoadResult.Document)
            contributionRepository.Add(contribution);

        return documentLoadResult;
    }

    private static void DisplayParsingDiagnostics(DocumentParsingDiagnostics diagnostics)
    {
        DataGrid diagnosticsGrid = new()
        {
            Margin = new Thickness(0, 1, 0, 1)
        };

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

        int totalRowCount = diagnostics.Pages
            .SelectMany(x => x.Tables)
            .Select(x => x.RowCount)
            .Sum();
        diagnosticsGrid.Footer = "Row Count: " + totalRowCount;

        diagnosticsGrid.Display();
    }

    private void Import(ContributionsDocument contributionsDocument)
    {
        Console.WriteLine($"Importing {contributionsDocument.Count} contributions into database.");

        ImportDiagnostics importDiagnostics = new();

        foreach (Contribution contribution in contributionsDocument)
        {
            Contribution existingContribution = contributionRepository.Get(contribution.Month);

            if (existingContribution == null)
            {
                contributionRepository.Add(contribution);
                importDiagnostics.AddCount++;
            }
            else
            {
                if (existingContribution.Equals(contribution))
                {
                    importDiagnostics.SkipCount++;
                }
                else
                {
                    existingContribution.GrossValue = contribution.GrossValue;
                    existingContribution.AdministrationFee = contribution.AdministrationFee;
                    existingContribution.NetValue = contribution.NetValue;
                    existingContribution.UnitValue = contribution.UnitValue;
                    existingContribution.UnitCount = contribution.UnitCount;
                    existingContribution.PaidInMonth = contribution.PaidInMonth;

                    importDiagnostics.UpdateCount++;
                }
            }
        }

        contributionRepository.SaveChanges();

        Console.WriteLine();
        Console.WriteLine("Data imported successfully.");
    }
}

internal class ImportDiagnostics
{
    public int AddCount { get; set; }

    public int UpdateCount { get; set; }

    public int SkipCount { get; set; }
}