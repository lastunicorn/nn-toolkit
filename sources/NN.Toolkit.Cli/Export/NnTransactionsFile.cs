using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.Export;

internal sealed class NnTransactionsFile : IDisposable, IAsyncDisposable
{
    private readonly IList<string> labels;
    private readonly StreamWriter output;

    public NnTransactionsFile(string filePath, IList<string> labels)
    {
        this.labels = labels;
        
        output = new StreamWriter(filePath);
        output.WriteLine("Security Name,Ticker Symbol,Date,Time,Value,Shares,Type,Fees,Note");
    }

    public void Write(Contribution contribution)
    {
        string date = $"{contribution.PaidInMonth.Year:00}-{contribution.PaidInMonth.Month:00}-01";
        string note = labels == null
            ? string.Empty
            : string.Join("; ", labels.Zip(contribution.ToStringArray(), (h, v) => $"{h}={v}"));

        output.WriteLine($"NN,NN,{date},08:05,{contribution.GrossValue},{contribution.UnitCount},Buy,{contribution.AdministrationFee},\"{note}\"");
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