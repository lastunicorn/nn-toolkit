using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ImportAccount;

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
        string filePathSafe = filePath ?? "contributions.pdf";

        DocumentLoadResult documentLoadResult = ParseDocument(filePathSafe);
        DisplayParsingDiagnostics(documentLoadResult.Diagnostics);

        ImportDiagnostics importDiagnostics = Import(documentLoadResult.Document);
        DisplayImportDiagnostics(importDiagnostics);
    }

    private DocumentLoadResult ParseDocument(string filePath)
    {
        Console.WriteLine($"Parsing document '{filePath}'");
        return ContributionsDocument.LoadFromFile(filePath);
    }

    private static void DisplayParsingDiagnostics(DocumentParsingDiagnostics diagnostics)
    {
        DataGrid diagnosticsGrid = new()
        {
            Margin = new Thickness(0, 1, 0, 1)
        };

        diagnosticsGrid.Columns.Add($"Pages ({diagnostics.Pages.Count})");
        diagnosticsGrid.Columns.Add("Extraction Algorithm");
        diagnosticsGrid.Columns.Add("Table Count", HorizontalAlignment.Right);
        diagnosticsGrid.Columns.Add("Row Count", HorizontalAlignment.Right);

        foreach (PageParsingDiagnostics page in diagnostics.Pages)
        {
            string pageNumber = $"Page {page.PageIndex}";
            TableExtractionApproachPretty tableExtractionApproach = page.TableExtractionApproach;
            int tableCount = page.Tables.Count;
            int rowCount = page.Tables
                .Select(x => x.RowCount)
                .Sum();

            diagnosticsGrid.Rows.Add(pageNumber, tableExtractionApproach, tableCount, rowCount);
        }

        int totalRowCount = diagnostics.Pages
            .SelectMany(x => x.Tables)
            .Select(x => x.RowCount)
            .Sum();
        diagnosticsGrid.Footer = "Row Count: " + totalRowCount;

        diagnosticsGrid.Display();
    }

    private ImportDiagnostics Import(ContributionsDocument contributionsDocument)
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

        return importDiagnostics;
    }

    private void DisplayImportDiagnostics(ImportDiagnostics importDiagnostics)
    {
        DataGrid diagnosticsGrid = new()
        {
            Margin = new Thickness(0, 1, 0, 1)
        };

        diagnosticsGrid.Columns.Add("Name", HorizontalAlignment.Left);
        diagnosticsGrid.Columns.Add("Value", HorizontalAlignment.Right);

        diagnosticsGrid.Rows.Add("Add", importDiagnostics.AddCount);
        diagnosticsGrid.Rows.Add("Update", importDiagnostics.UpdateCount);
        diagnosticsGrid.Rows.Add("Skip", importDiagnostics.SkipCount);

        diagnosticsGrid.Display();
    }
}