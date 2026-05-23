namespace DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

internal interface IUnitOfWork
{
    ContributionRepository ContributionRepository { get; }

    FundNavRepository FundNavRepository { get; }

    void SaveChanges();
}