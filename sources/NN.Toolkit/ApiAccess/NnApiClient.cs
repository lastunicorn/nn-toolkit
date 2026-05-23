using System.Text;
using System.Text.Json;

namespace DustInTheWind.NN.Toolkit.ApiAccess;

public sealed class NnApiClient : IDisposable, INnApiClient
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

	public async Task<GraphData> GetGraph(DateOnly startDate, DateOnly endDate, int count)
	{
		DateTimeOffset startOffset = new(startDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		long startMilliseconds = startOffset.ToUnixTimeMilliseconds();

		DateTimeOffset endOffset = new(endDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		endOffset = endOffset.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerMillisecond);
		long endMilliseconds = endOffset.ToUnixTimeMilliseconds();

		GraphRequestBody graphRequestBody = new()
		{
			Bl = "2",
			NumberOfPoints = count,
			Currency = "LEI",
			DateRangeFrom = startMilliseconds,
			DateRangeTo = endMilliseconds
		};

		string requestBody = JsonSerializer.Serialize(graphRequestBody, JsonSerializerOptions);
		using StringContent content = new(requestBody, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await http.PostAsync("https://www.nn.ro/api/graph/post", content);
		response.EnsureSuccessStatusCode();

		string jsonEnvelope = await response.Content.ReadAsStringAsync();
		using JsonDocument envelope = JsonDocument.Parse(jsonEnvelope);
		
		string json = envelope.RootElement.GetString();
		using JsonDocument doc = JsonDocument.Parse(json!);
		
		GraphData graphData = new()
		{
			Name = doc.RootElement[0].GetProperty("name").GetString()
		};

		foreach (JsonElement item in doc.RootElement[0].GetProperty("data").EnumerateArray())
		{
			long dateMilliseconds = item[0].GetInt64();
			DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(dateMilliseconds).DateTime;
			
			decimal value = item[1].GetDecimal();
		
			NnGraphPoint nnGraphPoint = new()
			{
				Date = dateTime,
				Value = value
			};
		
			graphData.Points.Add(nnGraphPoint);
		}
		
		return graphData;
	}

	public void Dispose()
	{
		http?.Dispose();
	}
}