using System.Text;
using System.Text.Json;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.Domain;
using DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromWeb;

internal class ImportFundFromWebUseCase : IUseCase
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		WriteIndented = true
	};

	private readonly UnitOfWork unitOfWork;

	private readonly DateOnly? fromDate;
	private readonly DateOnly? toDate;
	private readonly int? year;

	public ImportFundFromWebUseCase(DateOnly? fromDate, DateOnly? toDate, int? year, UnitOfWork unitOfWork)
	{
		this.fromDate = fromDate;
		this.toDate = toDate;
		this.year = year;
		this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task Execute()
	{
		if (year == null)
			throw new ArgumentNullException(nameof(year));

		IEnumerable<FundNav> fundNavs = await ReadFromNnApi();

		ImportDiagnostics importDiagnostics = Import(fundNavs);
		DisplayImportDiagnostics($"Fund NAV values for {year}", importDiagnostics);

		await unitOfWork.SaveChangesAsync();
	}

	private async Task<IEnumerable<FundNav>> ReadFromNnApi()
	{
		long dateRangeFrom = new DateTimeOffset(year.Value, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
		long dateRangeTo = new DateTimeOffset(year.Value, 12, 31, 23, 59, 59, TimeSpan.Zero).ToUnixTimeMilliseconds();

		int numberOfPoints = DateTime.IsLeapYear(year.Value) ? 366 : 365;

		GraphApiRequestBody graphApiRequestBody = new()
		{
			Bl = "2",
			NumberOfPoints = numberOfPoints,
			Currency = "LEI",
			DateRangeFrom = dateRangeFrom,
			DateRangeTo = dateRangeTo
		};

		using HttpClient http = new HttpClient();

		string requestBody = JsonSerializer.Serialize(graphApiRequestBody, JsonSerializerOptions);
		using StringContent content = new(requestBody, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await http.PostAsync("https://www.nn.ro/api/graph/post", content);
		response.EnsureSuccessStatusCode();

		string json = await response.Content.ReadAsStringAsync();
		using JsonDocument envelope = JsonDocument.Parse(json);
		using JsonDocument doc = JsonDocument.Parse(envelope.RootElement.GetString()!);

		List<FundNav> fundNavs = [];

		foreach (JsonElement item in doc.RootElement[0].GetProperty("data").EnumerateArray())
		{
			string date = DateTimeOffset.FromUnixTimeMilliseconds(item[0].GetInt64()).ToString("yyyy-MM-dd");
			string value = item[1].GetRawText();

			FundNav fundNav = new()
			{
				Date = DateOnly.Parse(date),
				Value = decimal.Parse(value)
			};

			fundNavs.Add(fundNav);
		}

		return fundNavs;
	}

	private ImportDiagnostics Import(IEnumerable<FundNav> fundNavs)
	{
		ImportDiagnostics importDiagnostics = new();

		try
		{
			foreach (FundNav fundNav in fundNavs)
			{
				FundNav existingFundNav = unitOfWork.FundNavRepository.Get(fundNav.Date);

				if (existingFundNav == null)
				{
					unitOfWork.FundNavRepository.Add(fundNav);
					importDiagnostics.AddCount++;
				}
				else if (existingFundNav.Value == fundNav.Value)
				{
					importDiagnostics.SkipCount++;
				}
				else
				{
					existingFundNav.Value = fundNav.Value;
					importDiagnostics.UpdateCount++;
				}
			}
		}
		catch (Exception ex)
		{
			importDiagnostics.Error = ex;
		}

		return importDiagnostics;
	}

	private void DisplayImportDiagnostics(string title, ImportDiagnostics importDiagnostics)
	{
		DataGrid diagnosticsGrid = new()
		{
			Title = title,
			Margin = new Thickness(0, 1, 0, 1)
		};

		diagnosticsGrid.Columns.Add("Name", HorizontalAlignment.Left);
		diagnosticsGrid.Columns.Add("Value", HorizontalAlignment.Right);

		diagnosticsGrid.Rows.Add("Add", importDiagnostics.AddCount);
		diagnosticsGrid.Rows.Add("Update", importDiagnostics.UpdateCount);
		diagnosticsGrid.Rows.Add("Skip", importDiagnostics.SkipCount);

		diagnosticsGrid.Display();

		if (importDiagnostics.Error != null)
			CustomConsole.WriteLineError($"Error importing fund values: {importDiagnostics.Error}");
	}
}