using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.Ports.Export;

internal sealed class NnCashTransactionsFile : IDisposable, IAsyncDisposable
{
    private readonly StreamWriter output;

    public NnCashTransactionsFile(string filePath)
    {
        output = new StreamWriter(filePath);
        output.WriteLine("Type,Cash Account,Date,Time,Value,Note");
    }

    public Task WriteAsync(Contribution contribution)
    {
        string date = $"{contribution.PaidInMonth.Year:00}-{contribution.PaidInMonth.Month:00}-01";
        return output.WriteLineAsync($"Deposit,NN,{date},08:00,{contribution.GrossValue},\"Luna: {contribution.Month}\"");
    }

    public void Dispose()
    {
        output?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (output != null)
            await output.DisposeAsync();
    }
}