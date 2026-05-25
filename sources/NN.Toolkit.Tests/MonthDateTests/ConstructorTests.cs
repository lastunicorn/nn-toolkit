using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ConstructorTests
{
	[Theory]
	[InlineData(1, 1)]
	[InlineData(1, 12)]
	[InlineData(2024, 1)]
	[InlineData(2024, 12)]
	public void Constructor_SavesBoundaryValues_WhenInputIsValid(int year, int month)
	{
		MonthDate monthDate = new(year, month);

		Assert.Equal(year, monthDate.Year);
		Assert.Equal(month, monthDate.Month);
	}

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