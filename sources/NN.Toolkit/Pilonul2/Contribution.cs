namespace DustInTheWind.NN.Toolkit.Pilonul2;

public class Contribution
{
    public MonthDate Month { get; set; }

    public decimal GrossValue { get; set; }

    public decimal AdministrationFee { get; set; }

    public decimal NetValue { get; set; }

    public decimal UnitValue { get; set; }

    public decimal UnitCount { get; set; }

    public MonthDate PaidInMonth { get; set; }
}
