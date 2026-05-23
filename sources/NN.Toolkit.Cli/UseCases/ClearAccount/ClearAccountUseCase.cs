using DustInTheWind.NN.Toolkit.Cli.DataAccess;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ClearAccount;

internal class ClearAccountUseCase : IUseCase
{
    private readonly IUnitOfWork unitOfWork;

    public ClearAccountUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void Execute()
    {
        unitOfWork.ContributionRepository.Clear();
        unitOfWork.SaveChanges();

        Console.WriteLine("All contributions have been cleared from the database.");
    }
}