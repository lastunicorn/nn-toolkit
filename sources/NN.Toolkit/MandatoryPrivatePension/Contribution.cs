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