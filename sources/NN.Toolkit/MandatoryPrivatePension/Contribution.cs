using System.Globalization;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

public class Contribution
{
	public MonthDate Month { get; set; }

	public decimal GrossValue { get; set; }

	public decimal AdministrationFee { get; set; }

	public decimal NetValue { get; set; }

	public decimal UnitValue { get; set; }

	public decimal UnitCount { get; set; }

	public MonthDate PaidInMonth { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is not Contribution other)
			return false;

		return Month == other.Month
			&& GrossValue == other.GrossValue
			&& AdministrationFee == other.AdministrationFee
			&& NetValue == other.NetValue
			&& UnitValue == other.UnitValue
			&& UnitCount == other.UnitCount
			&& PaidInMonth == other.PaidInMonth;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Month, GrossValue, AdministrationFee, NetValue, UnitValue, UnitCount, PaidInMonth);
	}

	public string[] ToStringArray()
	{
		return
		[
			Month.ToString(),
			GrossValue.ToString(CultureInfo.InvariantCulture),
			AdministrationFee.ToString(CultureInfo.InvariantCulture),
			NetValue.ToString(CultureInfo.InvariantCulture),
			UnitValue.ToString(CultureInfo.InvariantCulture),
			UnitCount.ToString(CultureInfo.InvariantCulture),
			PaidInMonth.ToString()
		];
	}
}