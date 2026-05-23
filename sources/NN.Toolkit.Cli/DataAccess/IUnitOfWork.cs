namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

internal interface IUnitOfWork
{
    ContributionRepository ContributionRepository { get; }

    FundNavRepository FundNavRepository { get; }

    void SaveChanges();
}