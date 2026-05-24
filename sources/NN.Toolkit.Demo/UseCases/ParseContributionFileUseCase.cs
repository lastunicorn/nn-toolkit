using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.Demo.UseCases;

/// <summary>
/// To run this use case, you need to have the PDF file "contributions.pdf"
/// in the same directory as the executable.
/// It is available from the NN Direct mobile application.
/// </summary>
internal class ParseContributionFileUseCase
{
	public Task Execute()
	{
		const string filePath = "contributions.pdf";

		DocumentLoadResult documentLoadResult = ParseDocument(filePath);
		DisplayParsingDiagnostics(documentLoadResult.Diagnostics);
		DisplayContributions(documentLoadResult.Document);

		return Task.CompletedTask;
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

	private void DisplayContributions(IEnumerable<Contribution> contributions)
	{
		Console.WriteLine("Contributions:");

		DataGrid dataGrid = new()
		{
			Margin = "0 1 0 1"
		};

		dataGrid.Columns.Add("Month", HorizontalAlignment.Center);
		dataGrid.Columns.Add("Gross Value", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Administration Fee", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Net Value", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Unit Value", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Unit Count", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Paid in Month", HorizontalAlignment.Center);

		foreach (Contribution contribution in contributions)
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
}