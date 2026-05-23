using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

internal static class Database
{
    public static List<Contribution> Contributions { get; } = [];

    static Database()
    {
        Contributions.AddRange([
            new Contribution
            {
                Month = new MonthDate(2024, 1),
                GrossValue = 1000m,
                AdministrationFee = 50m,
                NetValue = 950m,
                UnitValue = 10m,
                UnitCount = 95m,
                PaidInMonth = new MonthDate(2024, 1)
            },
            new Contribution
            {
                Month = new MonthDate(2024, 2),
                GrossValue = 1000m,
                AdministrationFee = 50m,
                NetValue = 950m,
                UnitValue = 10m,
                UnitCount = 95m,
                PaidInMonth = new MonthDate(2024, 2)
            }
        ]);
    }
}