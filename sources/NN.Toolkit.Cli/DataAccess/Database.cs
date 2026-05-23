using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class Database
{
    public List<Contribution> Contributions { get; } = [];

    public Database()
    {
        ContributionPersister contributionPersister = new();
        Contributions.AddRange(contributionPersister.Load());
    }

    public void SaveAll()
    {
        ContributionPersister contributionPersister = new();
        contributionPersister.Save(Contributions);
    }
}