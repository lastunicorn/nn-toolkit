using DustInTheWind.ConsoleTools.Arguments;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ClearAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ClearFund;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ExportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ExportFund;
using DustInTheWind.NN.Toolkit.Cli.UseCases.Help;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromFile;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ShowAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ShowFund;

namespace DustInTheWind.NN.Toolkit.Cli;

// account import [<pdf-file-path>] - Imports 
// account import [--file <pdf-file-path>]
// account clear
// account export [--format pp]
// account

// fund import --from 2026-01-01 --to 2026-12-31
// fund import --year 2026
// fund import --file "historical_2008.csv"
// fund clear
// fund

// help

internal static class Program
{
    internal static void Main(string[] args)
    {
        Arguments arguments = new(args);

        IUseCase useCase = CreateUseCase(arguments) ?? new HelpUseCase();
        useCase.Execute();
    }

    private static IUseCase CreateUseCase(Arguments arguments)
    {
        if (arguments.Count == 0)
            return null;

        Argument noun = arguments[0];
        if (noun?.Type != ArgumentType.Ordinal)
            return null;

        return TryCreateImportAccountUseCase(arguments)
               ?? TryCreateExportAccountUseCase(arguments)
               ?? TryCreateClearAccountUseCase(arguments)
               ?? TryCreateShowAccountUseCase(arguments)
               ?? TryCreateImportFundFromFileUseCase(arguments)
               ?? TryCreateExportFundUseCase(arguments)
               ?? TryCreateClearFundUseCase(arguments)
               ?? TryCreateShowFundUseCase(arguments)
               ?? TryCreateHelpUseCase(arguments);
    }

    private static IUseCase TryCreateImportAccountUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "account")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "import")
            return null;

        Argument fileArgument = arguments["file"] ?? arguments[2];

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ImportAccountUseCase(fileArgument?.Value, unitOfWork);
    }

    private static IUseCase TryCreateExportAccountUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "account")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "export")
            return null;

        Argument formatArgument = arguments["format"];

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ExportAccountUseCase(formatArgument?.Value, unitOfWork);
    }

    private static IUseCase TryCreateClearAccountUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "account")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "clear")
            return null;

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ClearAccountUseCase(unitOfWork);
    }

    private static IUseCase TryCreateShowAccountUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "account")
            return null;

        Argument verb = arguments[1];

        if (verb != null && verb.Value != "show")
            return null;

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ShowAccountUseCase(unitOfWork);
    }

    private static IUseCase TryCreateImportFundFromFileUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "fund")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "import")
            return null;

        Argument sourceArgument = arguments["source"];

        if (sourceArgument != null)
        {
            if (sourceArgument.Value == "file")
            {
                Argument fileArgument = arguments["file"];

                Database database = new();
                UnitOfWork unitOfWork = new(database);
                FileSystemService fileSystemService = new();
                return new ImportFundFromFileUseCase(fileArgument?.Value, unitOfWork, fileSystemService);
            }
        }
        else
        {
            Argument fileArgument = arguments["file"];

            if (fileArgument != null)
            {
                Database database = new();
                UnitOfWork unitOfWork = new(database);
                FileSystemService fileSystemService = new();
                return new ImportFundFromFileUseCase(fileArgument.Value, unitOfWork, fileSystemService);
            }
        }

        return null;
    }

    private static IUseCase TryCreateExportFundUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "fund")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "export")
            return null;

        Argument fileArgument = arguments["file"];

        if (fileArgument == null)
            return null;

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        FileSystemService fileSystemService = new();
        return new ExportFundUseCase(fileArgument.Value, unitOfWork, fileSystemService);
    }

    private static IUseCase TryCreateClearFundUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "fund")
            return null;

        Argument verb = arguments[1];

        if (verb?.Value != "clear")
            return null;

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ClearFundUseCase(unitOfWork);
    }

    private static ShowFundUseCase TryCreateShowFundUseCase(Arguments arguments)
    {
        Argument noun = arguments[0];

        if (noun?.Value != "fund")
            return null;

        Argument verb = arguments[1];

        if (verb != null && verb.Value != "show")
            return null;

        Database database = new();
        UnitOfWork unitOfWork = new(database);
        return new ShowFundUseCase(unitOfWork);
    }

    private static IUseCase TryCreateHelpUseCase(Arguments arguments)
    {
        return new HelpUseCase();
    }
}