using DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Ports.Export;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ExportAccount;

internal class ExportAccountUseCase : IUseCase
{
    private readonly string exportFormat;
    private readonly IUnitOfWork unitOfWork;

    public ExportAccountUseCase(string exportFormat, IUnitOfWork unitOfWork)
    {
        this.exportFormat = exportFormat;
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Execute()
    {
        string exportFormatSafe = exportFormat ?? "pp";

        switch (exportFormatSafe.ToLower())
        {
            case "pp":
                IEnumerable<Contribution> contributions = unitOfWork.ContributionRepository.GetAll();
                await ExportToCsv(contributions);
                break;

            default:
                Console.WriteLine($"Export format '{exportFormat}' is not supported.");
                break;
        }
    }

    private static async Task ExportToCsv(IEnumerable<Contribution> contributions)
    {
        string[] labels =
        [
            "Luna",
            "Contribuție brută (lei)",
            "Comision de administrare (lei)",
            "Contribuție netă (lei)",
            "Valoare unitate de fond (lei)",
            "Număr unități de fond",
            "Plătită în luna"
        ];
        
        await using NnTransactionsFile nnTransactionsFile = new("NN_transactions.csv", labels);
        await using NnCashTransactionsFile nnCashTransactionsFile = new("NN_cash_transactions.csv");

        foreach (Contribution contribution in contributions)
        {
            await nnTransactionsFile.WriteAsync(contribution);
            await nnCashTransactionsFile.WriteAsync(contribution);
        }

        Console.WriteLine();
        Console.WriteLine("Account contributions were exported to CSV files:");
        Console.WriteLine("  - NN_transactions.csv");
        Console.WriteLine("  - NN_cash_transactions.csv");
    }
}