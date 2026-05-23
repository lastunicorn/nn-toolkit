using DustInTheWind.ConsoleTools;

namespace DustInTheWind.NN.Toolkit.Cli.ConsoleUtils;

internal class XConsole
{
    private ConsoleColor? foregroundColor;
    private ConsoleColor? backgroundColor;

    private XConsole()
    {
    }

    public static XConsole Create()
    {
        return new XConsole();
    }

    public XConsole With(ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
    {
        this.foregroundColor = foregroundColor;
        this.backgroundColor = backgroundColor;

        return this;
    }

    public XConsole WriteLine()
    {
        Console.WriteLine();
        return this;
    }

    public XConsole WriteLine(string text)
    {
        CustomConsole.WithColors(foregroundColor, backgroundColor, () => Console.WriteLine(text));
        return this;
    }
}