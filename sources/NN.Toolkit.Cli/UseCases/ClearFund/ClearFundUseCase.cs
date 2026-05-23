using DustInTheWind.ConsoleTools;
using DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

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
        unitOfWork.SaveChanges();
        
        CustomConsole.WriteLineSuccess("All fund records have been cleared.");
    }
}