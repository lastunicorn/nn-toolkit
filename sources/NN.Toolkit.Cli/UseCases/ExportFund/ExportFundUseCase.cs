using CsvHelper;
using DustInTheWind.NN.Toolkit.Cli.Domain;
using DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromFile;
using System.Globalization;
using DustInTheWind.ConsoleTools;
using DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ExportFund;

internal class ExportFundUseCase : IUseCase
{
    private readonly string filePath;
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileSystemService fileSystemService;

    public ExportFundUseCase(string filePath, IUnitOfWork unitOfWork, IFileSystemService fileSystemService)
    {
        this.filePath = filePath;
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
    }

    public async Task Execute()
    {
        if (filePath == null)
            throw new ArgumentNullException(nameof(filePath));

        IEnumerable<FundNav> fundNavs = unitOfWork.FundNavRepository.GetAll();

        await using StreamWriter writer = fileSystemService.OpenStreamWriter(filePath);
        await using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<FundNavMap>();
        csv.Context.TypeConverterOptionsCache.GetOptions<DateOnly>().Formats = ["yyyy-MM-dd"];
        await csv.WriteRecordsAsync(fundNavs);

        Console.WriteLine();
        CustomConsole.WriteLineSuccess($"Fund NAV values were exported to: {filePath}");
    }
}