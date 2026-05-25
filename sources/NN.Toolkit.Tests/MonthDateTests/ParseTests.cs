using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ParseTests
{
	[Fact]
	public void Parse_ExtractsMonthAndYear()
	{
		MonthDate monthDate = MonthDate.Parse("11/2025");

		Assert.Equal(2025, monthDate.Year);
		Assert.Equal(11, monthDate.Month);
	}
}