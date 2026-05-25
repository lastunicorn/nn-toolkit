using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ToStringTests
{
	[Fact]
	public void ToString_ReturnsMonthAndYearInExpectedFormat()
	{
		MonthDate monthDate = new(2024, 2);

		string text = monthDate.ToString();

		Assert.Equal("02/2024", text);
	}
}