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
		// NN API uses Unix time in milliseconds.
		// Date range:
		//		- Start date range is NOT inclusive. We subtract 1 day to make it inclusive.
		//		- End date range is inclusive.
		
		DateTime startDateTime = StartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
			.AddDays(-1);
		DateTimeOffset startDateTimeOffset = new(startDateTime);
		long startUnixMilliseconds = startDateTimeOffset.ToUnixTimeMilliseconds();

		DateTime endDateTime = EndDate.ToDateTime(TimeOnly.MinValue);
		DateTimeOffset endDateTimeOffset = new(endDateTime, TimeSpan.Zero);
		long endUnixMilliseconds = endDateTimeOffset.ToUnixTimeMilliseconds();

		GraphRequestBody graphRequestBody = new()
		{
			Bl = "2",
			NumberOfPoints = Count,
			Currency = "LEI",
			DateRangeFrom = startUnixMilliseconds,
			DateRangeTo = endUnixMilliseconds
		};

		string requestBody = JsonSerializer.Serialize(graphRequestBody, JsonSerializerOptions);

		return new HttpRequestMessage(HttpMethod.Post, "graph/post")
		{
			Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
		};
	}
}