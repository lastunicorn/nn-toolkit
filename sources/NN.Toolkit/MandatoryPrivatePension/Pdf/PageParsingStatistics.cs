namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

public class PageParsingStatistics
{
    public int PageIndex { get; }
    
    public List<TableParsingStatistics> Tables { get; } = [];

    public Exception Error { get; set; }
    
    public bool HasErrors => Error != null ||  Tables.Any(x => x.HasErrors);

    public PageParsingStatistics(int pageIndex)
    {
        PageIndex = pageIndex;
    }

    public TableParsingStatistics AddTable()
    {
        TableParsingStatistics tableParsingStatistics = new();
        Tables.Add(tableParsingStatistics);
        return tableParsingStatistics;
    }
}