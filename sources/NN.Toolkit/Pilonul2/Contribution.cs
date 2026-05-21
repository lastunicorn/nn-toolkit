using System.Globalization;

namespace DustInTheWind.NN.Toolkit.Pilonul2;

public class Contribution
{
    public MonthDate Month { get; init; }

    public decimal GrossValue { get; init; }

    public decimal AdministrationFee { get; init; }

    public decimal NetValue { get; init; }

    public decimal UnitValue { get; init; }

    public decimal UnitCount { get; init; }

    public MonthDate PaidInMonth { get; init; }

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