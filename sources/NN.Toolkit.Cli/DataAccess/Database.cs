using DustInTheWind.NN.Toolkit.Cli.Domain;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class Database
{
    public List<Contribution> Contributions { get; } = [];

    public List<FundRecord> FundRecords { get; } = [];

    public Database()
    {
        ContributionPersister contributionPersister = new();
        Contributions.AddRange(contributionPersister.Load());

        FundRecordPersister fundRecordPersister = new();
        FundRecords.AddRange(fundRecordPersister.Load());
    }

    public void SaveAll()
    {
        ContributionPersister contributionPersister = new();
        contributionPersister.Save(Contributions);

        FundRecordPersister fundRecordPersister = new();
        fundRecordPersister.Save(FundRecords);
    }
}