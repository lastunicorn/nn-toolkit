using DustInTheWind.NN.Toolkit.Cli.DataAccess;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ClearFund;

internal class ClearFundUseCase : IUseCase
{
    private readonly UnitOfWork unitOfWork;

    public ClearFundUseCase(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void Execute()
    {
        unitOfWork.FundNavRepository.Clear();
        
        Console.WriteLine("All fund records have been cleared.");
    }
}