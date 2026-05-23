using System.Text;
using System.Text.Json;

namespace DustInTheWind.NN.Toolkit.Cli.Ports.NnAccess;

internal sealed class NnApiClient : IDisposable, INnApiClient
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		WriteIndented = true
	};

	private readonly HttpClient http;

	public NnApiClient()
	{
		http = new HttpClient();
	}

	public async Task<IEnumerable<NnGraphValue>> GetGraph(DateOnly startDate, DateOnly endDate, int count)
	{
		DateTimeOffset startOffset = new(startDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		long startMilliseconds = startOffset.ToUnixTimeMilliseconds();

		DateTimeOffset endOffset = new(endDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		endOffset = endOffset.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerMillisecond);
		long endMilliseconds = endOffset.ToUnixTimeMilliseconds();

		GraphApiRequestBody graphApiRequestBody = new()
		{
			Bl = "2",
			NumberOfPoints = count,
			Currency = "LEI",
			DateRangeFrom = startMilliseconds,
			DateRangeTo = endMilliseconds
		};

		string requestBody = JsonSerializer.Serialize(graphApiRequestBody, JsonSerializerOptions);
		using StringContent content = new(requestBody, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await http.PostAsync("https://www.nn.ro/api/graph/post", content);
		response.EnsureSuccessStatusCode();

		string json = await response.Content.ReadAsStringAsync();
		using JsonDocument envelope = JsonDocument.Parse(json);
		using JsonDocument doc = JsonDocument.Parse(envelope.RootElement.GetString()!);

		List<NnGraphValue> fundNavs = [];

		foreach (JsonElement item in doc.RootElement[0].GetProperty("data").EnumerateArray())
		{
			string date = DateTimeOffset.FromUnixTimeMilliseconds(item[0].GetInt64()).ToString("yyyy-MM-dd");
			string value = item[1].GetRawText();

			NnGraphValue nnGraphValue = new()
			{
				Date = DateOnly.Parse(date),
				Value = decimal.Parse(value)
			};

			fundNavs.Add(nnGraphValue);
		}

		return fundNavs;
	}

	public void Dispose()
	{
		http?.Dispose();
	}
}