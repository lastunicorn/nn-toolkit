using DustInTheWind.NN.Toolkit.Cli.Domain;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

public class Database
{
	public List<Contribution> Contributions { get; } = [];

	public List<FundNav> FundRecords { get; } = [];

	public async Task OpenAsync()
	{
		ContributionPersister contributionPersister = new();
		Contributions.AddRange(await contributionPersister.LoadAsync());

		FundRecordPersister fundRecordPersister = new();
		FundRecords.AddRange(await fundRecordPersister.LoadAsync());
	}

	public async Task SaveAllAsync()
	{
		ContributionPersister contributionPersister = new();
		await contributionPersister.SaveAsync(Contributions);

		FundRecordPersister fundRecordPersister = new();
		await fundRecordPersister.SaveAsync(FundRecords);
	}
}