using DustInTheWind.NN.Toolkit.Cli.DataAccess;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ClearAccount;

internal class ClearAccountUseCase : IUseCase
{
    private readonly ContributionRepository contributionRepository;

    public ClearAccountUseCase(ContributionRepository contributionRepository)
    {
        this.contributionRepository = contributionRepository ?? throw new ArgumentNullException(nameof(contributionRepository));
    }

    public void Execute()
    {
        contributionRepository.Clear();
        contributionRepository.SaveChanges();
        
        Console.WriteLine("All contributions have been cleared from the database.");
    }
}