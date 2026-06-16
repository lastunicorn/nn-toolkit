using System.Text.Json;

namespace DustInTheWind.NN.Toolkit.ApiAccess;

public sealed class NnApiClient : IDisposable, INnApiClient
{
	private readonly HttpClient http;

	public NnApiClient()
	{
		http = new HttpClient
		{
			BaseAddress = new Uri("https://www.nn.ro/api/")
		};
	}

	public async Task<GraphData> GetGraph(DateOnly startDate, DateOnly endDate, int count)
	{
		GetGraphRequest request = new()
		{
			StartDate = startDate,
			EndDate = endDate,
			Count = count
		};

		using HttpRequestMessage httpRequestMessage = request.ToHttpRequestMessage();
		HttpResponseMessage httpResponseMessage = await http.SendAsync(httpRequestMessage);

		httpResponseMessage.EnsureSuccessStatusCode();

		GetGraphResponse response = new(httpResponseMessage);
		return await response.ToGraphData();
	}

	public void Dispose()
	{
		http?.Dispose();
	}
}