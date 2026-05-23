using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Export;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases;

internal class ExportAccountUseCase : IUseCase
{
    private readonly string exportFormat;
    private readonly ContributionRepository contributionRepository;

    public ExportAccountUseCase(string exportFormat, ContributionRepository contributionRepository)
    {
        this.exportFormat = exportFormat;
        this.contributionRepository = contributionRepository ?? throw new ArgumentNullException(nameof(contributionRepository));
    }

    public void Execute()
    {
        string exportFormatSafe = exportFormat ?? "pp";

        switch (exportFormatSafe.ToLower())
        {
            case "pp":
                IEnumerable<Contribution> contributions = contributionRepository.GetAll();
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