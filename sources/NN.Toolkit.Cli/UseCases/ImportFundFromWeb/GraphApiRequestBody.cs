using System.Text.Json.Serialization;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromWeb;

internal class GraphApiRequestBody
{
	[JsonPropertyName("bl")]
	public string Bl { get; set; }

	[JsonPropertyName("numberOfPoints")]
	public int NumberOfPoints { get; set; }

	[JsonPropertyName("currency")]
	public string Currency { get; set; }

	[JsonPropertyName("dateRangeFrom")]
	public long DateRangeFrom { get; set; }

	[JsonPropertyName("dateRangeTo")]
	public long DateRangeTo { get; set; }
}