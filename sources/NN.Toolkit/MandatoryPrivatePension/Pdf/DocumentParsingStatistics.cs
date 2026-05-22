namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

public class DocumentParsingStatistics
{
    public List<PageParsingStatistics> Pages { get; } = [];

    public Exception Error { get; set; }
    
    public bool HasErrors => Pages.Any(x => x.Error != null);

    public PageParsingStatistics AddPage(int pageIndex)
    {
        PageParsingStatistics pageParsingStatistics = new(pageIndex);
        Pages.Add(pageParsingStatistics);
        return pageParsingStatistics; 
    }
}