namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

public class DocumentParsingDiagnostics
{
	public List<PageParsingDiagnostics> Pages { get; } = [];

	public Exception Error { get; set; }

	public bool HasErrors => Pages.Any(x => x.Error != null);

	public PageParsingDiagnostics AddPage(int pageIndex)
	{
		PageParsingDiagnostics pageParsingDiagnostics = new(pageIndex);
		Pages.Add(pageParsingDiagnostics);
		return pageParsingDiagnostics;
	}
}