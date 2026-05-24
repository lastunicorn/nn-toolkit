using System.Text;
using System.Text.Json;

namespace DustInTheWind.NN.Toolkit.ApiAccess;

internal class GetGraphRequest
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		WriteIndented = true
	};

	public DateOnly StartDate { get; init; }

	public DateOnly EndDate { get; init; }

	public int Count { get; init; }

	public HttpRequestMessage ToHttpRequestMessage()
	{
		DateTimeOffset startOffset = new(StartDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		long startMilliseconds = startOffset.ToUnixTimeMilliseconds();

		DateTimeOffset endOffset = new(EndDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
		endOffset = endOffset.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerMillisecond);
		long endMilliseconds = endOffset.ToUnixTimeMilliseconds();

		GraphRequestBody graphRequestBody = new()
		{
			Bl = "2",
			NumberOfPoints = Count,
			Currency = "LEI",
			DateRangeFrom = startMilliseconds,
			DateRangeTo = endMilliseconds
		};

		string requestBody = JsonSerializer.Serialize(graphRequestBody, JsonSerializerOptions);

		return new HttpRequestMessage(HttpMethod.Post, "graph/post")
		{
			Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
		};
	}
}