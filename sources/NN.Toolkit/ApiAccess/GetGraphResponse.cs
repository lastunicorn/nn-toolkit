using System.Text.Json;

namespace DustInTheWind.NN.Toolkit.ApiAccess;

internal class GetGraphResponse
{
	private readonly HttpResponseMessage httpResponseMessage;

	public GetGraphResponse(HttpResponseMessage httpResponseMessage)
	{
		this.httpResponseMessage = httpResponseMessage;
	}

	public async Task<GraphData> ToGraphData()
	{
		string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

		using JsonDocument envelope = JsonDocument.Parse(responseString);

		string json = envelope.RootElement.GetString();
		using JsonDocument doc = JsonDocument.Parse(json!);

		if (doc.RootElement.ValueKind != JsonValueKind.Array)
			throw new InvalidOperationException("Expected an array as root element.");

		GraphData graphData = new();

		if (doc.RootElement.GetArrayLength() > 0)
		{
			bool nameElementExists = doc.RootElement[0].TryGetProperty("name", out JsonElement nameElement);
			if (nameElementExists)
				graphData.Name = nameElement.GetString();

			bool dataPropertyExists = doc.RootElement[0].TryGetProperty("data", out JsonElement dataElement);
			if (dataPropertyExists)
			{
				IEnumerable<NnGraphPoint> points = dataElement.EnumerateArray()
					.Select(x =>
					{
						long dateMilliseconds = x[0].GetInt64();
						DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(dateMilliseconds).DateTime;

						decimal value = x[1].GetDecimal();

						return new NnGraphPoint
						{
							Date = dateTime,
							Value = value
						};
					});

				foreach (NnGraphPoint point in points)
					graphData.Points.Add(point);
			}
		}

		return graphData;
	}
}