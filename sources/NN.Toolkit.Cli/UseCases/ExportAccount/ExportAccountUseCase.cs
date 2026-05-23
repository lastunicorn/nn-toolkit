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

    public void Execute()
    {
        string exportFormatSafe = exportFormat ?? "pp";

        switch (exportFormatSafe.ToLower())
        {
            case "pp":
                IEnumerable<Contribution> contributions = unitOfWork.ContributionRepository.GetAll();
                ExportToCsv(contributions);
                break;

            default:
                Console.WriteLine($"Export format '{exportFormat}' is not supported.");
                break;
            {
            }
        }
    }

    private static void ExportToCsv(IEnumerable<Contribution> contributions)
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
        
        using NnTransactionsFile nnTransactionsFile = new("NN_transactions.csv", labels);
        using NnCashTransactionsFile nnCashTransactionsFile = new("NN_cash_transactions.csv");

        foreach (Contribution contribution in contributions)
        {
            nnTransactionsFile.Write(contribution);
            nnCashTransactionsFile.Write(contribution);
        }

        Console.WriteLine();
        Console.WriteLine("Account contributions were exported to CSV files:");
        Console.WriteLine("  - NN_transactions.csv");
        Console.WriteLine("  - NN_cash_transactions.csv");
    }
}