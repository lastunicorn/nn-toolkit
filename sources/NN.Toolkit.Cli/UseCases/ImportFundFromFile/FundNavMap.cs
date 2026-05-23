using CsvHelper.Configuration;
using DustInTheWind.NN.Toolkit.Cli.Domain;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromFile;

internal class FundNavMap : ClassMap<FundNav>
{
    public FundNavMap()
    {
        Map(x => x.Date).Name("Date");
        Map(x => x.Value).Name("Quote");
    }
}