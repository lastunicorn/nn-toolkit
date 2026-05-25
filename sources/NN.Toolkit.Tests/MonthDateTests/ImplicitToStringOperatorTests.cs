using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ImplicitToStringOperatorTests
{
	[Fact]
	public void ImplicitStringConversion_ReturnsFormattedValue()
	{
		MonthDate monthDate = new(2024, 7);

		string text = monthDate;

		Assert.Equal("07/2024", text);
	}
}