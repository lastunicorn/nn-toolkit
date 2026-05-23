using DustInTheWind.ConsoleTools.Arguments;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ClearAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ExportAccount;
using DustInTheWind.NN.Toolkit.Cli.UseCases.Help;
using DustInTheWind.NN.Toolkit.Cli.UseCases.ImportAccount;
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

        switch (noun.Value)
        {
            case "help":
                return new HelpUseCase();

            case "account":
                Argument verb = arguments[1];
                if (verb?.Type == ArgumentType.Ordinal)
                    switch (verb.Value)
                    {
                        case "import":
                        {
                            Argument file = arguments["file"] ?? arguments[2];
                            return new ImportAccountUseCase(file?.Value, new ContributionRepository());
                        }

                        case "export":
                        {
                            Argument format = arguments["format"];
                            return new ExportAccountUseCase(format?.Value, new ContributionRepository());
                        }

                        case "clear":
                            return new ClearAccountUseCase(new ContributionRepository());
                    }

                return new ShowAccountUseCase();

            case "fund":
                return new ShowFundUseCase();

            default:
                throw new Exception("Unknown command " + noun);
        }
    }
}