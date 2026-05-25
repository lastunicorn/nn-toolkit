using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ParseTests
{
	[Theory]
	[InlineData("01/0001", 1, 1)]
	[InlineData("12/2025", 12, 2025)]
	public void Parse_ExtractsMonthAndYear_ForBoundaryLikeValues(string text, int expectedMonth, int expectedYear)
	{
		MonthDate monthDate = MonthDate.Parse(text);

		Assert.Equal(expectedYear, monthDate.Year);
		Assert.Equal(expectedMonth, monthDate.Month);
	}

	[Fact]
	public void Parse_ExtractsMonthAndYear()
	{
		MonthDate monthDate = MonthDate.Parse("11/2025");

		Assert.Equal(2025, monthDate.Year);
		Assert.Equal(11, monthDate.Month);
	}

	[Theory]
	[InlineData("AA/2025")]
	[InlineData("11/BBBB")]
	public void Parse_ThrowsFormatException_WhenInputContainsNonNumericValues(string text)
	{
		Assert.Throws<FormatException>(() => MonthDate.Parse(text));
	}

	[Fact]
	public void Parse_ThrowsFormatException_WhenInputDoesNotMatchAnySupportedFormat()
	{
		Assert.Throws<FormatException>(() => MonthDate.Parse("112025"));
	}

	[Fact]
	public void Parse_ThrowsArgumentNullException_WhenInputIsNull()
	{
		Assert.Throws<ArgumentNullException>(() => MonthDate.Parse(null!));
	}
}