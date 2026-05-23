namespace DustInTheWind.NN.Toolkit.Cli.Ports.NnAccess;

internal interface INnApiClient
{
	Task<IEnumerable<NnGraphValue>> GetGraph(DateOnly startDate, DateOnly endDate, int count);
}