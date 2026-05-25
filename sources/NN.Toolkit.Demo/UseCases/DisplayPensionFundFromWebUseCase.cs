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
		IEnumerable<FundNav> fundNavs = await ReadFromNnApi();
		DisplayFundRecords(fundNavs);
	}

	private async Task<IEnumerable<FundNav>> ReadFromNnApi()
	{
		const int year = 2008;
		
		DateOnly fromDate = new(year, 1, 1);
		DateOnly toDate = new(year, 12, 31);

		int numberOfPoints = toDate.DayNumber - fromDate.DayNumber + 1;

		Console.WriteLine($"Reading fund NAV values from {fromDate} to {toDate} ({numberOfPoints} points)");

		NnApiClient nnApiClient = new();
		GraphData graphData = await nnApiClient.GetGraph(fromDate, toDate, numberOfPoints);

		return graphData.Points
			.Select(x => new FundNav
			{
				Date = x.Date,
				Value = x.Value
			})
			.ToList();
	}

	private void DisplayFundRecords(IEnumerable<FundNav> fundRecords)
	{
		DataGrid dataGrid = new()
		{
			Margin = "0 1 0 1"
		};

		dataGrid.Columns.Add("Date", HorizontalAlignment.Center);
		dataGrid.Columns.Add("Value", HorizontalAlignment.Right);

		int count = 0;
		
		foreach (FundNav fundRecord in fundRecords)
		{
			dataGrid.Rows.Add(
				fundRecord.Date,
				fundRecord.Value);
			
			count++;
		}
		
		dataGrid.Footer = $"Total records: {count}";

		dataGrid.Display();
	}
}