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

	public async Task Execute()
	{
		unitOfWork.FundNavRepository.Clear();
		await unitOfWork.SaveChangesAsync();

		CustomConsole.WriteLineSuccess("All fund records have been cleared.");
	}
}