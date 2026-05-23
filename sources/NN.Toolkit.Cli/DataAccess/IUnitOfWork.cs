namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

internal interface IUnitOfWork
{
    ContributionRepository ContributionRepository { get; }

    FundRecordRepository FundRecordRepository { get; }

    void SaveChanges();
}