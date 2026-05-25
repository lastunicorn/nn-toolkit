using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ConstructorTests
{
	[Fact]
	public void Constructor_SavesValues_WhenInputIsValid()
	{
		MonthDate monthDate = new(2024, 5);

		Assert.Equal(2024, monthDate.Year);
		Assert.Equal(5, monthDate.Month);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void Constructor_Throws_WhenYearIsInvalid(int year)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new MonthDate(year, 1));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(13)]
	[InlineData(-1)]
	public void Constructor_Throws_WhenMonthIsInvalid(int month)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new MonthDate(2024, month));
	}
}