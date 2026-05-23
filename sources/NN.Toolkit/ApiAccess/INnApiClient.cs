namespace DustInTheWind.NN.Toolkit.ApiAccess;

public interface INnApiClient
{
	Task<GraphData> GetGraph(DateOnly startDate, DateOnly endDate, int count);
}