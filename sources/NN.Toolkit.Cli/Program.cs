using DustInTheWind.ConsoleTools.Arguments;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ClearAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ClearFund;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ExportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.Help;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromFile;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportFundFromWeb;
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

        return noun.Value switch
        {
            "help" => new HelpUseCase(),
            "account" => HandleAccountCommand(arguments),
            "fund" => HandleFundCommand(arguments),
            _ => throw new Exception("Unknown command: " + noun)
        };
    }

    private static IUseCase HandleAccountCommand(Arguments arguments)
    {
        Argument verb = arguments[1];

        if (verb == null)
            return new ShowAccountUseCase(new UnitOfWork(new Database()));

        switch (verb.Type)
        {
            case ArgumentType.Ordinal:
                switch (verb.Value)
                {
                    case "import":
                    {
                        Argument file = arguments["file"] ?? arguments[2];
                        return new ImportAccountUseCase(file?.Value, new UnitOfWork(new Database()));
                    }

                    case "export":
                    {
                        Argument format = arguments["format"];
                        return new ExportAccountUseCase(format?.Value, new UnitOfWork(new Database()));
                    }

                    case "clear":
                        return new ClearAccountUseCase(new UnitOfWork(new Database()));
                }

                break;

            case ArgumentType.Named:
                return new ShowAccountUseCase(new UnitOfWork(new Database()));
        }

        throw new Exception("Unknown command: account " + verb.Value);
    }

    private static IUseCase HandleFundCommand(Arguments arguments)
    {
        Argument verb = arguments[1];

        if (verb == null)
            return new ShowFundUseCase(new UnitOfWork(new Database()));

        switch (verb.Type)
        {
            case ArgumentType.Ordinal:
                switch (verb.Value)
                {
                    case "import":
                    {
                        Argument fileArgument = arguments["file"];

                        if (fileArgument != null)
                        {
                            UnitOfWork unitOfWork = new(new Database());
                            FileSystemService fileSystemService = new();
                            return new ImportFundFromFileUseCase(fileArgument.Value, unitOfWork, fileSystemService);
                        }

                        Argument fromArgument = arguments["from"];
                        Argument toArgument = arguments["to"];
                        Argument yearArgument = arguments["year"];

                        if (fromArgument != null || toArgument != null || yearArgument != null)
                        {
                            DateOnly? from = fromArgument != null
                                ? DateOnly.Parse(fromArgument.Value)
                                : null;

                            DateOnly? to = toArgument != null
                                ? DateOnly.Parse(toArgument.Value)
                                : null;

                            uint? year = yearArgument != null
                                ? uint.Parse(yearArgument.Value)
                                : null;

                            return new ImportFundFromWebUseCase(from, to, year, new UnitOfWork(new Database()));
                        }
                        
                        throw new Exception("Unknown command: fund " + verb.Value);
                    }

                    case "export":
                    {
                        throw new NotImplementedException();
                        // Argument format = arguments["format"];
                        // return new ExportAccountUseCase(format?.Value, new UnitOfWork(new Database()));
                    }

                    case "clear":
                        return new ClearFundUseCase(new UnitOfWork(new Database()));
                }

                break;

            case ArgumentType.Named:
                return new ShowFundUseCase(new UnitOfWork(new Database()));
        }

        throw new Exception("Unknown command: fund " + verb.Value);
    }
}