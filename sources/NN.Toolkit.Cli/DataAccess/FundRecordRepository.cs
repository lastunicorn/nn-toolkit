using DustInTheWind.NN.Toolkit.Cli.Domain;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class FundRecordRepository
{
    private readonly Database database;

    public FundRecordRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public IEnumerable<FundRecord> GetAll()
    {
        return database.FundRecords;
    }

    public void Add(FundRecord fundRecord)
    {
        if (fundRecord == null) throw new ArgumentNullException(nameof(fundRecord));

        database.FundRecords.Add(fundRecord);
    }

    public void Clear()
    {
        database.FundRecords.Clear();
    }
}


