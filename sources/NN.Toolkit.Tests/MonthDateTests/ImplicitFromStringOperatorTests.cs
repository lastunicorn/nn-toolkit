using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class ImplicitFromStringOperatorTests
{
	[Fact]
	public void ImplicitMonthDateConversion_ParsesInputValue()
	{
		MonthDate monthDate = "09/2023";

		Assert.Equal(2023, monthDate.Year);
		Assert.Equal(9, monthDate.Month);
	}
}