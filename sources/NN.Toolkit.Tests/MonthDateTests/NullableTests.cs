using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Tests.MonthDateTests;

public class NullableTests
{
	[Fact]
	public void NullableMonthDate_ThrowsArgumentNullException_WhenNullIsCastToMonthDate()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			MonthDate? _ = (MonthDate)null;
		});
	}

	[Fact]
	public void NullableMonthDate_HasNoValue_WhenAssignedNull()
	{
		MonthDate? monthDate = null;

		Assert.False(monthDate.HasValue);
	}

	[Fact]
	public void NullableMonthDate_HasValue_WhenAssignedConcreteMonthDate()
	{
		MonthDate? monthDate = new MonthDate(2024, 6);

		Assert.True(monthDate.HasValue);
		Assert.Equal(2024, monthDate.Value.Year);
		Assert.Equal(6, monthDate.Value.Month);
	}

	[Fact]
	public void NullableMonthDate_ToString_ReturnsEmptyString_WhenNull()
	{
		MonthDate? monthDate = null;

		string text = monthDate.ToString();

		Assert.Equal(string.Empty, text);
	}

	[Fact]
	public void NullableMonthDate_ToString_ReturnsFormattedValue_WhenHasValue()
	{
		MonthDate? monthDate = new MonthDate(2024, 6);

		string text = monthDate.ToString();

		Assert.Equal("06/2024", text);
	}

	[Fact]
	public void NullableMonthDate_CanBeAssignedFromStringImplicitly_WhenInputIsValid()
	{
		MonthDate? monthDate = "03/2022";

		Assert.True(monthDate.HasValue);
		Assert.Equal(2022, monthDate.Value.Year);
		Assert.Equal(3, monthDate.Value.Month);
	}
}