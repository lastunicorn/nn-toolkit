namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

internal interface IUnitOfWork
{
    ContributionRepository ContributionRepository { get; }
    
    void SaveChanges();
}