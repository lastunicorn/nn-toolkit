using DustInTheWind.NN.Toolkit.Demo.UseCases;

namespace DustInTheWind.NN.Toolkit.Demo;

internal static class Program
{
	internal static async Task Main(string[] args)
	{
		// Uncomment the use case you want to run.

		//await ParseContributionFile();
		await DisplayPensionFundFromWeb();
	}

	private static Task ParseContributionFile()
	{
		ParseContributionFileUseCase useCase = new();
		return useCase.Execute();
	}

	private static Task DisplayPensionFundFromWeb()
	{
		DisplayPensionFundFromWebUseCase useCase = new();
		return useCase.Execute();
	}
}