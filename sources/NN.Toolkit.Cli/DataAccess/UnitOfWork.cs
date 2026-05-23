namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

internal class UnitOfWork: IUnitOfWork
{
    private readonly Database database;
    private ContributionRepository contributionRepository;
    private FundRecordRepository fundRecordRepository;
    
    public ContributionRepository ContributionRepository => contributionRepository ??= new ContributionRepository(database);

    public FundRecordRepository FundRecordRepository => fundRecordRepository ??= new FundRecordRepository(database);

    public UnitOfWork(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public void SaveChanges()
    {
        database.SaveAll();
    }
}