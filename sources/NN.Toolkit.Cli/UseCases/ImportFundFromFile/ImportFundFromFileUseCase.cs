using CsvHelper;
using CsvHelper.Configuration;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Domain;
using DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;
using System.Globalization;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromFile;

internal class ImportFundFromFileUseCase : IUseCase
{
    private readonly string filePath;
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileSystemService fileSystemService;

    public ImportFundFromFileUseCase(string filePath, IUnitOfWork unitOfWork, IFileSystemService fileSystemService)
    {
        this.filePath = filePath;
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
    }

    public void Execute()
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        IEnumerable<string> filePaths = fileSystemService.IsDirectory(filePath)
            ? fileSystemService.GetFiles(filePath, "*.csv")
            : [filePath];

        ImportDiagnostics totalDiagnostics = new();

        foreach (string path in filePaths)
        {
            IEnumerable<FundNav> fundNavs = ReadFromFile(path);
            ImportDiagnostics importDiagnostics = Import(fundNavs);
            DisplayImportDiagnostics(Path.GetFileName(path), importDiagnostics);

            totalDiagnostics.AddCount += importDiagnostics.AddCount;
            totalDiagnostics.UpdateCount += importDiagnostics.UpdateCount;
            totalDiagnostics.SkipCount += importDiagnostics.SkipCount;
        }

        CustomConsole.WriteLineSuccess("Fund values imported successfully");
        DisplayImportDiagnostics("Total", totalDiagnostics);

        unitOfWork.SaveChanges();
    }

    private IEnumerable<FundNav> ReadFromFile(string path)
    {
        Console.WriteLine($"Importing {path} ...");

        CsvConfiguration config = new(CultureInfo.InvariantCulture);

        using StreamReader streamReader = fileSystemService.OpenStreamReader(path);
        using CsvReader csv = new(streamReader, config);
        csv.Context.RegisterClassMap<FundNavMap>();

        return csv.GetRecords<FundNav>().ToList();
    }

    private ImportDiagnostics Import(IEnumerable<FundNav> fundNavs)
    {
        ImportDiagnostics importDiagnostics = new();

        foreach (FundNav fundNav in fundNavs)
        {
            FundNav existingFundNav = unitOfWork.FundNavRepository.Get(fundNav.Date);

            if (existingFundNav == null)
            {
                unitOfWork.FundNavRepository.Add(fundNav);
                importDiagnostics.AddCount++;
            }
            else if (existingFundNav.Value == fundNav.Value)
            {
                importDiagnostics.SkipCount++;
            }
            else
            {
                existingFundNav.Value = fundNav.Value;
                importDiagnostics.UpdateCount++;
            }
        }

        return importDiagnostics;
    }

    private void DisplayImportDiagnostics(string title, ImportDiagnostics importDiagnostics)
    {
        DataGrid diagnosticsGrid = new()
        {
            Title = title,
            Margin = new Thickness(0, 1, 0, 1)
        };

        diagnosticsGrid.Columns.Add("Name", HorizontalAlignment.Left);
        diagnosticsGrid.Columns.Add("Value", HorizontalAlignment.Right);

        diagnosticsGrid.Rows.Add("Add", importDiagnostics.AddCount);
        diagnosticsGrid.Rows.Add("Update", importDiagnostics.UpdateCount);
        diagnosticsGrid.Rows.Add("Skip", importDiagnostics.SkipCount);

        diagnosticsGrid.Display();
    }
}