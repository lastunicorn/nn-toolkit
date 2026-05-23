using DustInTheWind.NN.Toolkit.Cli;

public class HelpUseCase : IUseCase
{
    public void Execute()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  account import --file <filePath>");
        Console.WriteLine("  account clear");
        Console.WriteLine("  account export --format <formatName>");
        Console.WriteLine("  account");
        Console.WriteLine();
        Console.WriteLine("  fund import --from <date> --to <date>");
        Console.WriteLine("  fund import --year <year>");
        Console.WriteLine("  fund import --file <filePath>");
        Console.WriteLine("  fund clear");
        Console.WriteLine("  fund");
        Console.WriteLine();
        Console.WriteLine("  help");
    }
}