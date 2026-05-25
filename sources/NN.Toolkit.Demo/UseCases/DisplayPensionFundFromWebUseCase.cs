using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.ApiAccess;

namespace DustInTheWind.NN.Toolkit.Demo.UseCases;

/// <summary>
/// This use case is retrieving historical data from the NN API for the
/// Romanian Mandatory Pension Fund.
/// </summary>
internal class DisplayPensionFundFromWebUseCase
{
	public async Task Execute()
	{
		GraphData graphData = await ReadFromNnApi();
		DisplayFundRecords(graphData);
	}

	private async Task<GraphData> ReadFromNnApi()
	{
		const int year = 2007;

		DateOnly fromDate = new(year, 1, 1);
		DateOnly toDate = new(year, 12, 31);

		int numberOfPoints = toDate.DayNumber - fromDate.DayNumber + 1;

		Console.WriteLine("Using NN API to read historical data from the Romanian Mandatory Pension Fund.");
		Console.WriteLine($"From {fromDate} to {toDate} ({numberOfPoints} points)");

		NnApiClient nnApiClient = new();
		return await nnApiClient.GetGraph(fromDate, toDate, numberOfPoints);
	}

	private void DisplayFundRecords(GraphData graphData)
	{
		DataGrid dataGrid = new()
		{
			Margin = "0 1 0 1",
			Title = graphData.Name
		};

		dataGrid.Columns.Add("Date", HorizontalAlignment.Center);
		dataGrid.Columns.Add("Value", HorizontalAlignment.Right);

		int count = 0;

		foreach (NnGraphPoint nnGraphPoint in graphData.Points)
		{
			dataGrid.Rows.Add(
				nnGraphPoint.Date,
				nnGraphPoint.Value);

			count++;
		}

		dataGrid.Footer = $"Total records: {count}";

		dataGrid.Display();
	}
}