namespace DustInTheWind.NN.Toolkit.Demo;

/// <summary>
/// Fund Net Asset Value
/// </summary>
public record FundNav
{
	public DateTime Date { get; set; }

	public decimal Value { get; set; }
}