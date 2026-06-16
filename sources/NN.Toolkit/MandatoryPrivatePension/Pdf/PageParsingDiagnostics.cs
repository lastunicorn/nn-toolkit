namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

public class PageParsingDiagnostics
{
	public int PageIndex { get; }

	public TableExtractionApproach TableExtractionApproach { get; set; }

	public List<TableParsingDiagnostics> Tables { get; } = [];

	public Exception Error { get; set; }

	public bool HasErrors => Error != null || Tables.Any(x => x.HasErrors);

	public PageParsingDiagnostics(int pageIndex)
	{
		PageIndex = pageIndex;
	}

	public TableParsingDiagnostics AddTable()
	{
		TableParsingDiagnostics tableParsingDiagnostics = new();
		Tables.Add(tableParsingDiagnostics);
		return tableParsingDiagnostics;
	}
}