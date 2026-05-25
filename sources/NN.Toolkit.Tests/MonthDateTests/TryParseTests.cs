using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class TryParseTests
{
	[Theory]
	[InlineData("01/0001", 1, 1)]
	[InlineData("11/2025", 11, 2025)]
	public void TryParse_ReturnsTrue_AndExtractsMonthAndYear_WhenInputIsValid(string text, int expectedMonth, int expectedYear)
	{
		bool success = MonthDate.TryParse(text, out MonthDate monthDate);

		Assert.True(success);
		Assert.Equal(expectedYear, monthDate.Year);
		Assert.Equal(expectedMonth, monthDate.Month);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("112025")]
	[InlineData("AA/2025")]
	[InlineData("11/BBBB")]
	public void TryParse_ReturnsFalse_WhenInputIsInvalid(string text)
	{
		bool success = MonthDate.TryParse(text!, out MonthDate monthDate);

		Assert.False(success);
		Assert.Equal(default, monthDate);
	}
}

